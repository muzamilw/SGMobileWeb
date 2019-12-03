using SG2.Core.Web;
using SG2.CORE.BAL.Managers;
using SG2.CORE.MODAL.DTO.Customers;
using SG2.CORE.MODAL.DTO.Statistics;
using SG2.CORE.MODAL.DTO.SystemSettings;
using SG2.CORE.MODAL.ViewModals.Customers;
using SG2.CORE.MODAL.ViewModals.Statistics;
using SG2.CORE.WEB.Architecture;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;
using static SG2.CORE.COMMON.GlobalEnums;

namespace SG2.CORE.WEB.Controllers
{
    [AuthorizeCustomer]
    public class UserController : BaseController
    {
        protected readonly CustomerManager _customerManager;
        protected readonly CommonManager _commonManager;
        protected readonly StatisticsManager _statisticsManager;
        private readonly string _stripeApiKey = string.Empty;
        protected readonly List<SystemSettingsDTO> SystemConfigs;

        public UserController()
        {
            _customerManager = new CustomerManager();
            _commonManager = new CommonManager();
            _statisticsManager = new StatisticsManager();
            //Setting Stripe api key
            if (WebConfigurationManager.AppSettings["IsDebug"] == "1")
            {
                _stripeApiKey = WebConfigurationManager.AppSettings["StripeTestApiKey"];
            }
            else
            {
                _stripeApiKey = WebConfigurationManager.AppSettings["StripeLiveApiKey"];
            }
            SystemConfigs = SystemConfig.GetConfigs;

        }

        public ActionResult Home()
        {
            ViewBag.CurrentUser = this.CDT;
            ViewBag.SocailProfiles = this._customerManager.GetSocialProfilesByCustomerid(this.CDT.CustomerId);
            //StatisticsDTO statisticsDTO = _statisticsManager.GetFollersStatistics(1, DateTime.Now.AddDays(-7), DateTime.Now.AddDays(+5), "Weeekly");
            //FollowersStatisticsViewModel model = new FollowersStatisticsViewModel();
            //model.SocialProfileId = statisticsDTO.SocialProfileId;
            //model.Followers = statisticsDTO.Followers;
            //model.FollowersGain = statisticsDTO.FollowersGain;
            //model.Followings = statisticsDTO.Followings;
            //model.WeekDays = statisticsDTO.WeekDays;

            return View();
        }


        [HttpPost]
        public ActionResult CreateNewProfile(CustomerAddProfileRequest model)
        {
            var jr = new JsonResult();
            try
            {
                
                var result = _customerManager.AddInstagramSocialProfile(model.IntagramUserName, model.CustomerId);

                if ( result.Succcess == true)
                {
                    jr.Data = new { ResultType = "Success", message = result.Message };
                }
                else
                {
                    jr.Data = new { ResultType = "Error", message = result.Message };
                }
               

            }
            catch (Exception exp)
            {
                throw exp;
            }

            return Json(jr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserProfile()
        {
            if (!string.IsNullOrEmpty((string)TempData["Success"]))
            {
                ViewBag.Success = (string)TempData["Success"];
                ViewBag.Message = TempData["Message"];
            }

            var user = this.CDT;
            var userUpdate = new CustomerProfileUpdateViewModel()
            {
                FirstName = user.FirstName,
                SurName = user.SurName,
                EmailAddress = user.EmailAddress,
               
                PhoneNumber = "+"+user.PhoneCode + user.PhoneNumber,
                UserName = user.UserName,
                Countries = CommonManager.GetCountries()
            };

            var model = new CustomerProfileViewModel();
            {
                model.CustomerProfileUpdateVM = userUpdate;
                model.CustomerUpdatePasswordVM = new CustomerUpdatePasswordViewModel();
                model.IsOptedEducationalEmailSeries = CDT.IsOptedEducationalEmailSeries;
                model.IsOptedMarketingEmail = CDT.IsOptedMarketingEmail;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateUserProfile(CustomerProfileUpdateViewModel model)
        {
            try
            {
                var jr = new JsonResult();
                if (ModelState.IsValid)
                {

                    var userProfile = _customerManager.UpdateCustomerProfile(new CustomerDTO()
                    {
                        CustomerId = this.CDT.CustomerId,
                        FirstName = model.FirstName,
                        SurName = model.SurName,
                        UserName = model.UserName,
                        PhoneNumber = model.PhoneNumber,
                        PhoneCode = model.PhoneCode
                    });
                    this.CDT.UserName = model.UserName;
                    this.CDT.FirstName = model.FirstName;
                    this.CDT.SurName = model.SurName;
                    this.CDT.PhoneCode = model.PhoneCode;
                    this.CDT.PhoneNumber = model.PhoneNumber;
                    _sessionManager.Set(SessionConstants.Customer, this.CDT);
                    jr.Data = new { ResultType = "Success", message = "Profile updated successfully." };
                }
                else
                {
                    string messages = string.Join(", ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    jr.Data = new { ResultType = "Error", message = messages };
                }
                return jr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult UpdateUserEmailSubscription(bool IsOptedEducationalEmailSeries, bool IsOptedMarketingEmail)
        {
            try
            {
                var jr = new JsonResult();

                var cst = new CustomerDTO()
                {
                    CustomerId = this.CDT.CustomerId,
                    IsOptedEducationalEmailSeries = IsOptedEducationalEmailSeries,
                    IsOptedMarketingEmail = IsOptedMarketingEmail,
                    UpdatedBy = this.CDT.FirstName
                };

                if (_customerManager.UpdateCustomerEmailSubscription(cst) != null)
                {
                    this.CDT.IsOptedEducationalEmailSeries = IsOptedEducationalEmailSeries;
                    this.CDT.IsOptedMarketingEmail = IsOptedMarketingEmail;
                    _sessionManager.Set(SessionConstants.Customer, this.CDT);
                    jr.Data = new { ResultType = "Success", message = "Subscription updated successfully." };
                }
                else
                {
                    jr.Data = new { ResultType = "Error", message = "Error. User no updated. Please contact admin." };
                }

                return jr;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Plan(int SocialProfileId = 0)
        {
            try
            {
                var curCust = (CustomerDTO)_sessionManager.Get(SessionConstants.Customer);

                SocialProfile_PaymentsDTO subscriptionDTO = _customerManager.GetSubscription(SocialProfileId);

                var history = _customerManager.GetCustomerOrderHistory("50", 1, this.CDT.CustomerId, SocialProfileId);

                StripeConfiguration.SetApiKey(_stripeApiKey);
                var planService = new PlanService();
                var planOptions = new PlanListOptions
                {
                    Limit = 4,
                };
                var plans = planService.List(planOptions);

                List<CustomerPaymentPlansViewModel> payPlan = null;
                if (plans != null)
                {
                    payPlan = new List<CustomerPaymentPlansViewModel>();
                    foreach (var plan in plans)
                    {
                        CustomerPaymentPlansViewModel paymentPlansViewModel = new CustomerPaymentPlansViewModel();

                        if (subscriptionDTO.StripePlanId == plan.Id)
                        {
                            paymentPlansViewModel.currentPlan = true;
                        }
                        else
                        {
                            paymentPlansViewModel.currentPlan = false;
                        }

                        paymentPlansViewModel.PlanId = plan.Id;
                        paymentPlansViewModel.PlanName = plan.Nickname;
                        paymentPlansViewModel.PlanStatus = plan.Active;
                        paymentPlansViewModel.Amount = plan.Amount.Value;
                        paymentPlansViewModel.BillingScheme = plan.BillingScheme;
                        paymentPlansViewModel.Currency = plan.Currency;
                        paymentPlansViewModel.Interval = plan.Interval;
                        paymentPlansViewModel.IntervalCount = plan.IntervalCount;
                        paymentPlansViewModel.ProductCode = plan.ProductId;
                        payPlan.Add(paymentPlansViewModel);
                    }
                }

                var cardService = new CardService();
                var cardOptions = new CardListOptions
                {
                    Limit = 3,
                };
                List<CustomerPaymentCardsViewModel> payCards = null;
                if (this.CDT.StripeCustomerId != null)
                {
                    var striptCards = cardService.List(this.CDT.StripeCustomerId, cardOptions);
                    if (striptCards != null)
                    {
                        payCards = new List<CustomerPaymentCardsViewModel>();
                        foreach (var item in striptCards)
                        {
                            var card = new CustomerPaymentCardsViewModel();
                            card.Last4 = item.Last4;
                            card.ExpMonth = item.ExpMonth;
                            card.ExpYear = item.ExpYear;
                            card.Brand = item.Brand;
                            card.Funding = item.Funding;
                            payCards.Add(card);
                        }
                    }
                }

                CustomerPlanDetailViewModel cpdViewModel = new CustomerPlanDetailViewModel();
                cpdViewModel.PaymentPlans = payPlan;
                cpdViewModel.PaymentCards = payCards;
                cpdViewModel.OrderHistoryViewModels = history;
                return View(cpdViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult ConfirmAndPay(CustomerPaymentPlansViewModel model)
        {
            try
            {
                StripeConfiguration.SetApiKey(_stripeApiKey);
                var planService = new PlanService();
                var selPlan = planService.Get(model.PlanId);

                CustomerPaymentPlansViewModel payPlan = new CustomerPaymentPlansViewModel();
                if (selPlan != null)
                {
                    payPlan.PlanId = selPlan.Id;
                    payPlan.PlanName = selPlan.Nickname;
                    payPlan.PlanStatus = selPlan.Active;
                    payPlan.Amount = selPlan.Amount.Value;
                    payPlan.BillingScheme = selPlan.BillingScheme;
                    payPlan.Currency = selPlan.Currency;
                    payPlan.Interval = selPlan.Interval;
                    payPlan.IntervalCount = selPlan.IntervalCount;
                    payPlan.ProductCode = selPlan.ProductId;
                }

                //var paymentMethodService = new PaymentMethodService();
                //var paymentMethodListOptions = new PaymentMethodListOptions
                //{
                //    CustomerId = this.CDT.StripeCustomerId,
                //    Limit = 3,
                //    Type = "card",
                //};
                //var paymentmethods = paymentMethodService.List(paymentMethodListOptions);


                var service = new CardService();
                List<CustomerPaymentCardsViewModel> payCards = new List<CustomerPaymentCardsViewModel>();
                if (this.CDT.StripeCustomerId != null)
                {
                    var options = new CardListOptions
                    {
                        Limit = 3,
                    };
                    var striptCards = service.List(this.CDT.StripeCustomerId, options);


                    foreach (var item in striptCards)
                    {
                        var card = new CustomerPaymentCardsViewModel();
                        card.Last4 = item.Last4;
                        card.ExpMonth = item.ExpMonth;
                        card.ExpYear = item.ExpYear;
                        card.Brand = item.Brand;
                        card.Funding = item.Funding;
                        payCards.Add(card);
                    }
                }
                else
                {

                    var card = new CustomerPaymentCardsViewModel();
                    card.Last4 = "";
                    card.ExpMonth = 0;
                    card.ExpYear = 0;
                    card.Brand = "Visa";
                    card.Funding = "Card";
                    payCards.Add(card);

                }
                var cusPayAadConfirmVM = new CustomerPayAndConfirmViewModel();
                cusPayAadConfirmVM.CardDetails = payCards;
                cusPayAadConfirmVM.PaymentPlan = payPlan;


                return View(cusPayAadConfirmVM);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // customerSubscriptionViewModal.CustomerPaymentDetailsVM = payPlan;
            // return View(payPlan);            
            //_customerManager.UpdateStripeCustomerId(curCust.EmailAddress, "cus_EwmdhsPhDc2zOV");            
            //var subscriptionService = new SubscriptionService();
            //var subscription = subscriptionService.Get("sub_EwmdchSHry5R0d");
            //SubscriptionDTO subscriptionDTO = new SubscriptionDTO();            
            //subscriptionDTO.CustomerId = curCust.CustomerId;
            //subscriptionDTO.StripeSubscriptionId = subscription.Id;
            //subscriptionDTO.Description = subscription.Items.FirstOrDefault().Plan.Nickname;
            //subscriptionDTO.InstaUsrName = subscription.Plan.Nickname;
            //subscriptionDTO.Price = subscription.Plan.Amount;
            //subscriptionDTO.StripePlanId = subscription.Plan.Id;
            //subscriptionDTO.SubscriptionType = subscription.Plan.Interval;
            //subscriptionDTO.StartDate = subscription.Start;
            //subscriptionDTO.EndDate = subscription.EndedAt;

            //_customerManager.InsertSubscription(subscriptionDTO);
            //string StripeCustomerId =  _customerManager.GetStripeCustomerId(curCust.EmailAddress);
            //if (StripeCustomerId == null)
            //{            
            //    var paymentMethodService = new PaymentMethodService();
            //    var paymentMethodListOptions = new PaymentMethodListOptions
            //    {
            //        CustomerId = StripeCustomerId,
            //        Limit = 3,
            //    };
            //    var paymentMethodList = paymentMethodService.List(paymentMethodListOptions);

            //    if (paymentMethodList == null)
            //    {

            //        var paymentMethodCreateOptions = new PaymentMethodCreateOptions
            //        {
            //            Type = "card",
            //            Card = new PaymentMethodCardCreateOptions
            //            {
            //                Number = "4242424242424242",
            //                ExpMonth = 4,
            //                ExpYear = 2020,
            //                Cvc = "123",
            //            },
            //        };

            //        var newPaymentMethod = paymentMethodService.Create(paymentMethodCreateOptions);

            //        var customerCreateOptions = new CustomerCreateOptions
            //        {
            //            Description = "Customer for SG2@example.com",
            //            SourceToken = "tok_visa",
            //            Address = new AddressOptions
            //            {
            //                City = "Lahore",
            //                Country = "Pakistan",
            //                PostalCode = "54000",
            //                Line1 = "Abc street",
            //                Line2 = "PIA road",
            //                State = "Punjab"
            //            },

            //            InstaUsrName = "Raza",
            //            Email = "ssaqibshirazi@gmail.com",
            //            PaymentMethodId = newPaymentMethod.Id

            //        };

            //        var customerService = new CustomerService();
            //        Customer customer = customerService.Create(customerCreateOptions);

            //        _customerManager.UpdateStripeCustomerId(curCust.EmailAddress, customer.Id);

            //    }
            //    else
            //    {


            //    }

            //}



        }

        //[HttpPost]
        //public ActionResult ConfirmPayment(CustomerPayAndConfirmViewModel model)
        //{
        //    try
        //    {

        //        StripeConfiguration.SetApiKey(_stripeApiKey);
        //        Subscription subscription = new Subscription();
        //        SocialProfile_PaymentsDTO subscriptionDTO = new SocialProfile_PaymentsDTO();
        //        var subscriptionService = new SubscriptionService();

        //        if (this.CDT.StripeCustomerId != null)
        //        {
        //            subscriptionDTO = _customerManager.GetSubscription(this.CDT.CustomerId);
        //            DateTime prorationDate = DateTime.Now;
        //            if (subscriptionDTO.StripeSubscriptionId != null)
        //            {
        //                Subscription subscriptionItemUpdate = subscriptionService.Get(subscriptionDTO.StripeSubscriptionId);

        //                var items = new List<SubscriptionItemUpdateOption> {
        //                                new SubscriptionItemUpdateOption {
        //                                Id= subscriptionItemUpdate.Items.Data[0].Id,
        //                                Plan = model.PlanId,
        //                                Quantity= 1,
        //                                },
        //                            };
        //                var subscriptionUpdateoptions = new SubscriptionUpdateOptions
        //                {
        //                    Items = items,
        //                   // Billing = Billing.ChargeAutomatically,
        //                    //  billingcycleanchor = datetime.now,
        //                    BillingThresholds = { },
        //                    Prorate = true,
        //                    // cancelat = datetime.now.adddays(30),
        //                    //  daysuntildue = 5,
        //                    // DefaultPaymentMethodId = paymentmethod.id,
        //                   // BillingCycleAnchorNow = true,
        //                    //BillingCycleAnchorUnchanged = true,
        //                    ProrationDate = prorationDate,
        //                };
        //                subscription = subscriptionService.Update(subscriptionDTO.StripeSubscriptionId, subscriptionUpdateoptions);
        //            }
        //            else
        //            {
        //                var items = new List<SubscriptionItemOption> {
        //                              new SubscriptionItemOption {
        //                                Plan = model.PlanId,
        //                                Quantity= 1
        //                              }
        //                            };

        //                var subscriptionCreateoptions = new SubscriptionCreateOptions
        //                {
        //                    Customer = this.CDT.StripeCustomerId,
        //                    Items = items,
        //                    //Billing = Billing.ChargeAutomatically,
        //                    //  billingcycleanchor = datetime.now,
        //                    BillingThresholds = { },
        //                    // cancelat = datetime.now.adddays(30),
        //                    //  daysuntildue = 5,
        //                    // DefaultPaymentMethodId = paymentmethod.id,
        //                };
        //                subscription = subscriptionService.Create(subscriptionCreateoptions);
        //            }

        //        }
        //        else
        //        {


        //            var paymentMethodCreateOptions = new PaymentMethodCreateOptions
        //            {
        //                Type = "card",
        //                Card = new PaymentMethodCardCreateOptions
        //                {
        //                    Number = "4242424242424242",
        //                    ExpMonth = 4,
        //                    ExpYear = 2020,
        //                    Cvc = "123",
        //                },
        //            };

        //            var paymentMethodService = new PaymentMethodService();
        //            var paymentMethod = paymentMethodService.Create(paymentMethodCreateOptions);

        //            var customerCreateOptions = new CustomerCreateOptions
        //            {
        //                Description = " Customer for Social Growth" + this.CDT.EmailAddress,
        //                //SourceToken = "tok_visa",
        //                Address = new AddressOptions
        //                {
        //                    City = "Lahore",
        //                    Country = "Pakistan",
        //                    PostalCode = "54000",
        //                    Line1 = "Abc street",
        //                    Line2 = "PIA road",
        //                    State = "Punjab"
        //                },

        //                Name = this.CDT.FirstName + " " + this.CDT.UserName,
        //                Email = this.CDT.EmailAddress,
        //                PaymentMethod = paymentMethod.Id
        //            };

        //            var customerService = new CustomerService();
        //            Customer stripeCustomer = customerService.Create(customerCreateOptions);

        //            _customerManager.UpdateSocialProfileStripeCustomer(this.CDT.CustomerId, stripeCustomer.Id);


        //            var items = new List<SubscriptionItemOption> {
        //              new SubscriptionItemOption {
        //                Plan = model.PlanId,
        //                Quantity= 1
        //              }
        //            };

        //            var subscriptionCreateOptions = new SubscriptionCreateOptions
        //            {
        //                Customer = stripeCustomer.Id,
        //                Items = items,
        //                //Billing = Billing.ChargeAutomatically,
        //                //  BillingCycleAnchor = DateTime.Now,
        //                BillingThresholds = { },
        //                // CancelAt = DateTime.Now.AddDays(30),
        //                //  DaysUntilDue = 5,
        //                DefaultPaymentMethod = paymentMethod.Id,
        //            };


        //            subscription = subscriptionService.Create(subscriptionCreateOptions);

        //        }


        //        //SocialProfile_PaymentsDTO subDTO = new SocialProfile_PaymentsDTO();
        //        //subDTO.CustomerId = this.CDT.CustomerId;
        //        //subDTO.StripeSubscriptionId = subscription.Id;
        //        //subDTO.Description = subscription.Plan.Nickname;
        //        //subDTO.Name = subscription.Plan.Nickname;
        //        //subDTO.Price = subscription.Plan.Amount;
        //        //subDTO.StripePlanId = subscription.Plan.Id;
        //        //subDTO.SubscriptionType = subscription.Plan.Interval;
        //        //subDTO.StartDate = subscription.StartDate ?? DateTime.Now;
        //        //subDTO.EndDate = subscription.EndedAt ?? DateTime.Now.AddMonths(1);

        //        //if (subscriptionDTO.SubscriptionId != 0)
        //        //{
        //        //    subDTO.SubscriptionId = subscriptionDTO.SubscriptionId;
        //        //    _customerManager.UpdateSubscription(subDTO);
        //        //}
        //        //else
        //        //{
        //        //    _customerManager.InsertSubscription(subDTO);
        //        //}

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;

        //    }

        //    return View();
        //}

        public JsonResult CheckCurrentPassword(string CurrentPassword)
        {
            try
            {
                return this.CDT.Password == CurrentPassword ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult UpdateUserPassword(CustomerUpdatePasswordViewModel model)
        {
            try
            {
                var jr = new JsonResult();
                if (ModelState.IsValid)
                {
                    if (_customerManager.UpdateCustomerPassword(model.Password, this.CDT.CustomerId))
                    {
                        this.CDT.Password = model.Password;
                        _sessionManager.Set(SessionConstants.Customer, this.CDT);
                        jr.Data = new { ResultType = "Success", message = "Password updated successfully." };
                    }
                }
                else
                {
                    string messages = string.Join(", ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    jr.Data = new { ResultType = "Error", message = messages };
                }
                return jr;
            }
            catch (Exception)
            {

                throw;
            }
        }

      

        public ActionResult Confirmdelete()
        {
            var jr = new JsonResult();
            try
            {

                //StripeConfiguration.SetApiKey(_stripeApiKey);
                //var service = new SubscriptionService();
                //var subscription = service.Cancel(this.CDT.StripeSubscriptionId, null);

                ////--TODO: Check subscription status here
                //SocialProfile_PaymentsDTO subDTO = new SocialProfile_PaymentsDTO();
                //subDTO.CustomerId = this.CDT.CustomerId;
                //subDTO.StripeSubscriptionId = subscription.Id;
                //subDTO.Description = subscription.Plan.Nickname;
                //subDTO.Name = subscription.Plan.Nickname;
                //subDTO.Price = subscription.Plan.Amount;
                //subDTO.StripePlanId = subscription.Plan.Id;
                //subDTO.SubscriptionType = subscription.Plan.Interval;
                //subDTO.StartDate = subscription.StartDate ?? DateTime.Now;
                //subDTO.EndDate = subscription.EndedAt ?? DateTime.Now.AddMonths(1);
                //subDTO.StatusId = 26;// Canceled Subscription
                //_customerManager.InsertSubscription(subDTO);

                //int socialProfileId = 1;//TODO: Social Profile Id
                //if (_customerManager.DeleteCustomer(this.CDT.CustomerId, socialProfileId))
                //{

                //    jr.Data = new { ResultType = "Success", message = "User has successfully deleted." };
                //}
                //else
                //{
                //    jr.Data = new { ResultType = "Error", message = "User has successfully deleted." };
                //}
            }
            catch (Exception exp)
            {
                throw exp;
            }



            return Json(jr, JsonRequestBehavior.AllowGet);
        }
       
        //public ActionResult GetFollowersStatistics()
        //{
        //    var jr = new JsonResult();
        //    try
        //    {
                
        //        StatisticsViewModel statisticsViewModel = _statisticsManager.GetStatistics(8, DateTime.Now.AddDays(-15), DateTime.Now.AddDays(+5));
                
        //        if (statisticsViewModel.StatisticsListing !=  null)
        //        {
        //            jr.Data = new { ResultType = "Success", message = "",
        //                  ResultData = new {
        //                                        Date = statisticsViewModel.StatisticsListing.Select(x => x.Date.ToString("dd/MM/yyyy")).ToArray(),
        //                                        FollowersData = statisticsViewModel.StatisticsListing.Select(x => x.Followers.ToString()).ToArray(),
        //                                        FollowersGainData = statisticsViewModel.StatisticsListing.Select(x => x.FollowersGain.ToString()).ToArray(),
        //                                        FollowingsData = statisticsViewModel.StatisticsListing.Select(x => x.Followings.ToString()).ToArray(),
        //                                        FollowingsRatioData = statisticsViewModel.StatisticsListing.Select(x => x.FollowingsRatio.ToString()).ToArray(),
        //                                        AVGFollowersData  = statisticsViewModel.StatisticsListing.Select(x => x.AVGFollowers.ToString()).ToArray(),


        //                                        LikeData = statisticsViewModel.StatisticsListing.Select(x => x.Like.ToString()).ToArray(),
        //                                        CommentData = statisticsViewModel.StatisticsListing.Select(x => x.Comment.ToString()).ToArray(),
        //                                        LikeCommentData = statisticsViewModel.StatisticsListing.Select(x => x.LikeComments.ToString()).ToArray(),
        //                                        Engagement = statisticsViewModel.StatisticsListing.Select(x => x.Engagement.ToString()).ToArray(),


        //                                        TotalComment = statisticsViewModel.TotalComment.ToString(),
        //                                        TotalEngagement = statisticsViewModel.TotalEngagement.ToString(),
        //                                        TotalFollowers = statisticsViewModel.TotalFollowers.ToString(),
        //                                        TotalFollowersGain = statisticsViewModel.TotalFollowersGain.ToString(),
        //                                        TotalFollowings = statisticsViewModel.TotalFollowings.ToString(),
        //                                        TotalFollowingsRatio = statisticsViewModel.TotalFollowingsRatio.ToString(),
        //                                        TotalLike = statisticsViewModel.TotalLike.ToString(),
        //                                        TotalLikeComment = statisticsViewModel.TotalLikeComment.ToString()
        //                                    } };
        //        }
        //        else
        //        {
        //            jr.Data = new { ResultType = "Error", message = "" };
        //        }

        //    }
        //    catch (Exception exp)
        //    {
        //        throw exp;
        //    }
            
        //    return Json(jr, JsonRequestBehavior.AllowGet);
        //}

    }
}