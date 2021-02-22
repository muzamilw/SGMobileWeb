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
        //protected readonly TargetPreferencesManager _targetPreferenceManager;
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
            SystemConfigs = Architecture.SystemConfig.GetConfigsLatest();
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

        public ActionResult test()
        {
            ViewBag.CurrentUser = this.CDT;
            ViewBag.socialProfileId = 68;
            var SocailProfile = this._cm.GetSocialProfileById(68);
            ViewBag.socialProfile = SocailProfile.SocialProfile;
            ProfilesSearchRequest model = new ProfilesSearchRequest { Block = 99, Plan = 0, searchString = "", SocialType = 0 };
            ViewBag.ProfileCount = this._customerManager.GetSocialProfilesByCustomerid(this.CDT.CustomerId, model).Count();

            ViewBag.winapp = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("winapp").ToLower()).ConfigValue;
            ViewBag.macapp = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("macapp").ToLower()).ConfigValue;
            return View(SocailProfile);
        }

        public ActionResult Basic(int socialProfileId, int? success)
        {


            return RedirectToAction("startin", "Profile", new { socialProfileId = socialProfileId });


            ViewBag.CurrentUser = this.CDT;
            ViewBag.socialProfileId = socialProfileId;
            var SocailProfile = this._cm.GetSocialProfileById(socialProfileId);

            var url = this.Request.Headers["HOST"];
            //redirect to start page.

            if (url.Contains("saleflow") || url.Contains("connectwizz"))
            {
                return RedirectToAction("startin", "Profile", new { socialProfileId = socialProfileId });
            }
            else
            {
                if (SocailProfile.SocialProfile.PaymentPlanId == null || SocailProfile.SocialProfile.PaymentPlanId == 1)
                {
                    return RedirectToAction("start", "Profile", new { socialProfileId = socialProfileId });
                }
            }



                ProfilesSearchRequest model = new ProfilesSearchRequest { Block = 99, Plan = 0, searchString = "", SocialType = 0 };
                ViewBag.ProfileCount = this._customerManager.GetSocialProfilesByCustomerid(this.CDT.CustomerId, model).Count();

                ViewBag.winapp = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("winapp").ToLower()).ConfigValue;
                ViewBag.macapp = SystemConfigs.First(x => x.ConfigKey.ToLower() == ("macapp").ToLower()).ConfigValue;

                if (_sessionManager.Get(SessionConstants.profileId) == null && Convert.ToInt32(_sessionManager.Get(SessionConstants.profileId)) != socialProfileId)
                {
                    if (SocailProfile.SocialProfile.StripeSubscriptionId != null)
                    {
                        var _stripeApiKey = SystemConfig.GetConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;
                        StripeConfiguration.SetApiKey(_stripeApiKey);
                        var subscriptionService = new SubscriptionService();
                        Subscription subscriptionStatus = subscriptionService.Get(SocailProfile.SocialProfile.StripeSubscriptionId);
                        bool trialFlag = false;
                        if (subscriptionStatus.TrialEnd.HasValue && subscriptionStatus.TrialEnd.Value > DateTime.Now)
                        {
                            ViewBag.IsTrialActive = true;
                            trialFlag = true;
                        }
                        else
                        {
                            ViewBag.IsTrialActive = false;
                            trialFlag = false;
                        }

                        _sessionManager.Set(SessionConstants.profileTrialActive, trialFlag);
                        _sessionManager.Set(SessionConstants.profileId, SocailProfile.SocialProfile.SocialProfileId);
                    }
                    else
                    {
                        _sessionManager.Set(SessionConstants.profileTrialActive, false);
                        _sessionManager.Set(SessionConstants.profileId, SocailProfile.SocialProfile.SocialProfileId);
                    }
                }
                else
                {
                    ViewBag.IsTrialActive = _sessionManager.Get(SessionConstants.profileTrialActive);

                }

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
            return RedirectToAction("Basic", "Profile", new { socialProfileId = request.SocialProfile.SocialProfileId, success = 1 });
        }

        public ActionResult Lists(int socialProfileId, int? success)
        {
            ViewBag.socialProfileId = socialProfileId;
            ViewBag.CurrentUser = this.CDT;
            var SocailProfile = this._cm.GetSocialProfileById(socialProfileId);
            ProfilesSearchRequest model = new ProfilesSearchRequest { Block = 99, Plan = 0, searchString = "", SocialType = 0 };
            ViewBag.ProfileCount = this._customerManager.GetSocialProfilesByCustomerid(this.CDT.CustomerId, model).Count();

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
            ProfilesSearchRequest model = new ProfilesSearchRequest { Block = 99, Plan = 0, searchString = "", SocialType = 0 };
            ViewBag.ProfileCount = this._customerManager.GetSocialProfilesByCustomerid(this.CDT.CustomerId, model).Count();

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
            ProfilesSearchRequest model = new ProfilesSearchRequest { Block = 99, Plan = 0, searchString = "", SocialType = 0 };
            ViewBag.ProfileCount = this._customerManager.GetSocialProfilesByCustomerid(this.CDT.CustomerId, model).Count();

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
            ViewBag.IsTrialActive = _sessionManager.Get(SessionConstants.profileTrialActive);
            ViewBag.socialProfileId = socialProfileId;
            ViewBag.CurrentUser = this.CDT;
            var SocailProfile = this._cm.GetSocialProfileById(socialProfileId);
            var customer = _customerManager.GetCustomerByCustomerId(this.CDT.CustomerId);
            ProfilesSearchRequest model = new ProfilesSearchRequest { Block = 99, Plan = 0, searchString = "", SocialType = 0 };
            ViewBag.ProfileCount = this._customerManager.GetSocialProfilesByCustomerid(this.CDT.CustomerId, model).Count();

            ViewBag.Customer = customer;



            if (success.HasValue && success.Value == 1)
            {
                ViewBag.success = 1;
            }



            return View(SocailProfile);

        }

        public ActionResult start(int socialProfileId)
        {
            
            ViewBag.socialProfileId = socialProfileId;
            ViewBag.CurrentUser = this.CDT;
            var SocailProfile = this._cm.GetSocialProfileById(socialProfileId);
            var customer = _customerManager.GetCustomerByCustomerId(this.CDT.CustomerId);
            //ProfilesSearchRequest model = new ProfilesSearchRequest { Block = 99, Plan = 0, searchString = "", SocialType = 0 };
            //ViewBag.ProfileCount = this._customerManager.GetSocialProfilesByCustomerid(this.CDT.CustomerId, model).Count();

            ViewBag.Customer = customer;

            var _stripeApiKey = SystemConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;
            var _stripePublishKey = SystemConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue2;

            ViewBag.stripeApiKey = _stripeApiKey;
            ViewBag.stripePublishKey = _stripePublishKey;

           
            var url = this.Request.Headers["HOST"];

         
                return View(SocailProfile);

        }

        public ActionResult startin(int socialProfileId)
        {

            ViewBag.socialProfileId = socialProfileId;
            ViewBag.CurrentUser = this.CDT;
            var SocailProfile = this._cm.GetSocialProfileById(socialProfileId);
            var customer = _customerManager.GetCustomerByCustomerId(this.CDT.CustomerId);
            //ProfilesSearchRequest model = new ProfilesSearchRequest { Block = 99, Plan = 0, searchString = "", SocialType = 0 };
            //ViewBag.ProfileCount = this._customerManager.GetSocialProfilesByCustomerid(this.CDT.CustomerId, model).Count();

            ViewBag.Customer = customer;
            if (customer.IsBroker.HasValue && customer.IsBroker.Value == true)
            {
                ViewBag.profiles = _cm.GetSocialProfilesByCustomerid(customer.CustomerId, new ProfilesSearchRequest() { Block = 0, Plan= 0, searchString = "", SocialType = 0 });
            }

            var _stripeApiKey = SystemConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;
            var _stripePublishKey = SystemConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue2;

            ViewBag.stripeApiKey = _stripeApiKey;
            ViewBag.stripePublishKey = _stripePublishKey;

            return View("startlinkedin",SocailProfile);

        }


        [HttpPost]
        public ActionResult Target(SocialProfileDTO request)
        {
            var customer = _customerManager.GetCustomerByCustomerId(this.CDT.CustomerId);
            var brokermode = customer.IsBroker.HasValue && customer.IsBroker.Value == true;

            this._cm.UpdateTargetProfile(request, brokermode);
            this._cm.UpdateTargetProfileWhiteList(request);
            this._cm.UpdateTargetProfileBlackList(request);
            return RedirectToAction("Target", "Profile", new { socialProfileId = request.SocialProfile_Instagram_TargetingInformation.SocialProfileId, success = 1 });
        }
        public ActionResult Stats(int socialProfileId)
        {
            ViewBag.IsTrialActive = _sessionManager.Get(SessionConstants.profileTrialActive);
            ViewBag.socialProfileId = socialProfileId;
            ViewBag.CurrentUser = this.CDT;
            ViewBag.socialProfile = this._cm.GetSocialProfileById(socialProfileId).SocialProfile;
            ProfilesSearchRequest model = new ProfilesSearchRequest { Block = 99, Plan = 0, searchString = "", SocialType = 0 };
            ViewBag.ProfileCount = this._customerManager.GetSocialProfilesByCustomerid(this.CDT.CustomerId, model).Count();

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
                if (mode == 3)
                {
                    startdate = DateTime.Today.AddMonths(-3);  //3 months
                }
                else if (mode == 4)
                {
                    startdate = DateTime.Today.AddMonths(-6); //6 months
                }
                else if (mode == 5)
                {
                    startdate = DateTime.Today.AddMonths(-12); //6 months
                }


                var trends = _statisticsManager.GetProfileTrends(socialProfileId, startdate, enddate);

                foreach (var item in trends)
                {
                    item.FollowersTotal = item.FollowersTotal.HasValue ? item.FollowersTotal : 0;
                    item.Followings = item.Followings.HasValue ? item.Followings : 0;
                    item.Like = item.Like.HasValue ? item.Like : 0;
                    item.Engagement = (item.FollowersTotal ?? 1) * 100 / ((item.Followings ?? 1) == 0 ? 1 : (item.Followings ?? 1));
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


        public ActionResult History(int socialProfileId, string buy = "", string buysession = "")
        {
            ViewBag.IsTrialActive = _sessionManager.Get(SessionConstants.profileTrialActive);
            ViewBag.socialProfileId = socialProfileId;
            ViewBag.CurrentUser = this.CDT;
            var SocailProfile = this._cm.GetSocialProfileById(socialProfileId);

            var customer = _customerManager.GetCustomerByCustomerId(this.CDT.CustomerId);
            ProfilesSearchRequest model = new ProfilesSearchRequest { Block = 99, Plan = 0, searchString = "", SocialType = 0 };
            ViewBag.ProfileCount = this._customerManager.GetSocialProfilesByCustomerid(this.CDT.CustomerId, model).Count();

            ViewBag.Customer = customer;

            ViewBag.Plans = _planManager.GetallIntagramPaymentPlans(customer.IsBroker.HasValue ? customer.IsBroker.Value : false);

            var _stripeApiKey = SystemConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;
            var _stripePublishKey = SystemConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue2;

            ViewBag.stripeApiKey = _stripeApiKey;
            ViewBag.stripePublishKey = _stripePublishKey;



            if (buysession != "")
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

        public ActionResult BuyPhone(string socialprofileid, string email, string pageUrl)
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


            return this.Content(session.Id);
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

                _customerManager.UpdateCustomerContactPhone(model.phone, model.socialProfileId);
                var socialProfile = _customerManager.GetSocialProfileById(model.socialProfileId);
                var newPlan = _planManager.GetPlanInformationById(model.selectedPlanId);


                var subscriptionService = new SubscriptionService();
                Subscription stripeSubscription = null;

                if (!string.IsNullOrWhiteSpace(socialProfile.SocialProfile.StripeCustomerId))
                {
                    if (!String.IsNullOrWhiteSpace(socialProfile.SocialProfile.StripeSubscriptionId))
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

                    //_cm.UpdateSocialProfileStripeCustomer(model.socialProfileId, socialProfile.SocialProfile.StripeCustomerId, stripeSubscription.Id, newPlan.PlanId);
                    if (newPlan.PlanId == 8)
                        _cm.UpdateSocialProfileStripeCustomer(model.socialProfileId, socialProfile.SocialProfile.StripeCustomerId, stripeSubscription.Id, newPlan.PlanId);
                    else if (newPlan.PlanId == 5)
                        _cm.UpdateCustomerStripeCustomer(model.customerid, socialProfile.SocialProfile.StripeCustomerId, stripeSubscription.Id, newPlan.PlanId);

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
                    if (newPlan.PlanId == 2)
                    {
                        stripeSubscriptionCreateOptions.TrialPeriodDays = 7;
                    }

                    stripeSubscriptionCreateOptions.AddExpand("latest_invoice.payment_intent");
                    stripeSubscription = subscriptionService.Create(stripeSubscriptionCreateOptions);

                    //-- Update customer stripe id async call not to wait.
                    if (newPlan.PlanId == 8)
                        _cm.UpdateSocialProfileStripeCustomer(model.socialProfileId, stripeCustomer.Id, stripeSubscription.Id, newPlan.PlanId);
                    else if (newPlan.PlanId == 5)
                        _cm.UpdateCustomerStripeCustomer(model.customerid, stripeCustomer.Id, stripeSubscription.Id, newPlan.PlanId);

                    _sessionManager.Set(SessionConstants.profileTrialActive, true);
                    _sessionManager.Set(SessionConstants.profileId, model.socialProfileId);

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


                    SocialProfile_PaymentsDTO paymentRec = new SocialProfile_PaymentsDTO
                    {
                        SocialProfileId = model.socialProfileId,
                        StripeSubscriptionId = stripeSubscription.Id,
                        Description = stripeSubscription.Plan.Nickname,
                        Name = stripeSubscription.Plan.Nickname,
                        Price = stripeSubscription.Plan.Amount / 100,
                        //-- subDTO.Price = stripeSubscription.Plan.Amount;
                        StripePlanId = newPlan.StripePlanId,
                        SubscriptionType = stripeSubscription.Plan.Interval,

                        StartDate = stripeSubscription.CurrentPeriodStart.Value,
                        EndDate = stripeSubscription.CurrentPeriodEnd.Value,
                        StatusId = (int)GlobalEnums.PlanSubscription.Active,
                        PaymentPlanId = newPlan.PlanId,
                        StripeInvoiceId = stripeSubscription.LatestInvoiceId,
                        PaymentDateTime = DateTime.Now
                    };


                    if (newPlan.PlanId == 5)
                    {
                        _customerManager.InsertCustomerPayment(paymentRec);
                    }
                    else if (newPlan.PlanId == 8)
                    {
                        _cm.InsertSocialProfilePayment(paymentRec);

                    }

                    if (newPlan.PlanId != 1)
                    {
                        //klaviyoAPI.Klaviyo_AddtoList(klaviyoProfile, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, Klavio_PayingCustomers);
                        //ev.Event = "Plan Upgrade";
                        var dynamicTemplateData1 = new Dictionary<string, string>
                        {
                            {"name",this.CDT.FirstName},
                            {"email", this.CDT.EmailAddress},
                            {"senddate", DateTime.Today.ToLongDateString()},
                            {"invoicedate", paymentRec.StartDate.ToString("dd MMMM yyyy")},
                            {"planname", paymentRec.Name},
                             {"price", "$" + paymentRec.Price},
                              {"total", "$" + paymentRec.Price},
                               {"username", socialProfile.SocialProfile.SocialUsername}

                        };
                        BAL.Managers.EmailManager.SendEmail(this.CDT.EmailAddress, this.CDT.FirstName, EmailManager.EmailType.PlanUpgrade, dynamicTemplateData1);

                        //send email to internal users.

                        dynamicTemplateData1 = new Dictionary<string, string>
                        {
                            {"fullname",this.CDT.FirstName},
                            {"name", "SPP Team"},
                            {"email", this.CDT.EmailAddress},
                            {"igusername", socialProfile.SocialProfile.SocialUsername}
                        };
                        BAL.Managers.EmailManager.SendEmail("info@connectwizz.net,muzamilw@hotmail.com,  omar.c@me.com, haaris@socialplannerpro.com,haarischaudhry@hotmail.co.uk", "Wizz Team", EmailManager.EmailType.CreditCardEntered, dynamicTemplateData1);

                    }
                    else
                    {
                        //klaviyoAPI.Klaviyo_DeleteFromList(this.CDT.EmailAddress, "https://a.klaviyo.com/api/v2/list", _klaviyoPublishKey, Klavio_PayingCustomers);
                        //ev.Event = "Plan Downgrade";
                    }



                    var dynamicTemplateData = new Dictionary<string, string>
                {
                    {"name",this.CDT.FirstName},
                    {"email", this.CDT.EmailAddress},
                    {"senddate", DateTime.Today.ToLongDateString()},
                    {"error", stripeSubscription.ToJson()},
                    {"socialprofileid", model.socialProfileId.ToString() },
                };
                    BAL.Managers.EmailManager.SendEmail("info@connectwizz.net", "Wizz", EmailManager.EmailType.info, dynamicTemplateData);


                    return this.Content(stripeSubscription.ToJson(), "application/json");
                    //jr.Data = new { ResultType = "Success", Message = "success" };

                }
                else
                {
                    var dynamicTemplateData = new Dictionary<string, string>
                    {
                        {"name",this.CDT.FirstName},
                        {"email", this.CDT.EmailAddress},
                        {"senddate", DateTime.Today.ToLongDateString()},
                        {"error", "subscription error, object not found"},
                        {"socialprofileid", model.socialProfileId.ToString() },
                    };
                    BAL.Managers.EmailManager.SendEmail("info@connectwizz.net", "Wizz", EmailManager.EmailType.error, dynamicTemplateData);
                    //return this.Content("subscription error, object not found.");
                    return this.Content("{'StripeMessage': 'subscription error, object not found'}");
                }


            }
            catch (StripeException e)
            {
                var dynamicTemplateData = new Dictionary<string, string>
                {
                    {"name",this.CDT.FirstName},
                    {"email", this.CDT.EmailAddress},
                    {"senddate", DateTime.Today.ToLongDateString()},
                    {"error", e.ToString()},
                    {"socialprofileid", model.socialProfileId.ToString() },
                };
                BAL.Managers.EmailManager.SendEmail("info@connectwizz.net", "ConnectWizz", EmailManager.EmailType.error, dynamicTemplateData);


                switch (e.StripeError.ErrorType)
                {
                    case "card_error":
                        Console.WriteLine("Code: " + e.StripeError.Code);
                        Console.WriteLine("Message: " + e.StripeError.Message);
                        return this.Content(@"{""StripeMessage"": "" " + e.StripeError.Message + " \" } ");
                        break;
                    case "api_connection_error":
                        return this.Content("{'StripeMessage': '" + e.StripeError.Message + "'}");
                        break;
                    case "api_error":
                        return this.Content("{'StripeMessage': '" + e.StripeError.Message + "'}");
                        break;
                    case "authentication_error":
                        return this.Content("{'StripeMessage': '" + e.StripeError.Message + "'}");
                        break;
                    case "invalid_request_error":
                        return this.Content("{'StripeMessage': '" + e.StripeError.Message + "'}");
                        break;
                    case "rate_limit_error":
                        return this.Content("{'StripeMessage': '" + e.StripeError.Message + "'}");
                        break;
                    case "validation_error":
                        return this.Content("{'StripeMessage': '" + e.StripeError.Message + "'}");
                        break;
                    default:
                        // Unknown Error Type
                        return this.Content("{'StripeMessage': 'unknown error'}");
                        break;
                }
            }
            catch (Exception ex)
            {
                //return this.Content(ex.ToString());
                var dynamicTemplateData = new Dictionary<string, string>
                {
                    {"name",this.CDT.FirstName},
                    {"email", this.CDT.EmailAddress},
                    {"senddate", DateTime.Today.ToLongDateString()},
                    {"error", ex.ToString()},
                    {"socialprofileid", model.socialProfileId.ToString() },
                };
                BAL.Managers.EmailManager.SendEmail("info@connectwizz.net", "ConnectWizz", EmailManager.EmailType.error, dynamicTemplateData);

                return this.Content("{'StripeMessage': '" + ex.ToString() + "'}");
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








        [HttpPost]
        public ActionResult Confirmdelete(int SocialProfileId)
        {
            var jr = new JsonResult();
            try
            {

         

                SocialProfileDTO profileDTO = _cm.GetSocialProfileById(SocialProfileId);

                var _stripeApiKey = SystemConfig.GetConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;
                if (profileDTO != null)
                {

                    try
                    {

                    
                        if (!string.IsNullOrEmpty(profileDTO.SocialProfile.StripeSubscriptionId))
                        {
                            StripeConfiguration.SetApiKey(_stripeApiKey);
                            var service = new SubscriptionService();
                            var sub = service.Get(profileDTO.SocialProfile.StripeSubscriptionId);

                            var subscription = service.Cancel(sub.Id, null);

                            Task.Run(() =>
                            {

                                var dynamicTemplateData = new Dictionary<string, string>
                                    {
                                        {"name",this.CDT.FirstName},
                                        {"email", this.CDT.EmailAddress},
                                        {"senddate", DateTime.Today.ToLongDateString()},
                                        {"planname", profileDTO.CurrentPaymentPlan.PlanName},
                                         {"socialusername", "$" + profileDTO.SocialProfile.SocialUsername}


                                    };
                                BAL.Managers.EmailManager.SendEmail(this.CDT.EmailAddress, this.CDT.FirstName, EmailManager.EmailType.profileDeleted, dynamicTemplateData);


                            });

                            jr.Data = new { ResultType = "Success", message = "User has successfully Unsubscribe." };
                        }
                        else
                        {
                            jr.Data = new { ResultType = "Error", message = "No active subscription available." };

                        }
                    }
                    catch (Exception)
                    {

                        
                    }

                    try
                    {

                   

                    if (!string.IsNullOrEmpty(profileDTO.socialcustomer.StripeSubscriptionId))
                    {
                        StripeConfiguration.SetApiKey(_stripeApiKey);
                        var service = new SubscriptionService();
                        var sub = service.Get(profileDTO.socialcustomer.StripeSubscriptionId);

                        var subscription = service.Cancel(sub.Id, null);

                        Task.Run(() =>
                        {

                            var dynamicTemplateData = new Dictionary<string, string>
                                    {
                                        {"name",this.CDT.FirstName},
                                        {"email", this.CDT.EmailAddress},
                                        {"senddate", DateTime.Today.ToLongDateString()},
                                        {"planname", profileDTO.CurrentPaymentPlan.PlanName},
                                         {"socialusername", "$" + profileDTO.SocialProfile.SocialUsername}


                                    };
                            BAL.Managers.EmailManager.SendEmail(this.CDT.EmailAddress, this.CDT.FirstName, EmailManager.EmailType.profileDeleted, dynamicTemplateData);


                        });

                        jr.Data = new { ResultType = "Success", message = "User has successfully Unsubscribe." };
                    }
                    else
                    {
                        jr.Data = new { ResultType = "Error", message = "No active subscription available." };

                    }

                    }
                    catch (Exception)
                    {

                        
                    }

                }

                _customerManager.DeleteCustomer(this.CDT.CustomerId, 0);
                jr.Data = new { ResultType = "Error", message = "User has successfully deleted." };

                /*if ((profileDTO.socialcustomer.IsBroker ?? false) == false)
                {
                    _customerManager.DeleteCustomer(this.CDT.CustomerId, SocialProfileId);
                }
                else 
                { 
                    if (_customerManager.DeleteProfile(this.CDT.CustomerId, SocialProfileId))
                    {
                        {
                            jr.Data = new { ResultType = "Error", message = "User has successfully deleted." };
                        }
                    }
                }*/
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
            ViewBag.socialProfileDTO = spDto;
            ViewBag.socialProfileFollowedAccounts = spDto.SocialProfile_FollowedAccounts;
            ProfilesSearchRequest model = new ProfilesSearchRequest { Block = 99, Plan = 0, searchString = "", SocialType = 0 };
            ViewBag.ProfileCount = this._customerManager.GetSocialProfilesByCustomerid(this.CDT.CustomerId, model).Count();
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
            ProfilesSearchRequest model = new ProfilesSearchRequest { Block = 99, Plan = 0, searchString = "", SocialType = 0 };
            ViewBag.ProfileCount = this._customerManager.GetSocialProfilesByCustomerid(this.CDT.CustomerId, model).Count();

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
        [AllowAnonymous]
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

        [HttpPost]
        public ActionResult SaveUpdateUserDataIndividually(string value, string pk, int socialProfileId, int TargetingInformationId)
        {
            try
            {
                    var User = _customerManager.SaveUpdateProfileDataIndividually(value, pk, socialProfileId, TargetingInformationId);

                
                
                //return _customerManager.IsEmailExist(EmailAddress, id) ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
                return Json(User, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        


    }
}