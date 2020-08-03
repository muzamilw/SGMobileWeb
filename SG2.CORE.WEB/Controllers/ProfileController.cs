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
using SG2.CORE.MODAL.ViewModals.CRM;
using Stripe.Checkout;
using Stripe.Infrastructure;


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
        public ActionResult BlackList(int socialProfileId, int? success)
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
        public ActionResult WhiteList(int socialProfileId, int? success)
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
        public ActionResult WhiteList(SocialProfileDTO request)
        {
            this._cm.UpdateTargetProfileWhiteList(request);
            return RedirectToAction("WhiteList", "Profile", new { socialProfileId = request.SocialProfile_Instagram_TargetingInformation.SocialProfileId, success = 1 });
        }

        [HttpPost]
        public ActionResult BlackList(SocialProfileDTO request)
        {
            this._cm.UpdateTargetProfileBlackList(request);
            return RedirectToAction("BlackList", "Profile", new { socialProfileId = request.SocialProfile_Instagram_TargetingInformation.SocialProfileId, success = 1 });
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

            ViewBag.Customer = customer;

            

            if (success.HasValue && success.Value == 1)
            {
                ViewBag.success = 1;
            }

           

            return View(SocailProfile);

        }


		[HttpPost]
		public ActionResult Target(SocialProfileDTO request)
		{
            var customer = _customerManager.GetCustomerByCustomerId(this.CDT.CustomerId);
            var brokermode = customer.IsBroker.HasValue && customer.IsBroker.Value == true;

            this._cm.UpdateTargetProfile(request, brokermode);
			return RedirectToAction("Target", "Profile", new { socialProfileId = request.SocialProfile_Instagram_TargetingInformation.SocialProfileId, success = 1 });
		}
        public ActionResult Stats(int socialProfileId)
        {
            ViewBag.socialProfileId = socialProfileId;
            ViewBag.CurrentUser = this.CDT;
            ViewBag.socialProfile = this._cm.GetSocialProfileById(socialProfileId).SocialProfile;

            List<SocialProfile_ActionsViewModel> actions = this._cm.ReturnLastActions(socialProfileId, 500).Where(g => g.ActionID != 69).ToList();
            ViewBag.actions = actions;
            StatisticsViewModel statisticsViewModel = this._statisticsManager.GetStatistics(socialProfileId);
            statisticsViewModel.AppStatus = "Offline";
            if (actions.Count > 0)
            {
                SocialProfile_ActionsViewModel socialProfile_Actions = actions.OrderBy(o => o.ActionDateTime).FirstOrDefault();
                if (socialProfile_Actions != null && socialProfile_Actions.ActionDateTime != null)
                {
                    DateTime actionDateTime = Convert.ToDateTime(socialProfile_Actions.ActionDateTime);
                    TimeSpan span = DateTime.Now.Subtract(actionDateTime);
                    if (span.TotalMinutes <= 3)
                    {
                        statisticsViewModel.AppStatus = "Online";
                    }
                }
            }

            return View(statisticsViewModel);
           

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
                DateTime startdate = DateTime.Today.AddDays(-15);   //1 week
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
                    item.Followings = item.Followings.HasValue ? item.Followings : 0;
                    item.Like = item.Like.HasValue ? item.Like : 0;
                    item.Engagement = (item.FollowersTotal ?? 1) * 100 / ((item.Followings ?? 1) == 0 ? 1: (item.Followings ?? 1));
                    item.Unfollow = (item.Unfollow ?? 0);
                    item.StoryViews = (item.StoryViews ?? 0);
                    item.Comment = (item.Comment ?? 0);
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
                            FollowingsData = trends.Select(x => x.Followings.ToString()).ToArray(),
                            //FollowingsRatioData = statisticsViewModel.StatisticsListing.Select(x => x.FollowingsRatio.ToString()).ToArray(),
                            //AVGFollowersData = statisticsViewModel.StatisticsListing.Select(x => x.AVGFollowers.ToString()).ToArray(),


                            //LikeData = statisticsViewModel.StatisticsListing.Select(x => x.Like.ToString()).ToArray(),
                            //CommentData = statisticsViewModel.StatisticsListing.Select(x => x.Comment.ToString()).ToArray(),
                            //LikeCommentData = statisticsViewModel.StatisticsListing.Select(x => x.LikeComments.ToString()).ToArray(),
                            Engagement = trends.Select(x => x.Engagement.ToString()).ToArray(),

                            AvgLikes = trends.Select(x => x.Like.ToString()).ToArray(),
                            StoryViews = trends.Select(x => x.StoryViews.ToString()).ToArray(),
                            Unfollow = trends.Select(x => x.Unfollow.ToString()).ToArray(),
                            Comment = trends.Select(x => x.Comment.ToString()).ToArray()

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


        public ActionResult History(int socialProfileId, string buy = "", string buysession= "")
        {
            ViewBag.socialProfileId = socialProfileId;
            ViewBag.CurrentUser = this.CDT;
            var SocailProfile = this._cm.GetSocialProfileById(socialProfileId);

            var customer = _customerManager.GetCustomerByCustomerId(this.CDT.CustomerId);

            ViewBag.Customer = customer;

            ViewBag.Plans = _planManager.GetallIntagramPaymentPlans(customer.IsBroker.HasValue ? customer.IsBroker.Value : false);

            var _stripeApiKey = SystemConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;
            var _stripePublishKey = SystemConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue2;

            ViewBag.stripeApiKey = _stripeApiKey;
            ViewBag.stripePublishKey = _stripePublishKey;

            if (buysession  != "")
            {
                ViewBag.purchasesuccess = 1;
            }

            return View(SocailProfile);

        }

        public ActionResult UpdatePhone(string socialprofileid, string Phone)
        {
            try
            {
                _customerManager.UpdateCustomerContactPhone(Phone, Convert.ToInt32(socialprofileid));
            }
            catch (Exception)
            {

                throw;
            }

            return this.Content("ok");
            
        }

            public ActionResult BuyPhone(string socialprofileid,string email,string pageUrl)
        {
            // Set your secret key. Remember to switch to your live secret key in production!
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            var _stripeApiKey = SystemConfig.GetConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;
            StripeConfiguration.ApiKey = _stripeApiKey;

            var options = new SessionCreateOptions
            { ClientReferenceId = socialprofileid,
                CustomerEmail = email,
                BillingAddressCollection = "auto",
                PaymentMethodTypes = new List<string> {
                "card",
            },

            ShippingAddressCollection = new SessionShippingAddressCollectionOptions
            {
                AllowedCountries = new List<string> {
                    "US",
                    "CA",
                    "AU","NZ","GB","IE"
                    },
               
            },
            
            LineItems = new List<SessionLineItemOptions> {
            new SessionLineItemOptions {
                Name = "Dedicated android device",
                Description = "Dedicated android device pre-installed with social growth labs app delivered to your shipping address",
                Amount = 5000,
                Currency = "usd",
                Quantity = 1
                    
            },
            },
                SuccessUrl = pageUrl + "&buysession={CHECKOUT_SESSION_ID}",
                CancelUrl = pageUrl + "&buy=cancel",

            };

            var service = new SessionService();
            Session session = service.Create(options);
            session.ClientReferenceId = socialprofileid;
            

            return this.Content(session.Id) ;
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

                        var items = new List<SubscriptionItemOptions> {
                                        new SubscriptionItemOptions {
                                        
                                        Id= subscriptionItemUpdate.Items.Data[0].Id,
                                        Plan = newPlan.StripePlanId,
                                        Quantity= 1,
                                        },
                                    };

                        var subscriptionUpdateoptions = new SubscriptionUpdateOptions
                        {
                            Items = items.ToList(),

                            Prorate = true,
                            ProrationDate = DateTime.Now,
                            //ProrationDate = DateTime.Now,

                        };
                        subscriptionUpdateoptions.AddExpand("latest_invoice.payment_intent");
                        stripeSubscription = subscriptionService.Update(socialProfile.SocialProfile.StripeSubscriptionId, subscriptionUpdateoptions);
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
                        Description = " Regular Profile " + this.CDT.EmailAddress + " ---socialusername=" + socialProfile.SocialProfile.SocialUsername + " ---profileid=" + model.socialProfileId,
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


                    //updating the profile's flag to true after upgrade/purchase
                    try
                    {
                        if (newPlan.PlanId != 1)
                        {
                            socialProfile.SocialProfile_Instagram_TargetingInformation.FollowOn = true;
                            socialProfile.SocialProfile_Instagram_TargetingInformation.UnFollowOn = true;
                            socialProfile.SocialProfile_Instagram_TargetingInformation.AfterFollLikeuserPosts = true;
                            socialProfile.SocialProfile_Instagram_TargetingInformation.AfterFollViewUserStory = true;
                            socialProfile.SocialProfile_Instagram_TargetingInformation.UnFollFollowersAfterMinDays = true;

                            _cm.UpdateTargetProfile(socialProfile, true);

                        }
                    }
                    catch (Exception e)
                    {

                        throw;
                    }
                   

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
                    //var _klaviyoPublishKey = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klaviyo").ToLower()).ConfigValue;
                    //var Klavio_FreeCustomers = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klavio_FreeCustomers").ToLower()).ConfigValue;
                    //var Klavio_PayingCustomers = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klavio_PayingCustomers").ToLower()).ConfigValue;
                    
                    //klaviyoProfile.email = this.CDT.EmailAddress;

                    //if (newPlan.PlanId != 1)  //upgrading hence remove.
                    //    klaviyoAPI.Klaviyo_DeleteFromList(this.CDT.EmailAddress, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, Klavio_FreeCustomers);
                    //else
                    //    klaviyoAPI.Klaviyo_AddtoList(klaviyoProfile, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, Klavio_FreeCustomers);

                    //List<NotRequiredProperty> list = new List<NotRequiredProperty>()  {
                    //    new NotRequiredProperty("$email", this.CDT.EmailAddress),
                    //    new NotRequiredProperty("$first_name ", this.CDT.FirstName),
                    //    new NotRequiredProperty("$last_name ", this.CDT.SurName),
                    //    //new NotRequiredProperty("URL", URL),
                    //    new NotRequiredProperty("InvoiceDate",paymentRec.StartDate.ToString("dd MMMM yyyy") ),
                    //    new NotRequiredProperty("PlanName", paymentRec.Name),
                    //    new NotRequiredProperty("Price",  "$" + paymentRec.Price/100),
                    //    new NotRequiredProperty("Card", ""),
                    //    new NotRequiredProperty("Address","")
                    //};
                    



                    //klaviyoAPI.PeopleAPI(list, _klaviyoPublishKey);
                    //KlaviyoEvent ev = new KlaviyoEvent();

                    if (newPlan.PlanId != 1)
                    {
                        //klaviyoAPI.Klaviyo_AddtoList(klaviyoProfile, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, Klavio_PayingCustomers);
                        //ev.Event = "Plan Upgrade";
                        var dynamicTemplateData = new Dictionary<string, string>
                        {
                            {"name",this.CDT.FirstName},
                            {"email", this.CDT.EmailAddress},
                            {"senddate", DateTime.Today.ToLongDateString()},
                            {"invoicedate", paymentRec.StartDate.ToString("dd MMMM yyyy")},
                            {"planname", paymentRec.Name},
                             {"price", "$" + paymentRec.Price},
                              {"total", "$" + paymentRec.Price}

                        };
                        BAL.Managers.EmailManager.SendEmail(this.CDT.EmailAddress, this.CDT.FirstName, EmailManager.EmailType.PlanUpgrade, dynamicTemplateData);
                    }
                    else
                    {
                        //klaviyoAPI.Klaviyo_DeleteFromList(this.CDT.EmailAddress, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, Klavio_PayingCustomers);
                        //ev.Event = "Plan Downgrade";
                    }

                    //ev.Properties.NotRequiredProperties = list;
                    //ev.CustomerProperties.Email = this.CDT.EmailAddress;
                    //ev.CustomerProperties.FirstName = this.CDT.FirstName;
                    //ev.CustomerProperties.LastName = this.CDT.SurName;

                    //klaviyoAPI.EventAPI(ev, _klaviyoPublishKey);


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


                        //        List<NotRequiredProperty> list = new List<NotRequiredProperty>()
                        //{
                        //    new NotRequiredProperty("$email", this.CDT.EmailAddress),
                        //    new NotRequiredProperty("$first_name ", this.CDT.FirstName),
                        //    new NotRequiredProperty("$last_name ", this.CDT.SurName)
                        //};
                        //        ev.Event = "Account Deleted";
                        //        ev.Properties.NotRequiredProperties = list;
                        //        ev.CustomerProperties.Email = CDT.EmailAddress;
                        //        ev.CustomerProperties.FirstName = CDT.EmailAddress;
                        //        ev.CustomerProperties.LastName = CDT.EmailAddress;

                        //        var _klaviyoPublishKey = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klaviyo").ToLower()).ConfigValue;
                        //        var Klavio_FreeCustomers = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klavio_FreeCustomers").ToLower()).ConfigValue;
                        //        var Klavio_PayingCustomers = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klavio_PayingCustomers").ToLower()).ConfigValue;
                        //        var Klavio_DeletedCustomers = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("Klavio_DeletedCustomers").ToLower()).ConfigValue;

                        //        klaviyoAPI.EventAPI(ev, _klaviyoPublishKey);
                        //        klaviyoAPI.Klaviyo_DeleteFromList(this.CDT.EmailAddress, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, Klavio_FreeCustomers);
                        //        klaviyoAPI.Klaviyo_DeleteFromList(this.CDT.EmailAddress, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, Klavio_PayingCustomers);
                        //        var add = klaviyoAPI.Klaviyo_AddtoList(klaviyoProfile, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, Klavio_DeletedCustomers);
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

        public ActionResult FollowList(string id, string SPId)
        {
            int custId = Convert.ToInt32(id);

            ViewBag.socialProfileId = SPId;
            ViewBag.CurrentUser = this.CDT;
            SocialProfileDTO spDto = this._customerManager.GetSocialProfileById(Convert.ToInt32(SPId));
            ViewBag.socialProfile = spDto.SocialProfile;
            ViewBag.socialProfileFollowedAccounts = spDto.SocialProfile_FollowedAccounts;
            FollowListViewModel followList = new FollowListViewModel();
            followList.AppStatus = spDto.AppStatus;
            return View(followList);


        }

        [HttpPost]
        public ActionResult FollowList(FollowListViewModel followListViewModel)
        {
            int custId = followListViewModel.CustomerId;
            int socialProfileId = followListViewModel.SocialProfileId;
            SocialProfileDTO socialProfileDTO = this._customerManager.GetSocialProfileById(socialProfileId);
            ViewBag.socialProfileId = socialProfileId;
            ViewBag.CurrentUser = this.CDT;

            if (followListViewModel.FilterBy == "2")
            {
                socialProfileDTO.SocialProfile_FollowedAccounts = socialProfileDTO.SocialProfile_FollowedAccounts.Where(s => s.StatusId == 1 && s.FollowedDateTime == DateTime.Now.AddDays(-33)).ToList();
            }
            if (followListViewModel.FilterBy == "3")
            {
                socialProfileDTO.SocialProfile_FollowedAccounts = socialProfileDTO.SocialProfile_FollowedAccounts.Where(s => s.StatusId != 1 && s.FollowedDateTime == DateTime.Now.AddDays(-33)).ToList();
            }

            ViewBag.socialProfile = socialProfileDTO.SocialProfile;
            ViewBag.socialProfileFollowedAccounts = socialProfileDTO.SocialProfile_FollowedAccounts;

            followListViewModel.AppStatus = socialProfileDTO.AppStatus;
            return View(followListViewModel);


        }


        public ActionResult AllStats(int socialProfileId)
        {
            
            ViewBag.socialProfileId = socialProfileId;
            ViewBag.CurrentUser = this.CDT;
            ViewBag.socialProfile = this._customerManager.GetSocialProfileById(socialProfileId).SocialProfile;

            ViewBag.actions = this._customerManager.ReturnLastActions(socialProfileId, 500);
            ViewBag.growthSummary = this._statisticsManager.GetStatsGrowthSummary(socialProfileId);
            ViewBag.dailyStatsActivity = this._statisticsManager.GetStatsDailyActivity(socialProfileId, -15);
            return View(this._statisticsManager.GetStatistics(socialProfileId));
        }

        public ActionResult GetFollowersSection(int socialProfileId, int mode)
        {
            var jr = new JsonResult();
            try
            {
                double offSet = this._customerManager.GetAppTimeZoneOffSet(socialProfileId);
                DateTime offSetDate = DateTime.UtcNow.AddHours(offSet).Date;
                DateTime startdate = offSetDate.AddDays(-15);   //15 week
                DateTime enddate = offSetDate.AddHours(24);


                if (mode == 2)
                {
                    startdate = offSetDate.AddMonths(-1);  //1 months
                }
                else if (mode == 3)
                {
                    startdate = offSetDate.AddMonths(-3);  //3 months
                }
                else if (mode == 4)
                {
                    startdate = offSetDate.AddMonths(-6); //6 months
                }
                else if (mode == 5)
                {
                    startdate = offSetDate.AddMonths(-12); //12 months
                }

                var trends = _statisticsManager.GetProfileTrends(socialProfileId, startdate, enddate);

                var firstRecord = trends.FirstOrDefault();
                var lastRecord = trends.LastOrDefault();

                long? totalFollowers = 0;
                long? followersChange = 0;
                long? followersAvgChange = 0;
                long? followersMaxGrowth = 0;
                string followersMaxGrowthDate = "";
                long? followersChangePer = 0;
                long? totalLikes = 0;
                long? likesChange = 0;
                long? likesChangePer = 0;
                long? likesAvgChange = 0;
                long? likesMaxGrowth = 0;
                string likesMaxGrowthDate = "";


                if (firstRecord != null && lastRecord != null)
                {
                    totalFollowers = (lastRecord.FollowersTotal.HasValue ? lastRecord.FollowersTotal : 0);
                    totalLikes = (lastRecord.LikeTotal.HasValue ? lastRecord.LikeTotal : 0);
                    followersChange = totalFollowers - (firstRecord.FollowersTotal.HasValue ? firstRecord.FollowersTotal : 0);
                    likesChange = totalLikes - (firstRecord.LikeTotal.HasValue ? firstRecord.LikeTotal : 0);
                    followersAvgChange = (followersChange / (offSetDate - startdate).Days);
                    likesAvgChange = (likesChange / (offSetDate - startdate).Days);

                    if (totalFollowers > 0 && followersChange > 0)
                    {
                        followersChangePer = ((followersChange * 100 / totalFollowers));
                    }

                    if (totalLikes > 0 && likesChange > 0)
                    {
                        likesChangePer = (likesChange * 100 / totalLikes);
                    }
                }


                int count = 1;

                foreach (var item in trends)
                {

                    item.Followers = item.Followers.HasValue ? item.Followers : 0;
                    item.FollowersTotal = item.FollowersTotal.HasValue ? item.FollowersTotal : 0;
                    item.Followings = item.Followings.HasValue ? item.Followings : 0;
                    item.FollowingsTotal = item.FollowingsTotal.HasValue ? item.FollowingsTotal : 0;
                    item.Posts = item.Posts.HasValue ? item.Posts : 0;
                    item.AverageLikes = ((item.Like.HasValue ? item.Like : 0) / ((item.Posts.HasValue && item.Posts > 0) ? item.Posts : 1));
                    count++;
                    if (count > 1 && item.Followers > followersMaxGrowth)
                    {
                        followersMaxGrowth = item.Followers;
                        followersMaxGrowthDate = item.Date.ToString("on MMMM dd, yyyy ");
                    }
                    if (count > 1 && item.Like > likesMaxGrowth)
                    {
                        likesMaxGrowth = item.Like;
                        likesMaxGrowthDate = item.Date.ToString("on MMMM dd, yyyy ");
                    }
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
                            Followers = trends.Select(x => x.Followers.ToString()).ToArray(),
                            FollowersTotal = trends.Select(x => x.FollowersTotal.ToString()).ToArray(),
                            FollowingsTotal = trends.Select(x => x.FollowingsTotal.ToString()).ToArray(),
                            Followings = trends.Select(x => x.Followings.ToString()).ToArray(),
                            Post = trends.Select(x => x.Posts.ToString()).ToArray(),
                            AverageLikes = trends.Select(x => x.AverageLikes.ToString()).ToArray(),
                            TotalFollowers = totalFollowers.ToString(),
                            FollowersChange = followersChange.ToString(),
                            FollowersChangePer = followersChangePer.ToString(),
                            FollowersMaxGrowth = followersMaxGrowth.ToString(),
                            FollowersMaxGrowthDate = followersMaxGrowthDate,
                            FollowersAvgChange = followersAvgChange,
                            TotalLikes = totalLikes,
                            LikesChange = likesChange,
                            LikesChangePer = likesChangePer,
                            LikesMaxGrowth = likesMaxGrowth,
                            LikesMaxGrowthDate = likesMaxGrowthDate,
                            LikesAvgChange = likesAvgChange,
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

        public ActionResult GetEngagementSection(int socialProfileId, int mode)
        {
            var jr = new JsonResult();
            try
            {
                double offSet = this._customerManager.GetAppTimeZoneOffSet(socialProfileId);
                DateTime offSetDate = DateTime.UtcNow.AddHours(offSet).Date;
                DateTime startdate = offSetDate.AddDays(-15);   //15 week
                DateTime enddate = offSetDate.AddHours(24);


                if (mode == 2)
                {
                    startdate = offSetDate.AddMonths(-1);  //1 months
                }
                else if (mode == 3)
                {
                    startdate = offSetDate.AddMonths(-3);  //3 months
                }
                else if (mode == 4)
                {
                    startdate = offSetDate.AddMonths(-6); //6 months
                }
                else if (mode == 5)
                {
                    startdate = offSetDate.AddMonths(-12); //12 months
                }


                var trends = _statisticsManager.GetProfileTrends(socialProfileId, startdate, enddate);

                foreach (var item in trends)
                {
                    item.Followings = item.Followings.HasValue ? item.Followings : 0;
                    item.Unfollow = item.Unfollow.HasValue ? item.Unfollow : 0;
                    item.Like = item.Like.HasValue ? item.Like : 0;
                    item.StoryViews = item.StoryViews.HasValue ? item.StoryViews : 0;
                    item.Comment = (item.Comment ?? 0);
                    item.Engagement = ((item.StoryViewsTotal ?? 0) + (item.LikeTotal ?? 0) + item.Comment + (item.FollowingsTotal ?? 0) - (item.Unfollow ?? 0)) / ((item.FollowersTotal ?? 1) == 0 ? 1 : item.FollowersTotal);
                    
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
                            Followings = trends.Select(x => x.Followings.ToString()).ToArray(),
                            Engagement = trends.Select(x => x.Engagement.ToString()).ToArray(),
                            Likes = trends.Select(x => x.Like.ToString()).ToArray(),
                            StoryViews = trends.Select(x => x.StoryViews.ToString()).ToArray(),
                            Unfollow = trends.Select(x => x.Unfollow.ToString()).ToArray(),
                            Comment = trends.Select(x => x.Comment.ToString()).ToArray()

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


    }
}