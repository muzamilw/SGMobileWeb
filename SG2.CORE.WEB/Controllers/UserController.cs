using klaviyo.net;
using SG2.Core.Web;
using SG2.CORE.BAL.Managers;
using SG2.CORE.COMMON;
using SG2.CORE.MODAL.DTO.Customers;
using SG2.CORE.MODAL.DTO.Notification;
using SG2.CORE.MODAL.DTO.Statistics;
using SG2.CORE.MODAL.DTO.SystemSettings;
using SG2.CORE.MODAL.klaviyo;
using SG2.CORE.MODAL.ViewModals.Customers;
using SG2.CORE.MODAL.ViewModals.Statistics;
using SG2.CORE.WEB.Architecture;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        protected readonly PlanInformationManager _planInformationManager;
        protected readonly NotificationManager _notManager;

        public UserController()
        {
            _customerManager = new CustomerManager();
            _commonManager = new CommonManager();
            _statisticsManager = new StatisticsManager();
            _planInformationManager = new PlanInformationManager();
            _notManager = new NotificationManager();
            SystemConfigs = SystemConfig.GetConfigs;

        }

        public ActionResult Home()
        {
            ProfilesSearchRequest model = new ProfilesSearchRequest { Block = 99, Plan = 0, searchString= "", SocialType = 0 };
            ViewBag.CurrentUser = this.CDT;
            var Cust = _customerManager.GetCustomerByCustomerId(this.CDT.CustomerId);
            ViewBag.Customer = Cust;
            if (Cust.IsBroker.HasValue && Cust.IsBroker.Value)
            {
                ViewBag.PaymentHistory = _customerManager.GetCustomerBrokerPaymentHistory(Cust.CustomerId);

                Cust.BrokerLogo = "/AgencyLogos/" + Cust.BrokerLogo;
            }
            ViewBag.SocailProfiles = this._customerManager.GetSocialProfilesByCustomerid(this.CDT.CustomerId,model);

            var _stripeApiKey = SystemConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;
            var _stripePublishKey = SystemConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue2;

            ViewBag.stripeApiKey = _stripeApiKey;
            ViewBag.stripePublishKey = _stripePublishKey;

            return View(model);
        }
        [HttpPost]
        public ActionResult Home(ProfilesSearchRequest model)
        {
            if (model.searchString == null)
                model.searchString = "";

           
            ViewBag.CurrentUser = this.CDT;
            var Cust = _customerManager.GetCustomerByCustomerId(this.CDT.CustomerId);
            ViewBag.Customer = Cust;
            if (Cust.IsBroker.HasValue && Cust.IsBroker.Value)
            {
                ViewBag.PaymentHistory = _customerManager.GetCustomerBrokerPaymentHistory(Cust.CustomerId);
            }
            ViewBag.SocailProfiles = this._customerManager.GetSocialProfilesByCustomerid(this.CDT.CustomerId,model);
            var _stripeApiKey = SystemConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;
            var _stripePublishKey = SystemConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue2;

            ViewBag.stripeApiKey = _stripeApiKey;
            ViewBag.stripePublishKey = _stripePublishKey;

            return View(model);
        }


        [HttpPost]
        public ActionResult CreateNewProfile(CustomerAddProfileRequest model)
        {
            var jr = new JsonResult();
            try
            {
                
                var result = _customerManager.AddInstagramSocialProfile(model.IntagramUserName, model.customerId);

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

        [HttpPost]
        public ActionResult UpdateBrokerProfile(CustomerBrokerProfileRequest model)
        {
            var jr = new JsonResult();
            try
            {

                var result = _customerManager.UpdateCustomerBrokerProfile(model);

                if (result == true)
                {
                    jr.Data = new { ResultType = "Success", message = "Updated successfully" };
                }
                else
                {
                    jr.Data = new { ResultType = "Error", message = "error updating" };
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

                PhoneNumber = "+" + user.PhoneCode + user.PhoneNumber,
                UserName = user.UserName,
                Countries = CommonManager.GetCountries(),
                AddressLine1 = user.AddressLine1,
                AddressLine2 = user.AddressLine2,
                City = user.City,
                State = user.State,
                Country = user.Country,
                PostCode = user.PostCode,
                Notes = user.Notes

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
        public ActionResult ConfirmCancelAfilliateSubscription(int customerId)
        {
            var jr = new JsonResult();
            try
            {

                KlaviyoAPI klaviyoAPI = new KlaviyoAPI();
                KlaviyoProfile klaviyoProfile = new KlaviyoProfile();
                KlaviyoEvent ev = new KlaviyoEvent();

                SG2.CORE.MODAL.Customer customer = _customerManager.GetCustomerByCustomerId(customerId);


                //if (!profileDTO.IsJVServerRunning)
                //{
                //    severMode = "Manual";
                //}
                var _stripeApiKey = SystemConfig.GetConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;
                if (customer != null)
                {

                    if (customer.IsBroker.HasValue && customer.IsBroker.Value)
                    {
                        if (!string.IsNullOrEmpty(customer.StripeSubscriptionId))
                        {
                            StripeConfiguration.SetApiKey(_stripeApiKey);
                            var service = new SubscriptionService();
                            try
                            {
                                var sub = service.Get(customer.StripeSubscriptionId);

                                var subscription = service.Cancel(sub.Id, null);

                            }
                            catch (Exception)
                            {

                            }

                            //cancelling all social profile stripe subscriptions
                            var profiles = _customerManager.GetSocialProfilesByCustomerid(customer.CustomerId, new ProfilesSearchRequest { Block = 99, Plan = 0, searchString = "", SocialType = 0 });
                            foreach (var profile in profiles)
                            {
                                var dbprofile = _customerManager.GetSocialProfileById(profile.SocialProfileId);
                                if (dbprofile.SocialProfile.StripeSubscriptionId != null)
                                {
                                    try
                                    {
                                        var subs = service.Get(dbprofile.SocialProfile.StripeSubscriptionId);
                                        service.Cancel(subs.Id, null);
                                    }
                                    catch (Exception)
                                    {


                                    }
                                }
                            }

                            //if (subscription != null)
                            //{
                            //    //_cm.UpdateJVStatus(SocialProfileId, (int)GlobalEnums.JVStatus.ProfileRequiresCancelling);
                            //    _customerManager.UpdateSubscriptionStatus(Convert.ToInt32(profileDTO.SocialProfile.SocialProfileId), (int)GlobalEnums.PlanSubscription.canceled);
                            //}



                            Task.Run(() =>
                            {

                                var nt = new NotificationDTO()
                                {
                                    Notification = NotificationMessages[(int)NotificationMessagesIndexes.Unsubscribe],
                                    CreatedBy = customer.CustomerId.ToString(),
                                    CreatedOn = DateTime.Now,
                                    Updatedby = customer.CustomerId.ToString(),
                                    UpdateOn = DateTime.Now,
                                    SocialProfileId = customer.CustomerId,
                                    StatusId = (int)GeneralStatus.Unread,
                                    Mode = "Auto"
                                };
                                _notManager.AddNotification(nt);


                                List<NotRequiredProperty> list = new List<NotRequiredProperty>()
                        {
                            new NotRequiredProperty("$email", this.CDT.EmailAddress),
                            new NotRequiredProperty("$first_name ", this.CDT.FirstName),
                            new NotRequiredProperty("$last_name ", this.CDT.SurName),
                            new NotRequiredProperty("PlanName", "A LICENSE"),
                        };
                                ev.Event = "Afilliate Cancelled";
                                ev.Properties.NotRequiredProperties = list;
                                ev.CustomerProperties.Email = CDT.EmailAddress;
                                ev.CustomerProperties.FirstName = CDT.FirstName;
                                ev.CustomerProperties.LastName = CDT.EmailAddress;

                                var _klaviyoPublishKey = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klaviyo").ToLower()).ConfigValue;
                                var Klavio_FreeCustomers = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klavio_FreeCustomers").ToLower()).ConfigValue;
                                var Klavio_PayingCustomers = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klavio_PayingCustomers").ToLower()).ConfigValue;

                                klaviyoAPI.EventAPI(ev, _klaviyoPublishKey);
                                klaviyoAPI.Klaviyo_DeleteFromList(this.CDT.EmailAddress, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, Klavio_PayingCustomers);
                                var add = klaviyoAPI.Klaviyo_AddtoList(klaviyoProfile, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, Klavio_FreeCustomers);
                            });

                            jr.Data = new { ResultType = "Success", message = "User has successfully Unsubscribe." };
                        }
                        else
                        {
                            jr.Data = new { ResultType = "Error", message = "No active subscription available." };

                        }



                    }
                    else
                    {
                        jr.Data = new { ResultType = "Error", message = "No active subscription available." };

                    }
                }

                           
                if (_customerManager.UpdateCustomerStripeCustomer(customer.CustomerId,null, null,0))
                {

                    //--TODO: Update Klaviyo Web API Key

                    // klaviyoAPI.Klaviyo_DeleteFromList(this.CDT.EmailAddress, "H6fnAh");

                    //List<NotRequiredProperty> list = new List<NotRequiredProperty>()  {
                    //    new NotRequiredProperty("$email", this.CDT.EmailAddress),
                    //    new NotRequiredProperty("$first_name ", this.CDT.FirstName),
                    //    new NotRequiredProperty("$last_name ", this.CDT.SurName),
                    //};
                    //klaviyoProfile.email = this.CDT.EmailAddress;
                    //klaviyoAPI.PeopleAPI(list);
                    //var add = klaviyoAPI.Klaviyo_AddtoList(klaviyoProfile, "u45Z4H");



                    // }
                    //else
                    {
                        jr.Data = new { ResultType = "Success", message = "Afilliate subscription has been successfully Cancelled." };
                    }
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return Json(jr, JsonRequestBehavior.AllowGet);
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
                        PhoneCode = model.PhoneCode,
                        AddressLine1 = model.AddressLine1,
                        AddressLine2 = model.AddressLine2,
                        City = model.City,
                        State = model.State,
                        Country = model.Country,
                        PostCode = model.PostCode,
                        Notes = model.Notes
                    });
                    this.CDT.UserName = model.UserName;
                    this.CDT.FirstName = model.FirstName;
                    this.CDT.SurName = model.SurName;
                    this.CDT.PhoneCode = model.PhoneCode;
                    this.CDT.PhoneNumber = model.PhoneNumber;

                    this.CDT.AddressLine2 = model.AddressLine2;
                    this.CDT.AddressLine1 = model.AddressLine1;
                    this.CDT.City = model.City;
                    this.CDT.State = model.State;
                    this.CDT.Country = model.Country;
                    this.CDT.PostCode = model.PostCode;
                    this.CDT.Notes = model.Notes;



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

        [HttpPost]
        public ActionResult CreateStripeCustomerSubscription(NewSubscriptionRequestModel model)
        {

            KlaviyoAPI klaviyoAPI = new KlaviyoAPI();
            KlaviyoProfile klaviyoProfile = new KlaviyoProfile();
            var jr = new JsonResult();
            try
            {
                int socialProfileId = model.socialProfileId;//TODO: Social Profile Id

                var _stripeApiKey = SystemConfig.GetConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;
                StripeConfiguration.SetApiKey(_stripeApiKey);

                var customer = _customerManager.GetCustomerByCustomerId(model.customerid);
                var newPlan = _planInformationManager.GetPlanInformationById(model.selectedPlanId);


                var subscriptionService = new SubscriptionService();
                Subscription stripeSubscription = null;

                if (customer.StripeCustomerId != null)
                {
                    if (customer.StripeSubscriptionId != null)
                    {
                        //if (model.paymentmethod != null)
                        //{
                        //    var options = new CustomerUpdateOptions
                        //    {
                        //        PaymentMethod = model.paymentmethod,
                        //    };
                        //    var service = new CustomerService();
                        //    Customer customer = service.Update(this.CDT.StripeCustomerId, options);
                        //}
                        Subscription subscriptionItemUpdate = subscriptionService.Get(customer.StripeSubscriptionId);

                        var items = new List<SubscriptionItemOptions> {
                                        new SubscriptionItemOptions {
                                        Id= subscriptionItemUpdate.Items.Data[0].Id,
                                        Plan = newPlan.StripePlanId,
                                        Quantity= 1,
                                        },
                                    };

                        var subscriptionUpdateoptions = new SubscriptionUpdateOptions
                        {
                            Items = items,

                            Prorate = true,
                            ProrationDate = DateTime.Now,
                            //ProrationDate = DateTime.Now,

                        };
                        subscriptionUpdateoptions.AddExpand("latest_invoice.payment_intent");
                        stripeSubscription = subscriptionService.Update(customer.StripeSubscriptionId, subscriptionUpdateoptions);
                    }
                    else
                    {
                        var stripeItems = new List<SubscriptionItemOptions> {
                                      new SubscriptionItemOptions {
                                        Plan = newPlan.StripePlanId,
                                        Quantity= 1
                                      }
                                    };
                        var stripeSubscriptionCreateoptions = new SubscriptionCreateOptions
                        {
                            Customer = customer.StripeCustomerId,
                            Items = stripeItems,

                        };
                        stripeSubscriptionCreateoptions.AddExpand("latest_invoice.payment_intent");
                        stripeSubscription = subscriptionService.Create(stripeSubscriptionCreateoptions);
                    }

                    _customerManager.UpdateCustomerStripeCustomer(model.socialProfileId, customer.StripeCustomerId, stripeSubscription.Id, newPlan.PlanId);

                }
                ////////////////// new scenario
                else
                {
                    var stripeCustomerCreateOptions = new CustomerCreateOptions
                    {
                        Description = " Afilliate Customer for Social Growth Labs " + this.CDT.EmailAddress + " with Customer id " + model.socialProfileId,
                        PaymentMethod = model.paymentmethod,
                        Name = this.CDT.FirstName + " " + this.CDT.SurName,
                        Email = this.CDT.EmailAddress,
                        Phone = this.CDT.PhoneCode + this.CDT.PhoneNumber,
                        InvoiceSettings = new CustomerInvoiceSettingsOptions
                        {
                            DefaultPaymentMethod = model.paymentmethod,
                        },
                    };
                    var stripeCustomerService = new CustomerService();
                    Customer stripeCustomer = stripeCustomerService.Create(stripeCustomerCreateOptions);




                    var stripeItems = new List<SubscriptionItemOptions> {
                      new SubscriptionItemOptions {
                        Plan = newPlan.StripePlanId,
                        Quantity= 1
                      }
                    };
                    var stripeSubscriptionCreateOptions = new SubscriptionCreateOptions
                    {
                        Customer = stripeCustomer.Id,
                        Items = stripeItems,
                        //BillingCycleAnchor = DateTimeOffset.FromUnixTimeSeconds(1576486590).UtcDateTime
                        //  BillingCycleAnchor = DateTime.Now,
                        //BillingThresholds = {  }
                    };
                    stripeSubscriptionCreateOptions.AddExpand("latest_invoice.payment_intent");
                    stripeSubscription = subscriptionService.Create(stripeSubscriptionCreateOptions);

                    //-- Update customer stripe id async call not to wait.
                    _customerManager.UpdateCustomerStripeCustomer(model.customerid, stripeCustomer.Id, stripeSubscription.Id, newPlan.PlanId);

                }

                //--TODO: Check subscription status here

                if (stripeSubscription != null)
                {



                    PlanService service = new PlanService();
                    //-- Subscription Description
                    if (stripeSubscription.Plan == null)
                    {
                        var selectedPlan = service.Get(this.CDT.StripePlanId);
                        stripeSubscription.Plan = selectedPlan;
                    }

                    SocialProfile_PaymentsDTO paymentRec = new SocialProfile_PaymentsDTO();
                    paymentRec.SocialProfileId = model.customerid;
                    paymentRec.StripeSubscriptionId = stripeSubscription.Id;
                    paymentRec.Description = stripeSubscription.Plan.Nickname;
                    paymentRec.Name = stripeSubscription.Plan.Nickname;
                    paymentRec.Price = stripeSubscription.Plan.Amount / 100;
                    //-- subDTO.Price = stripeSubscription.Plan.Amount;
                    paymentRec.StripePlanId = newPlan.StripePlanId;
                    paymentRec.SubscriptionType = stripeSubscription.Plan.Interval;

                    paymentRec.StartDate = stripeSubscription.CurrentPeriodStart.Value;
                    paymentRec.EndDate = stripeSubscription.CurrentPeriodEnd.Value;
                    paymentRec.StatusId = (int)GlobalEnums.PlanSubscription.Active;
                    paymentRec.PaymentPlanId = newPlan.PlanId;
                    paymentRec.StripeInvoiceId = stripeSubscription.LatestInvoiceId;
                    paymentRec.PaymentDateTime = DateTime.Now;

                    _customerManager.InsertCustomerPayment(paymentRec);


                    var nt = new NotificationDTO()
                    {
                        Notification = string.Format(NotificationMessages[(int)NotificationMessagesIndexes.PlanSubscribe], stripeSubscription.Plan.Nickname),
                        CreatedBy = model.socialProfileId.ToString(),
                        CreatedOn = System.DateTime.Now,
                        Updatedby = model.socialProfileId.ToString(),
                        UpdateOn = DateTime.Now,
                        SocialProfileId = model.socialProfileId,
                        StatusId = (int)GeneralStatus.Unread,
                       
                    };
                    _notManager.AddNotification(nt);


                    //--TODO: Update Klaviyo Web API Key
                    var _klaviyoPublishKey = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klaviyo").ToLower()).ConfigValue;
                    var Klavio_FreeCustomers = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klavio_FreeCustomers").ToLower()).ConfigValue;
                    var Klavio_PayingCustomers = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klavio_PayingCustomers").ToLower()).ConfigValue;


                    //klaviyoAPI.Klaviyo_DeleteFromList(this.CDT.EmailAddress, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, _klavio_NonPayingSubscribeList);

                    List<NotRequiredProperty> list = new List<NotRequiredProperty>()  {
                        new NotRequiredProperty("$email", this.CDT.EmailAddress),
                        new NotRequiredProperty("$first_name ", this.CDT.FirstName),
                        new NotRequiredProperty("$last_name ", this.CDT.SurName),
                        //new NotRequiredProperty("URL", URL),
                        new NotRequiredProperty("InvoiceDate",paymentRec.StartDate.ToString("dd MMMM yyyy") ),
                        new NotRequiredProperty("PlanName", paymentRec.Name),
                        new NotRequiredProperty("Price",  "$" + paymentRec.Price/100),
                        new NotRequiredProperty("Card", ""),
                        new NotRequiredProperty("Address","")
                    };
                    klaviyoProfile.email = this.CDT.EmailAddress;



                    klaviyoAPI.PeopleAPI(list, _klaviyoPublishKey);
                    klaviyoAPI.Klaviyo_DeleteFromList(this.CDT.EmailAddress, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, Klavio_FreeCustomers);
                    var add = klaviyoAPI.Klaviyo_AddtoList(klaviyoProfile, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, Klavio_PayingCustomers);

                    KlaviyoEvent ev = new KlaviyoEvent();
                    ev.Event = "Afilliate Subscribed";
                    ev.Properties.NotRequiredProperties = list;
                    ev.CustomerProperties.Email = this.CDT.EmailAddress;
                    ev.CustomerProperties.FirstName = this.CDT.FirstName;
                    ev.CustomerProperties.LastName = this.CDT.SurName;

                    klaviyoAPI.EventAPI(ev, _klaviyoPublishKey);


                    return this.Content(stripeSubscription.ToJson(), "application/json");
                    //jr.Data = new { ResultType = "Success", Message = "success" };

                }
                else
                {
                    return this.Content("subscription error, object not found.");
                }


            }
            catch (Exception ex)
            {
                return this.Content(ex.ToString());
            }
        }

        //public ActionResult Plan(int SocialProfileId = 0)
        //{
        //    try
        //    {
        //        var curCust = (CustomerDTO)_sessionManager.Get(SessionConstants.Customer);

        //        //SocialProfile_PaymentsDTO subscriptionDTO = _customerManager.GetSubscription(SocialProfileId);
        //        var profile = _customerManager.GetSocialProfileById(SocialProfileId);
        //        var history = _customerManager.GetCustomerOrderHistory("50", 1, this.CDT.CustomerId, SocialProfileId);

        //        StripeConfiguration.SetApiKey(_stripeApiKey);
        //        var planService = new PlanService();
        //        var planOptions = new PlanListOptions
        //        {
        //            Limit = 4,
        //        };
        //        var plans = _planInformationManager.GetallIntagramPaymentPlans(false);

        //        List<CustomerPaymentPlansViewModel> payPlan = null;
        //        if (plans != null)
        //        {
        //            payPlan = new List<CustomerPaymentPlansViewModel>();
        //            foreach (var plan in plans)
        //            {
        //                CustomerPaymentPlansViewModel paymentPlansViewModel = new CustomerPaymentPlansViewModel();

        //                if (profile.SocialProfile.PaymentPlanId == plan.PlanId)
        //                {
        //                    paymentPlansViewModel.currentPlan = true;
        //                }
        //                else
        //                {
        //                    paymentPlansViewModel.currentPlan = false;
        //                }

        //                paymentPlansViewModel.PlanId = plan.PlanId.ToString();
        //                paymentPlansViewModel.PlanName = plan.PlanName;
        //                paymentPlansViewModel.PlanStatus = true;
        //                paymentPlansViewModel.Amount = Convert.ToInt64( plan.PlanPrice);
        //                //paymentPlansViewModel.BillingScheme = plan.b;
        //                paymentPlansViewModel.Currency = "USD";
        //                paymentPlansViewModel.Interval = "per month";
        //                //paymentPlansViewModel.IntervalCount = plan.IntervalCount;
        //                //paymentPlansViewModel.ProductCode = plan.ProductId;
        //                payPlan.Add(paymentPlansViewModel);
        //            }
        //        }

        //        var cardService = new CardService();
        //        var cardOptions = new CardListOptions
        //        {
        //            Limit = 3,
        //        };
        //        List<CustomerPaymentCardsViewModel> payCards = null;
        //        if (this.CDT.StripeCustomerId != null)
        //        {
        //            var striptCards = cardService.List(this.CDT.StripeCustomerId, cardOptions);
        //            if (striptCards != null)
        //            {
        //                payCards = new List<CustomerPaymentCardsViewModel>();
        //                foreach (var item in striptCards)
        //                {
        //                    var card = new CustomerPaymentCardsViewModel();
        //                    card.Last4 = item.Last4;
        //                    card.ExpMonth = item.ExpMonth;
        //                    card.ExpYear = item.ExpYear;
        //                    card.Brand = item.Brand;
        //                    card.Funding = item.Funding;
        //                    payCards.Add(card);
        //                }
        //            }
        //        }

        //        CustomerPlanDetailViewModel cpdViewModel = new CustomerPlanDetailViewModel();
        //        cpdViewModel.PaymentPlans = payPlan;
        //        cpdViewModel.PaymentCards = payCards;
        //        cpdViewModel.OrderHistoryViewModels = history;
        //        return View(cpdViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //[HttpPost]
        //public ActionResult ConfirmAndPay(CustomerPaymentPlansViewModel model)
        //{
        //    try
        //    {
        //        StripeConfiguration.SetApiKey(_stripeApiKey);
        //        var planService = new PlanService();
        //        var selPlan = planService.Get(model.PlanId);

        //        CustomerPaymentPlansViewModel payPlan = new CustomerPaymentPlansViewModel();
        //        if (selPlan != null)
        //        {
        //            payPlan.PlanId = selPlan.Id;
        //            payPlan.PlanName = selPlan.Nickname;
        //            payPlan.PlanStatus = selPlan.Active;
        //            payPlan.Amount = selPlan.Amount.Value;
        //            payPlan.BillingScheme = selPlan.BillingScheme;
        //            payPlan.Currency = selPlan.Currency;
        //            payPlan.Interval = selPlan.Interval;
        //            payPlan.IntervalCount = selPlan.IntervalCount;
        //            payPlan.ProductCode = selPlan.ProductId;
        //        }

        //        //var paymentMethodService = new PaymentMethodService();
        //        //var paymentMethodListOptions = new PaymentMethodListOptions
        //        //{
        //        //    CustomerId = this.CDT.StripeCustomerId,
        //        //    Limit = 3,
        //        //    Type = "card",
        //        //};
        //        //var paymentmethods = paymentMethodService.List(paymentMethodListOptions);


        //        var service = new CardService();
        //        List<CustomerPaymentCardsViewModel> payCards = new List<CustomerPaymentCardsViewModel>();
        //        if (this.CDT.StripeCustomerId != null)
        //        {
        //            var options = new CardListOptions
        //            {
        //                Limit = 3,
        //            };
        //            var striptCards = service.List(this.CDT.StripeCustomerId, options);


        //            foreach (var item in striptCards)
        //            {
        //                var card = new CustomerPaymentCardsViewModel();
        //                card.Last4 = item.Last4;
        //                card.ExpMonth = item.ExpMonth;
        //                card.ExpYear = item.ExpYear;
        //                card.Brand = item.Brand;
        //                card.Funding = item.Funding;
        //                payCards.Add(card);
        //            }
        //        }
        //        else
        //        {

        //            var card = new CustomerPaymentCardsViewModel();
        //            card.Last4 = "";
        //            card.ExpMonth = 0;
        //            card.ExpYear = 0;
        //            card.Brand = "Visa";
        //            card.Funding = "Card";
        //            payCards.Add(card);

        //        }
        //        var cusPayAadConfirmVM = new CustomerPayAndConfirmViewModel();
        //        cusPayAadConfirmVM.CardDetails = payCards;
        //        cusPayAadConfirmVM.PaymentPlan = payPlan;


        //        return View(cusPayAadConfirmVM);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    // customerSubscriptionViewModal.CustomerPaymentDetailsVM = payPlan;
        //    // return View(payPlan);            
        //    //_customerManager.UpdateStripeCustomerId(curCust.EmailAddress, "cus_EwmdhsPhDc2zOV");            
        //    //var subscriptionService = new SubscriptionService();
        //    //var subscription = subscriptionService.Get("sub_EwmdchSHry5R0d");
        //    //SubscriptionDTO subscriptionDTO = new SubscriptionDTO();            
        //    //subscriptionDTO.CustomerId = curCust.CustomerId;
        //    //subscriptionDTO.StripeSubscriptionId = subscription.Id;
        //    //subscriptionDTO.Description = subscription.Items.FirstOrDefault().Plan.Nickname;
        //    //subscriptionDTO.InstaUsrName = subscription.Plan.Nickname;
        //    //subscriptionDTO.Price = subscription.Plan.Amount;
        //    //subscriptionDTO.StripePlanId = subscription.Plan.Id;
        //    //subscriptionDTO.SubscriptionType = subscription.Plan.Interval;
        //    //subscriptionDTO.StartDate = subscription.Start;
        //    //subscriptionDTO.EndDate = subscription.EndedAt;

        //    //_customerManager.InsertSubscription(subscriptionDTO);
        //    //string StripeCustomerId =  _customerManager.GetStripeCustomerId(curCust.EmailAddress);
        //    //if (StripeCustomerId == null)
        //    //{            
        //    //    var paymentMethodService = new PaymentMethodService();
        //    //    var paymentMethodListOptions = new PaymentMethodListOptions
        //    //    {
        //    //        CustomerId = StripeCustomerId,
        //    //        Limit = 3,
        //    //    };
        //    //    var paymentMethodList = paymentMethodService.List(paymentMethodListOptions);

        //    //    if (paymentMethodList == null)
        //    //    {

        //    //        var paymentMethodCreateOptions = new PaymentMethodCreateOptions
        //    //        {
        //    //            Type = "card",
        //    //            Card = new PaymentMethodCardCreateOptions
        //    //            {
        //    //                Number = "4242424242424242",
        //    //                ExpMonth = 4,
        //    //                ExpYear = 2020,
        //    //                Cvc = "123",
        //    //            },
        //    //        };

        //    //        var newPaymentMethod = paymentMethodService.Create(paymentMethodCreateOptions);

        //    //        var customerCreateOptions = new CustomerCreateOptions
        //    //        {
        //    //            Description = "Customer for SG2@example.com",
        //    //            SourceToken = "tok_visa",
        //    //            Address = new AddressOptions
        //    //            {
        //    //                City = "Lahore",
        //    //                Country = "Pakistan",
        //    //                PostalCode = "54000",
        //    //                Line1 = "Abc street",
        //    //                Line2 = "PIA road",
        //    //                State = "Punjab"
        //    //            },

        //    //            InstaUsrName = "Raza",
        //    //            Email = "ssaqibshirazi@gmail.com",
        //    //            PaymentMethodId = newPaymentMethod.Id

        //    //        };

        //    //        var customerService = new CustomerService();
        //    //        Customer customer = customerService.Create(customerCreateOptions);

        //    //        _customerManager.UpdateStripeCustomerId(curCust.EmailAddress, customer.Id);

        //    //    }
        //    //    else
        //    //    {


        //    //    }

        //    //}



        //}

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


        public ActionResult AgencyInstagramTarget(int? success)
        {

            var SocailProfile = this._customerManager.GetSocialProfileTargetByBrokerCustomerId(this.CDT.CustomerId);
           
            if (success.HasValue && success.Value == 1)
            {
                ViewBag.success = 1;
            }


            return View(SocailProfile);

        }

        [HttpPost]
        public ActionResult AgencyInstagramTarget(SocialProfileDTO request)
        {

            this._customerManager.UpdateTargetProfile(request);
            return RedirectToAction("AgencyInstagramTarget", "user", new { success = 1 });
        }

    }
}