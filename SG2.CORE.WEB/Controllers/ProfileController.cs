using SG2.CORE.BAL.Managers;
using SG2.CORE.MODAL.DTO.TargetPreferences;
using SG2.CORE.MODAL.ViewModals.TargetPreferences;
using SG2.CORE.MODAL.DTO.Customers;
using System;
using System.Linq;
using System.Web.Mvc;
using static SG2.CORE.COMMON.GlobalEnums;
using SG2.CORE.MODAL.DTO.PlanInformation;
using Stripe;
using SG2.CORE.MODAL.ViewModals.Customers;
using System.Collections.Generic;
using SG2.Core.Web;
using SG2.CORE.COMMON;
using SG2.CORE.WEB.Architecture;
using SG2.CORE.MODAL.DTO.SystemSettings;
using klaviyo.net;
using SG2.CORE.MODAL.klaviyo;
using SG2.CORE.MODAL.ViewModals.Statistics;
using SG2.CORE.MODAL.DTO.QueueLogger;
using Newtonsoft.Json;
using SG2.CORE.MODAL.DTO.Notification;
using System.Threading.Tasks;
//using SG2.CORE.MODAL;
using System.Reflection;
using System.Web.Configuration;

namespace SG2.CORE.WEB.Controllers
{
    [AuthorizeCustomer]
    public class ProfileController : BaseController
    {
       // protected readonly TargetPreferencesManager _targetPreferenceManager;
        protected readonly CustomerManager _cm;
        protected readonly CommonManager _commonManager;
        protected readonly PlanInformationManager _planManager;
        protected readonly StatisticsManager _statisticsManager;
        protected readonly CustomerManager _customerManager;
        protected readonly List<SystemSettingsDTO> SystemConfigs;
        protected readonly NotificationManager _notManager;
        private readonly string _stripeApiKey = string.Empty;
        //protected readonly QueueLoggerManager _queueLoggerManager;
        //protected readonly ProxyManager _proxyManager;
        //private readonly JVBoxManager _jVBoxManager;
        private string severMode = "Auto";
        //QueuePublisher<InstagramActionMessage> queue = new QueuePublisher<InstagramActionMessage>();

        public ProfileController()
        {
             
        //_targetPreferenceManager = new TargetPreferencesManager();
        _cm = new CustomerManager();
            _planManager = new PlanInformationManager();
            _statisticsManager = new StatisticsManager();
            _commonManager = new CommonManager();
            _customerManager = new CustomerManager();
            SystemConfigs = SystemConfig.GetConfigs;
            //_queueLoggerManager = new QueueLoggerManager();
            _notManager = new NotificationManager();
            //_proxyManager = new ProxyManager();
            //_jVBoxManager = new JVBoxManager();

            //Setting Stripe api key
            //if (WebConfigurationManager.AppSettings["IsDebug"] == "1")
            //{
            //    _stripeApiKey = WebConfigurationManager.AppSettings["StripeTestApiKey"];
            //}
            //else
            //{
            //    _stripeApiKey = WebConfigurationManager.AppSettings["StripeLiveApiKey"];
            //}
            
        }

        public ActionResult Basic(int socialProfileId, int? success)
        {
            ViewBag.CurrentUser = this.CDT;
            ViewBag.socialProfileId = socialProfileId;
            var SocailProfile = this._cm.GetSocialProfileById(socialProfileId);
            if (success.HasValue && success.Value == 1)
            {
                ViewBag.success = 1;
            }

            return View(SocailProfile);

        }



        [HttpPost]
        public ActionResult Basic(SocialProfileDTO request)
        {
            this._cm.UpdateBasicSocialProfile(request);
            return RedirectToAction("Basic", "Profile", new { socialProfileId = request.SocialProfile.SocialProfileId, success=1 });
        }

        public ActionResult Lists(int socialProfileId, int? success)
        {
            ViewBag.socialProfileId = socialProfileId;
            ViewBag.CurrentUser = this.CDT;
            var SocailProfile = this._cm.GetSocialProfileById(socialProfileId);

            if (success.HasValue && success.Value == 1)
            {
                ViewBag.success = 1;
            }

            return View(SocailProfile);

        }


        [HttpPost]
        public ActionResult Lists(SocialProfileDTO request)
        {
            this._cm.UpdateTargetProfileLists(request);
            return RedirectToAction("Lists", "Profile", new { socialProfileId = request.SocialProfile_Instagram_TargetingInformation.SocialProfileId, success = 1 });
        }

        public ActionResult Target(int socialProfileId, int? success)
        {
            ViewBag.socialProfileId = socialProfileId;
            ViewBag.CurrentUser = this.CDT;
            var SocailProfile = this._cm.GetSocialProfileById(socialProfileId);
            var customer =_customerManager.GetCustomerByCustomerId(this.CDT.CustomerId);

            
            ViewBag.Plans = _planManager.GetallIntagramPaymentPlans(customer.IsBroker.HasValue? customer.IsBroker.Value:false);

            if (success.HasValue && success.Value == 1)
            {
                ViewBag.success = 1;
            }

            var _stripeApiKey = SystemConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;
            var _stripePublishKey = SystemConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue2;

            ViewBag.stripeApiKey = _stripeApiKey;
            ViewBag.stripePublishKey = _stripePublishKey;
            StripeConfiguration.SetApiKey(_stripeApiKey);
            var planService = new PlanService();
            var cardService = new CardService();
            var cardOptions = new CardListOptions
            {
                Limit = 3,
            };
            List<CustomerPaymentCardsViewModel> payCards = null;
            //if (SocailProfile.SocialProfile.StripeCustomerId != null)
            //{
            //    var striptCards = cardService.List(SocailProfile.SocialProfile.StripeCustomerId, cardOptions);
                
            //    if (striptCards != null)
            //    {
            //        payCards = new List<CustomerPaymentCardsViewModel>();
            //        foreach (var item in striptCards)
            //        {
            //            var card = new CustomerPaymentCardsViewModel();
            //            card.Last4 = item.Last4;
            //            card.ExpMonth = item.ExpMonth;
            //            card.ExpYear = item.ExpYear;
            //            card.Brand = item.Brand;
            //            card.Funding = item.Funding;
            //            payCards.Add(card);
            //        }
            //    }
            //}

            //ViewBag.paycards = payCards;
           

            return View(SocailProfile);

            ////if (!string.IsNullOrEmpty((string)TempData["Success"]))
            ////{
            ////    ViewBag.Success = (string)TempData["Success"];
            ////    ViewBag.Message = TempData["Message"];
            ////}
            //int SocialProfileId = socialProfileId ?? 0;
            //ViewBag.CurrentUser = this.CDT;
            //ViewBag.SocailProfiles = this._cm.GetSocialProfilesByCustomerid(this.CDT.CustomerId);
            //var history = _customerManager.GetCustomerOrderHistory("50", 1, this.CDT.CustomerId, SocialProfileId);
           
            //StripeConfiguration.SetApiKey(_stripeApiKey);

            //SocialProfileDTO profileDTO = new SocialProfileDTO();
            //if (socialProfileId != null && socialProfileId > 0)
            //{
            //    profileDTO = _cm.GetSocialProfileById(socialProfileId ?? 0);
            //}

            //TargetPreferencesViewModel targetPreferences = new TargetPreferencesViewModel();
            //var plans = _planManager.GetAllSocialGrowthPlans().ToList();

            //PlanInformationDTO selectedPlan = new PlanInformationDTO();
            //if (profileDTO.SocialProfile.PaymentPlanId != null)
            //{
            //    selectedPlan = plans.FirstOrDefault(x => x.PlanId == profileDTO.SocialProfile.PaymentPlanId);
            //}

            //targetPreferences.ProfileName = profileDTO.SocialProfile.SocialUsername;
            ////targetPreferences.Preference1 = profileDTO.Preference1;
            ////targetPreferences.Preference2 = profileDTO.Preference2;
            ////targetPreferences.Preference3 = profileDTO.Preference3;
            ////targetPreferences.Preference4 = profileDTO.Preference4;
            ////targetPreferences.Preference5 = profileDTO.Preference5;
            ////targetPreferences.Preference6 = profileDTO.Preference6;
            ////targetPreferences.Preference7 = profileDTO.Preference7;
            ////targetPreferences.Preference8 = profileDTO.Preference8;
            ////targetPreferences.Preference9 = profileDTO.Preference9;
            ////targetPreferences.Preference10 = profileDTO.Preference10;
            ////targetPreferences.City = profileDTO.PrefferedCityId;
            ////targetPreferences.Country = profileDTO.PrefferedCountryId;
            //targetPreferences.InstaPassword = profileDTO.SocialProfile.SocialPassword;
            //targetPreferences.InstaUser = profileDTO.SocialProfile.SocialUsername;
            //targetPreferences.TargetInformationId = profileDTO.SocialProfile_Instagram_TargetingInformation.TargetingInformationId;
            //targetPreferences.SocialProfileId = socialProfileId;
            //targetPreferences.Plans = plans;
            //targetPreferences.ActivePlanName = profileDTO.CurrentPaymentPlan.PlanName;
            //targetPreferences.ActivePlanId = selectedPlan.PlanId == 0 ? null : (int?)selectedPlan.PlanId;
            //targetPreferences.ActivePlanPrice = selectedPlan.DisplayPrice;
            //targetPreferences.ActivePlanLikes = selectedPlan.Likes;
            //targetPreferences.DefaultPaymentPlan = plans.FirstOrDefault(x => x.IsParentPlan == true &&  x.SocialPlatform == 30 && x.IsDefault.Value == true);
            ////targetPreferences.JVStatus = profileDTO.JVBoxStatusName;
            ////targetPreferences.JVStatusId = profileDTO.JVStatusId;
            //targetPreferences.StripeApiKey = _stripeApiKey;
            //targetPreferences.StripePublishKey = _stripePublishKey;
            //targetPreferences.SPStatusId = profileDTO.SocialProfile.StatusId;
            //targetPreferences.OrderHistoryViewModels = history;
            ////targetPreferences.IsOptedMarketingEmail = profileDTO.IsOptedMarketingEmail;
            ////targetPreferences.SocialAccAS = profileDTO.SocialAccAS;
            ////if (targetPreferences.Country != null)
            ////{
            ////    targetPreferences.Cities = CommonManager.GetCities().Where(m => m.CountryId == Convert.ToInt16(profileDTO.PrefferedCountryId)).ToList();
            ////}
            ////else
            ////{
            ////    targetPreferences.Cities = CommonManager.GetCities();
            ////}

            //var cardService = new CardService();
            //var cardOptions = new CardListOptions
            //{
            //    Limit = 3,
            //};
            //List<CustomerPaymentCardsViewModel> payCards = null;
            //if (this.CDT.StripeCustomerId != null)
            //{
            //    var striptCards = cardService.List(this.CDT.StripeCustomerId, cardOptions);
            //    if (striptCards != null)
            //    {
            //        payCards = new List<CustomerPaymentCardsViewModel>();
            //        foreach (var item in striptCards)
            //        {
            //            var card = new CustomerPaymentCardsViewModel();
            //            card.Last4 = item.Last4;
            //            card.ExpMonth = item.ExpMonth;
            //            card.ExpYear = item.ExpYear;
            //            card.Brand = item.Brand;
            //            card.Funding = item.Funding;
            //            payCards.Add(card);
            //        }
            //    }
            //}

            //targetPreferences.PaymentCards = payCards;
            ////targetPreferences.Countries = CommonManager.GetCountries();
            //return View(targetPreferences);

        }


		[HttpPost]
		public ActionResult Target(SocialProfileDTO request)
		{
			
			this._cm.UpdateTargetProfile(request);
			return RedirectToAction("Target", "Profile", new { socialProfileId = request.SocialProfile_Instagram_TargetingInformation.SocialProfileId, success = 1 });
		}
        public ActionResult Stats(int socialProfileId)
        {
            ViewBag.socialProfileId = socialProfileId;
            ViewBag.CurrentUser = this.CDT;
            ViewBag.socialProfile = this._cm.GetSocialProfileById(socialProfileId).SocialProfile;

            ViewBag.actions = this._cm.ReturnLastActions(socialProfileId, 100);
            

            return View(this._statisticsManager.GetStatistics(socialProfileId));
           

        }
		[HttpPost]
		public ActionResult ContactDetailsPOST(FormCollection fomr)
		{

			string DeviceBinLocation = Convert.ToString(fomr["DeviceBinLocation"]);
			string SocialProfileName = Convert.ToString(fomr["SocialProfileName"]);
			int SocialProfileId = Convert.ToInt32(fomr["SocialProfileId"]);
			var model = this._cm.GetSocialProfileById(SocialProfileId);
			model.SocialProfile.DeviceBinLocation = DeviceBinLocation;
			model.SocialProfile.SocialProfileName = SocialProfileName;
			model.SocialProfile.SocialProfileId = SocialProfileId;
			//int SocialProfileId = socialProfileId ?? 0;
			var socialprofile = this._cm.UpdateBasicSocialProfile(model);
			//ViewBag.SocailProfile = socialprofile;
			return RedirectToAction("Basic", "Profile", new { @socialProfileId = SocialProfileId });
			//return RedirectToAction("Basic", "Profile");

		}
		public ActionResult MatchFilters(int? socialProfileId = null)
		{
			//int SocialProfileId = socialProfileId ?? 0;
			//var socialprofile = this._cm.GetSocialProfileById(SocialProfileId);
			//ViewBag.SocailProfile = socialprofile;
			//return RedirectToAction("Basic", "Profile", new { @socialProfileId = socialProfileId });
			return PartialView();

		}
		public ActionResult Trends(int socialProfileId, int mode)
        {
            var jr = new JsonResult();
            try
            {
                DateTime startdate = DateTime.Today.AddDays(-7);   //1 week
                DateTime enddate = DateTime.Today.AddHours(24);

                if (mode == 2)
                {
                    startdate = DateTime.Today.AddDays(-30);  //3 months
                }
                if ( mode == 3)
                {
                    startdate = DateTime.Today.AddMonths(-3);  //3 months
                }
                else if ( mode == 4)
                {
                    startdate = DateTime.Today.AddMonths(-6); //6 months
                }
                else if (mode == 5)
                {
                    startdate = DateTime.Today.AddMonths(-12); //6 months
                }


                var trends  = _statisticsManager.GetProfileTrends(socialProfileId, startdate, enddate);

                foreach (var item in trends)
                {
                    item.FollowersTotal  = item.FollowersTotal.HasValue ?  item.FollowersTotal: 0;
                    item.FollowingsTotal = item.FollowingsTotal.HasValue ? item.FollowingsTotal : 0;
                    item.LikeTotal = item.LikeTotal.HasValue ? item.LikeTotal : 0;
                    item.Engagement = item.Engagement.HasValue ? item.Engagement : 0;
                }


                if (trends != null)
                {
                    jr.Data = new
                    {
                        ResultType = "Success",
                        message = "",
                        ResultData = new
                        {
                            Date = trends.Select(x => x.Date.ToString("dd-MMM-yyyy")).ToArray(),
                            Followers = trends.Select(x => x.FollowersTotal.ToString()).ToArray(),
                            //FollowersGainData = statisticsViewModel.StatisticsListing.Select(x => x.FollowersGain.ToString()).ToArray(),
                            FollowingsData = trends.Select(x => x.FollowingsTotal.ToString()).ToArray(),
                            //FollowingsRatioData = statisticsViewModel.StatisticsListing.Select(x => x.FollowingsRatio.ToString()).ToArray(),
                            //AVGFollowersData = statisticsViewModel.StatisticsListing.Select(x => x.AVGFollowers.ToString()).ToArray(),


                            //LikeData = statisticsViewModel.StatisticsListing.Select(x => x.Like.ToString()).ToArray(),
                            //CommentData = statisticsViewModel.StatisticsListing.Select(x => x.Comment.ToString()).ToArray(),
                            //LikeCommentData = statisticsViewModel.StatisticsListing.Select(x => x.LikeComments.ToString()).ToArray(),
                            Engagement = trends.Select(x => x.Engagement.ToString()).ToArray(),

                            AvgLikes = trends.Select(x => x.LikeTotal.ToString()).ToArray()


                            //TotalComment = statisticsViewModel.TotalComment.ToString(),
                            //TotalEngagement = statisticsViewModel.TotalEngagement.ToString(),
                            //TotalFollowers = statisticsViewModel.TotalFollowers.ToString(),
                            //TotalFollowersGain = statisticsViewModel.TotalFollowersGain.ToString(),
                            //TotalFollowings = statisticsViewModel.TotalFollowings.ToString(),
                            //TotalFollowingsRatio = statisticsViewModel.TotalFollowingsRatio.ToString(),
                            //TotalLike = statisticsViewModel.TotalLike.ToString(),
                            //TotalLikeComment = statisticsViewModel.TotalLikeComment.ToString()
                        }
                    };
                }
                else
                {
                    jr.Data = new { ResultType = "Error", message = "" };
                }

            }
            catch (Exception exp)
            {
                throw exp;
            }

            return Json(jr, JsonRequestBehavior.AllowGet);
        }


        public ActionResult History(int socialProfileId)
        {
            ViewBag.socialProfileId = socialProfileId;
            ViewBag.CurrentUser = this.CDT;
            var SocailProfile = this._cm.GetSocialProfileById(socialProfileId);

            return View(SocailProfile);

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

                var socialProfile = _customerManager.GetSocialProfileById(model.socialProfileId);
                var newPlan = _planManager.GetPlanInformationById(model.selectedPlanId);


                var subscriptionService = new SubscriptionService();
                Subscription stripeSubscription = null;

                if (socialProfile.SocialProfile.StripeCustomerId != null)
                {
                    if (socialProfile.SocialProfile.StripeSubscriptionId != null)
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
                        Subscription subscriptionItemUpdate = subscriptionService.Get(socialProfile.SocialProfile.StripeSubscriptionId);

                        var items = new List<SubscriptionItemUpdateOption> {
                                        new SubscriptionItemUpdateOption {
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
                        stripeSubscription = subscriptionService.Update(socialProfile.SocialProfile.StripeSubscriptionId, subscriptionUpdateoptions);
                    }
                    else
                    {
                        var stripeItems = new List<SubscriptionItemOption> {
                                      new SubscriptionItemOption {
                                        Plan = newPlan.StripePlanId,
                                        Quantity= 1
                                      }
                                    };
                        var stripeSubscriptionCreateoptions = new SubscriptionCreateOptions
                        {
                            Customer = socialProfile.SocialProfile.StripeCustomerId,
                            Items = stripeItems,
                          
                        };
                        stripeSubscriptionCreateoptions.AddExpand("latest_invoice.payment_intent");
                        stripeSubscription = subscriptionService.Create(stripeSubscriptionCreateoptions);
                    }

                    _cm.UpdateSocialProfileStripeCustomer(model.socialProfileId, socialProfile.SocialProfile.StripeCustomerId, stripeSubscription.Id, newPlan.PlanId);

                }
                ////////////////// new scenario
                else
                {
                    var stripeCustomerCreateOptions = new CustomerCreateOptions
                    {
                        Description = " Customer for Social Growth Labs" + this.CDT.EmailAddress + " with profile id " + model.socialProfileId,
                        PaymentMethod = model.paymentmethod,
                        Name = this.CDT.FirstName + " " + this.CDT.SurName,
                        Email = this.CDT.EmailAddress,
                        InvoiceSettings = new CustomerInvoiceSettingsOptions
                        {
                            DefaultPaymentMethod = model.paymentmethod,
                        },
                    };
                    var stripeCustomerService = new CustomerService();
                    Customer stripeCustomer = stripeCustomerService.Create(stripeCustomerCreateOptions);

                   

                    
                    var stripeItems = new List<SubscriptionItemOption> {
                      new SubscriptionItemOption {
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
                    _cm.UpdateSocialProfileStripeCustomer(model.socialProfileId, stripeCustomer.Id, stripeSubscription.Id, newPlan.PlanId);

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
                    paymentRec.SocialProfileId = model.socialProfileId;
                    paymentRec.StripeSubscriptionId = stripeSubscription.Id;
                    paymentRec.Description = stripeSubscription.Plan.Nickname;
                    paymentRec.Name = stripeSubscription.Plan.Nickname;
                    paymentRec.Price = stripeSubscription.Plan.Amount/100;
                    //-- subDTO.Price = stripeSubscription.Plan.Amount;
                    paymentRec.StripePlanId = newPlan.StripePlanId;
                    paymentRec.SubscriptionType = stripeSubscription.Plan.Interval;

                    paymentRec.StartDate = stripeSubscription.CurrentPeriodStart.Value;
                    paymentRec.EndDate = stripeSubscription.CurrentPeriodEnd.Value;
                    paymentRec.StatusId = (int)GlobalEnums.PlanSubscription.Active;
                    paymentRec.PaymentPlanId = newPlan.PlanId;
                    paymentRec.StripeInvoiceId = stripeSubscription.LatestInvoiceId;
                    paymentRec.PaymentDateTime = DateTime.Now;

                    _cm.InsertSocialProfilePayment(paymentRec);
                   

                    var nt = new NotificationDTO()
                    {
                        Notification = string.Format(NotificationMessages[(int)NotificationMessagesIndexes.PlanSubscribe], stripeSubscription.Plan.Nickname),
                        CreatedBy = model.socialProfileId.ToString(),
                        CreatedOn = System.DateTime.Now,
                        Updatedby = model.socialProfileId.ToString(),
                        UpdateOn = DateTime.Now,
                        SocialProfileId = model.socialProfileId,
                        StatusId = (int)GeneralStatus.Unread,
                        Mode = severMode
                    };
                    _notManager.AddNotification(nt);


                    //--TODO: Update Klaviyo Web API Key
                    var _klaviyoPublishKey = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klaviyo").ToLower()).ConfigValue;
                    var _klavio_PayingSubscribeList = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klavio_PayingSubscribeList").ToLower()).ConfigValue;
                    var _klavio_NonPayingSubscribeList = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klavio_NonPayingSubscribeList").ToLower()).ConfigValue;
                    klaviyoAPI.Klaviyo_DeleteFromList(this.CDT.EmailAddress, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, _klavio_NonPayingSubscribeList);

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
                    var add = klaviyoAPI.Klaviyo_AddtoList(klaviyoProfile, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, _klavio_PayingSubscribeList);


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
        //public ActionResult SaveTargetPreferencesOnly(TargetPreferencesViewModel model)
        //{
        //    try
        //    {
        //        var jr = new JsonResult();
        //        if (ModelState.IsValid)
        //        {
        //            CustomerTargetProfileDTO profileDTO = _cm.GetSocialProfilesById(model.SocialProfileId ?? 0);

        //            if (!profileDTO.IsJVServerRunning)
        //            {
        //                severMode = "Manual";
        //            }

        //            var dl = _targetPreferenceManager.SaveTargetPreferences(new TargetPreferencesDTO()
        //            {
        //                Preference1 = model.Preference1,
        //                Preference2 = model.Preference2,
        //                Preference3 = model.Preference3,
        //                Preference4 = model.Preference4,
        //                Preference5 = model.Preference5,
        //                Preference6 = model.Preference6,
        //                Preference7 = model.Preference7,
        //                Preference8 = model.Preference8,
        //                Preference9 = model.Preference9,
        //                Preference10 = model.Preference10,
        //                SocialProfileId = model.SocialProfileId,
        //                ProfileName = model.ProfileName,
        //                CustomerId = this.CDT.CustomerId,
        //                QueueStatusId = (Int16)GlobalEnums.QueueStatus.Pending,
        //                SocialAccAs = model.SocialAccAS
        //            });


        //                var nt = new NotificationDTO()
        //            {
        //                Notification = NotificationMessages[(int)NotificationMessagesIndexes.UpdateTargetPreferences],
        //                CreatedBy = profileDTO.SocialProfileId.ToString(),
        //                CreatedOn = System.DateTime.Now,
        //                Updatedby = profileDTO.SocialProfileId.ToString(),
        //                UpdateOn = DateTime.Now,
        //                SocialProfileId = profileDTO.SocialProfileId,
        //                StatusId = (int)GeneralStatus.Unread,
        //                Mode = severMode
        //            };
        //            _notManager.AddNotification(nt);


        //            var JVListingData = _customerManager.SetSocialProfileArchive(model.SocialProfileId ?? 0, 0);
        //            if (JVListingData == true)
        //            {
        //                var ntUnarchive = new NotificationDTO()
        //                {
        //                    Notification = NotificationMessages[(int)NotificationMessagesIndexes.UnarchiveFromTargetPreferences],
        //                    CreatedBy = profileDTO.SocialProfileId.ToString(),
        //                    CreatedOn = System.DateTime.Now,
        //                    Updatedby = profileDTO.SocialProfileId.ToString(),
        //                    UpdateOn = DateTime.Now,
        //                    SocialProfileId = profileDTO.SocialProfileId,
        //                    StatusId = (int)GeneralStatus.Unread,
        //                    Mode = severMode
        //                };
        //                _notManager.AddNotification(ntUnarchive);

        //            }

        //            if (dl != null)
        //            {
        //                var SPId = dl.SocialProfileId;
        //                jr.Data = new { ResultType = "Success", message = "Target Preferences Updated Successfully", SPId };
        //            }
        //            else
        //            {
        //                jr.Data = new { ResultType = "Error", message = "Error." };
        //            }
        //            return Json(jr, JsonRequestBehavior.AllowGet);
        //            //--Queue Logic to update preferences to JV according to Plan/Subscription
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //[HttpPost]
        //public ActionResult SaveSocialProfileData(string InstaUser = "",
        //                                            string InstaPassword = "",
        //                                            int City = 0,
        //                                            int Country = 0,
        //                                            int? SocialProfileId = null,
        //                                            string sessionId = "",
        //                                            string action = "",
        //                                            string verificationCode = "",
        //                                            int invalidCredentialsAttempts = 0)
        //{


        //    var jr = new JsonResult();

        //    try
        //    {
        //        int proxyNoOfAttemps = 0;
        //        var _googleApiKey = SystemConfigs.First(x => x.ConfigKey == "GoogleMapApiKey").ConfigValue;

        //    BadIP:
        //        if (SocialProfileId != null && SocialProfileId > 0)
        //        {
        //            CustomerTargetProfileDTO profileDTO = _cm.GetSocialProfilesById(SocialProfileId ?? 0);
        //            string rpcSessionId = (string)_sessionManager.Get(GlobalEnums.SessionConstants.JVRPCSESSIONID);
        //            var prevInstaUser = profileDTO.SocialUsername;
        //            var prevJVStatusId = profileDTO.JVStatusId;

        //            if (!profileDTO.IsJVServerRunning)
        //            {
        //                severMode = "Manual";
        //            }

        //            #region Check if profile is block due attempts
        //            if (
        //                profileDTO.JVStatusId == (int)GlobalEnums.JVStatus.ProxyAndInternetIssue
        //                && profileDTO.JVAttemptsBlockedTill != null
        //                && profileDTO.JVAttemptsBlockedTill >= DateTime.Now)
        //            {
        //                jr.Data = new
        //                {
        //                    ResultType = "Success",
        //                    Message = "",
        //                    ResultData = new { JVStaus = "ProxyAttemptLater24Hrs", JVRPCSessionId = rpcSessionId, JVStatusId = (int)GlobalEnums.JVStatus.ProxyAndInternetIssue }
        //                };
        //                return jr;
        //            }
        //            if (
        //                (profileDTO.JVStatusId == (int)GlobalEnums.JVStatus.InvalidCredentials
        //                    || profileDTO.JVStatusId == (int)GlobalEnums.JVStatus.InvalidCredentialReSend
        //                )
        //                && profileDTO.JVAttemptsBlockedTill != null
        //                && profileDTO.JVAttemptsBlockedTill >= DateTime.Now)
        //            {
        //                jr.Data = new
        //                {
        //                    ResultType = "Success",
        //                    Message = "",
        //                    ResultData = new { JVStaus = "InvalidCredentailsAttemptLater24Hrs", JVRPCSessionId = rpcSessionId, JVStatusId = (int)GlobalEnums.JVStatus.ProxyAndInternetIssue }
        //                };
        //                return jr;
        //            }
        //            #endregion

        //            if (proxyNoOfAttemps > 0)
        //            {
        //                _proxyManager.SaveBadProxyIP(profileDTO.ProxyId ?? 0, profileDTO.SocialProfileId);
        //                profileDTO.ProxyIPNumber = null;
        //            }

        //            if (
        //                    profileDTO.JVStatusId == (int)GlobalEnums.JVStatus.ProfileNotSetup
        //                    || profileDTO.JVStatusId == (int)GlobalEnums.JVStatus.InvalidCredentials
        //                    || profileDTO.JVStatusId == (int)GlobalEnums.JVStatus.InvalidCredentialReSend
        //                )
        //            {
        //                var User = _targetPreferenceManager.SaveSocialProfileData(InstaUser, InstaPassword, City, Country, SocialProfileId ?? 0, 0, verificationCode);
        //                var nt = new NotificationDTO()
        //                {
        //                    Notification = NotificationMessages[(int)NotificationMessagesIndexes.UpdateSocailProfile],
        //                    CreatedBy = profileDTO.SocialProfileId.ToString(),
        //                    CreatedOn = DateTime.Now,
        //                    Updatedby = profileDTO.SocialProfileId.ToString(),
        //                    UpdateOn = DateTime.Now,
        //                    SocialProfileId = profileDTO.SocialProfileId,
        //                    StatusId = (int)GeneralStatus.Unread,
        //                    Mode = severMode

        //                };
        //                _notManager.AddNotification(nt);
        //            }

        //            if (
        //                    profileDTO.JVStatusId == (int)GlobalEnums.JVStatus.EmailVerificationRequired
        //                    || profileDTO.JVStatusId == (int)GlobalEnums.JVStatus.TwoFactor
        //                )
        //            {
        //                var User = _targetPreferenceManager.SaveSocialProfileData("", "", 0, 0, SocialProfileId ?? 0, 0, verificationCode);
        //                var nt = new NotificationDTO()
        //                {
        //                    Notification = string.Format(NotificationMessages[(int)NotificationMessagesIndexes.ProfileVerificationCode], verificationCode),
        //                    CreatedBy = profileDTO.SocialProfileId.ToString(),
        //                    CreatedOn = System.DateTime.Now,
        //                    Updatedby = profileDTO.SocialProfileId.ToString(),
        //                    UpdateOn = DateTime.Now,
        //                    SocialProfileId = profileDTO.SocialProfileId,
        //                    StatusId = (int)GeneralStatus.Unread,
        //                    Mode = severMode

        //                };
        //                _notManager.AddNotification(nt);
        //            }

        //            #region Assing MPBox
        //            if (profileDTO.JVboxId == null || profileDTO.JVboxId == 0)
        //            {
        //                 var jVBox = _cm.AssignJVBoxToCustomer(this.CDT.CustomerId, profileDTO.SocialProfileId);

        //                 var jvBoxName = jVBox.BoxName;

        //                if (!_jVBoxManager.JVBoxGetServerRunningStatus((int)jVBox.JVBoxId))
        //                {
        //                    severMode = "Manual";
        //                }

        //                var nt = new NotificationDTO()
        //                {
        //                    Notification = string.Format(NotificationMessages[(int)NotificationMessagesIndexes.MPBoxAssign], jvBoxName),
        //                    CreatedBy = profileDTO.SocialProfileId.ToString(),
        //                    CreatedOn = DateTime.Now,
        //                    Updatedby = profileDTO.SocialProfileId.ToString(),
        //                    UpdateOn = DateTime.Now,
        //                    SocialProfileId = profileDTO.SocialProfileId,
        //                    StatusId = (int)GeneralStatus.Unread,
        //                    Mode = severMode
        //                };
        //                _notManager.AddNotification(nt);
        //            }
        //            #endregion

        //            #region AssignIpAddress
        //            if (string.IsNullOrEmpty(profileDTO.ProxyIPNumber))
        //            {
        //                if (City > 0)
        //                {
        //                    var city = CommonManager.GetCityAndCountryData(City).FirstOrDefault();
        //                    if (city != null)
        //                    {
        //                        var proxyInfo = _commonManager.AssignedNearestProxyIP(this.CDT.CustomerId, city.CountyCityName.Replace(",", ""), SocialProfileId ?? 0, _googleApiKey);
        //                        if (proxyInfo != null)
        //                        {
        //                            profileDTO.ProxyIPNumber = proxyInfo.ProxyIPNumber;
        //                            profileDTO.ProxyPort = proxyInfo.ProxyPort;
        //                            profileDTO.ProxyIPName = proxyInfo.ProxyIPName;

        //                            var nt = new NotificationDTO()
        //                            {
        //                                Notification = string.Format(NotificationMessages[(int)NotificationMessagesIndexes.IPAssinged], proxyInfo.ProxyIPNumber, proxyInfo.ProxyPort),
        //                                CreatedBy = profileDTO.SocialProfileId.ToString(),
        //                                CreatedOn = System.DateTime.Now,
        //                                Updatedby = profileDTO.SocialProfileId.ToString(),
        //                                UpdateOn = DateTime.Now,
        //                                SocialProfileId = profileDTO.SocialProfileId,
        //                                StatusId = (int)GeneralStatus.Unread,
        //                                Mode= severMode
        //                            };
        //                            _notManager.AddNotification(nt);

        //                        }
        //                    }
        //                }
        //            }
        //            #endregion

        //            #region CheckingServerIsRunning

        //            if (!profileDTO.IsJVServerRunning)
        //            {
        //                _cm.BlockProfile24Hrs(SocialProfileId ?? 0, proxyNoOfAttemps);
        //                jr.Data = new
        //                {
        //                    ResultType = "Success",
        //                    Message = "",
        //                    ResultData = new
        //                    {
        //                        JVStaus = "ServerNotRunning",
        //                        JVRPCSessionId = rpcSessionId,
        //                        JVStatusId = (int)GlobalEnums.JVStatus.InvalidCredentials,
        //                        InvalidCredentialsAttempts = invalidCredentialsAttempts + 1
        //                    }
        //                };
        //                return jr;
        //            }

        //            #endregion

        //            jr.Data = new
        //            {
        //                ResultType = "Success",
        //                Message = "",
        //                ResultData = new { JVStaus = "ProfileSaved" }
        //            };
        //            return jr;

        //        }
        //        else
        //        {
        //            jr.Data = new { ResultType = "Error", Message = "Social Profile not found. Please contact admin." };
        //        }
        //        return jr;
        //    }
        //    catch (Exception ex)
        //    {
        //        jr.Data = new
        //        {
        //            ResultType = "Success",
        //            Message = "Profile setup session expired. Please try again later after 30 mints.",
        //            ResultData = new { JVStaus = "Exception", JVRPCSessionId = "", JVStatusId = "" }
        //        };
        //        return jr;
        //    }

        //}







        //public ActionResult GetStats(int socialProfileId)
        //{
        //    var jr = new JsonResult();
        //    try
        //    {
        //        StatisticsViewModel statisticsViewModel = _statisticsManager.GetStatistics(socialProfileId, DateTime.Now.AddDays(-15), DateTime.Now.AddDays(+5));
        //        if (statisticsViewModel.StatisticsListing != null)
        //        {
        //            jr.Data = new
        //            {
        //                ResultType = "Success",
        //                message = "",
        //                ResultData = new
        //                {
        //                    Date = statisticsViewModel.StatisticsListing.Select(x => x.Date.ToString("dd/MM/yyyy")).ToArray(),
        //                    FollowersData = statisticsViewModel.StatisticsListing.Select(x => x.Followers.ToString()).ToArray(),
        //                    FollowersGainData = statisticsViewModel.StatisticsListing.Select(x => x.FollowersGain.ToString()).ToArray(),
        //                    FollowingsData = statisticsViewModel.StatisticsListing.Select(x => x.Followings.ToString()).ToArray(),
        //                    FollowingsRatioData = statisticsViewModel.StatisticsListing.Select(x => x.FollowingsRatio.ToString()).ToArray(),
        //                    AVGFollowersData = statisticsViewModel.StatisticsListing.Select(x => x.AVGFollowers.ToString()).ToArray(),


        //                    LikeData = statisticsViewModel.StatisticsListing.Select(x => x.Like.ToString()).ToArray(),
        //                    CommentData = statisticsViewModel.StatisticsListing.Select(x => x.Comment.ToString()).ToArray(),
        //                    LikeCommentData = statisticsViewModel.StatisticsListing.Select(x => x.LikeComments.ToString()).ToArray(),
        //                    Engagement = statisticsViewModel.StatisticsListing.Select(x => x.Engagement.ToString()).ToArray(),


        //                    TotalComment = statisticsViewModel.TotalComment.ToString(),
        //                    TotalEngagement = statisticsViewModel.TotalEngagement.ToString(),
        //                    TotalFollowers = statisticsViewModel.TotalFollowers.ToString(),
        //                    TotalFollowersGain = statisticsViewModel.TotalFollowersGain.ToString(),
        //                    TotalFollowings = statisticsViewModel.TotalFollowings.ToString(),
        //                    TotalFollowingsRatio = statisticsViewModel.TotalFollowingsRatio.ToString(),
        //                    TotalLike = statisticsViewModel.TotalLike.ToString(),
        //                    TotalLikeComment = statisticsViewModel.TotalLikeComment.ToString()
        //                }
        //            };
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



        [HttpPost]
        public ActionResult Confirmdelete(int SocialProfileId)
        {
            var jr = new JsonResult();
            try
            {

                KlaviyoAPI klaviyoAPI = new KlaviyoAPI();
                KlaviyoProfile klaviyoProfile = new KlaviyoProfile();
                KlaviyoEvent ev = new KlaviyoEvent();

                SocialProfileDTO profileDTO = _cm.GetSocialProfileById(SocialProfileId);


                //if (!profileDTO.IsJVServerRunning)
                //{
                //    severMode = "Manual";
                //}
                var _stripeApiKey = SystemConfig.GetConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;
                if (profileDTO != null)
                {

                    if (profileDTO.SocialProfile.StatusId != 18)
                    {
                        if (!string.IsNullOrEmpty(profileDTO.SocialProfile.StripeSubscriptionId))
                        {
                            StripeConfiguration.SetApiKey(_stripeApiKey);
                            var service = new SubscriptionService();
                            var sub = service.Get(profileDTO.SocialProfile.StripeSubscriptionId);

                            var subscription = service.Cancel(sub.Id,null);

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
                                    CreatedBy = profileDTO.SocialProfile.SocialProfileId.ToString(),
                                    CreatedOn = DateTime.Now,
                                    Updatedby = profileDTO.SocialProfile.SocialProfileId.ToString(),
                                    UpdateOn = DateTime.Now,
                                    SocialProfileId = profileDTO.SocialProfile.SocialProfileId,
                                    StatusId = (int)GeneralStatus.Unread,
                                    Mode = severMode
                                };
                                _notManager.AddNotification(nt);


                                List<NotRequiredProperty> list = new List<NotRequiredProperty>()
                        {
                            new NotRequiredProperty("$email", this.CDT.EmailAddress),
                            new NotRequiredProperty("$first_name ", this.CDT.FirstName),
                            new NotRequiredProperty("$last_name ", this.CDT.SurName)
                        };
                                ev.Event = "Account Deleted";
                                ev.Properties.NotRequiredProperties = list;
                                ev.CustomerProperties.Email = CDT.EmailAddress;
                                ev.CustomerProperties.FirstName = CDT.EmailAddress;
                                ev.CustomerProperties.LastName = CDT.EmailAddress;

                                var _klaviyoPublishKey = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klaviyo").ToLower()).ConfigValue;
                                var _klavio_UnsubscribeList = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klavio_UnsubscribeList").ToLower()).ConfigValue;
                                var _klavio_PayingSubscribeList = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klavio_PayingSubscribeList").ToLower()).ConfigValue;

                                klaviyoAPI.EventAPI(ev, _klaviyoPublishKey);
                                klaviyoAPI.Klaviyo_DeleteFromList(this.CDT.EmailAddress, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, _klavio_PayingSubscribeList);
                                var add = klaviyoAPI.Klaviyo_AddtoList(klaviyoProfile, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, _klavio_UnsubscribeList);
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

                //var exchangeNames = profileDTO.JVBoxExchangeName.Split(',');
                //--TODO: Check subscription status here
                //SubscriptionDTO subDTO = new SubscriptionDTO();
                //subDTO.CustomerId = this.CDT.CustomerId;
                //subDTO.StripeSubscriptionId = subscription.Id;
                //subDTO.Description = subscription.Plan.Nickname;
                //subDTO.Name = subscription.Plan.Nickname;
                //subDTO.Price = subscription.Plan.Amount;
                //subDTO.StripePlanId = subscription.Plan.Id;
                //subDTO.SubscriptionType = subscription.Plan.Interval;
                //subDTO.StartDate = subscription.Start ?? DateTime.Now;
                //subDTO.EndDate = subscription.EndedAt ?? DateTime.Now.AddMonths(1);
                //subDTO.StatusId = 26;// Canceled Subscription
                //_customerManager.InsertSubscription(subDTO);

                //int socialProfileId = 1;//TODO: Social Profile Id
                if (_customerManager.DeleteProfile(this.CDT.CustomerId, SocialProfileId))
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
                        jr.Data = new { ResultType = "Error", message = "User has successfully deleted." };
                    }
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return Json(jr, JsonRequestBehavior.AllowGet);
        }

    }
}