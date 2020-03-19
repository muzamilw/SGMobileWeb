using SG2.CORE.BAL.Managers;
using System;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using PagedList;
using SG2.CORE.MODAL.ViewModals.Backend;
using SG2.CORE.COMMON;
using SG2.CORE.MODAL.DTO.TargetPreferences;
using SG2.CORE.WEB.App_Start;
using System.Web;
using SG2.CORE.WEB.Architecture;
using Stripe;
using SG2.CORE.MODAL.DTO.Customers;
using System.Collections.Generic;
using SG2.CORE.MODAL.DTO.SystemSettings;
using SG2.CORE.MODAL.ViewModals.Customers;
using SG2.CORE.MODAL.ViewModals.CRM;

namespace SG2.CORE.WEB.Areas.SuperAdmin.Controllers
{
    [SuperAdminAuthorizeFilters]
    public class CRMController : BaseController
    {
        protected readonly CustomerManager _customerManager;
        protected readonly CommonManager _commonManager;
        private readonly string _PageSize = string.Empty;
        //protected readonly TargetPreferencesManager _targetPreferenceManager;
        protected readonly List<SystemSettingsDTO> SystemConfigs;
        protected readonly PlanInformationManager _planmanager;
        protected readonly StatisticsManager _statisticsManager;

        public CRMController()
        {
            _customerManager = new CustomerManager();
            _commonManager = new CommonManager();
            _PageSize = WebConfigurationManager.AppSettings["PageSize"];
            //_targetPreferenceManager = new TargetPreferencesManager();
            ViewBag.SetMenuActiveClass = "CRM";
            SystemConfigs = SystemConfig.GetConfigs;
            _planmanager = new PlanInformationManager();
            _statisticsManager = new StatisticsManager();
        }

        // GET: SuperAdmin/CRM
        public ActionResult Index(int page = 1, string SearchCriteria = "", int? StatusId = null, string ProductId = null, string JVStatus = null, int? Subscription=null, int ? profileType = null, int? BlockStatus = null, int? AppConnStatus = null)
        {
            try
            {
                var customers = _customerManager.GetUserData(SearchCriteria, _PageSize, page, StatusId, ProductId, JVStatus, Subscription, profileType, BlockStatus, AppConnStatus);
                var ProductIds = _customerManager.GetProductIds();
                var totatRecord = customers.FirstOrDefault()?.TotalRecord ?? 0;
                IPagedList<CustomerListingViewModel> pageOrders = new StaticPagedList<CustomerListingViewModel>(customers, page, Convert.ToInt32(_PageSize), totatRecord);
                var PageSize = pageOrders.PageCount;
                var PageNumber = pageOrders.PageNumber;
                var model = new CustomerIndexViewModel()
                {
                    CustomerListing = pageOrders,
                    ApplicationStatuses = this.ApplicationStatuses,
                    ProductIds = ProductIds,
                    TotalRecord = PageSize,
                    PageNumber = PageNumber,
                    SearchCriteria = SearchCriteria,
                    ProductId = ProductId,
                    StatusId = StatusId,
                    BlockStatus = BlockStatus


                };
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult CustomerOrderHistory(string id, string SPId, int page = 1)
        {

            try
            {
                // var Cus = HttpUtility.UrlDecode(id);
                //int custId = Convert.ToInt32(CryptoEngine.Decrypt(customerId.ToString()));
                int custId = Convert.ToInt32((CryptoEngine.Decrypt(id)));
                int SocialPId = Convert.ToInt32((CryptoEngine.Decrypt(SPId)));
                ViewBag.SocailProfile = this._customerManager.GetSocialProfileById(SocialPId);
                var COH = _customerManager.GetCustomerOrderHistory(_PageSize, page, custId, SocialPId);
                var totatRecord = COH.FirstOrDefault()?.TotalRecords ?? 0;

                IPagedList<CustomerOrderHistoryViewModel> pageOrders = new StaticPagedList<CustomerOrderHistoryViewModel>(COH, page, Convert.ToInt32(_PageSize), totatRecord);
                CustomerOrderHistoryIndexViewModel customerOrderHistoryIndexView = new CustomerOrderHistoryIndexViewModel();
                customerOrderHistoryIndexView.CustomerOrderHListing = pageOrders;
                customerOrderHistoryIndexView.TotalRecord = pageOrders.PageCount;
                customerOrderHistoryIndexView.PageNumber = pageOrders.PageNumber;
                customerOrderHistoryIndexView.Id = Convert.ToString(custId);
                customerOrderHistoryIndexView.SocialProfileId = Convert.ToString(SocialPId);
                customerOrderHistoryIndexView.SPName = COH.FirstOrDefault()?.SPName ?? "";
                customerOrderHistoryIndexView.JVStatus = COH.FirstOrDefault()?.JVStatus ?? "";
                customerOrderHistoryIndexView.Email = COH.FirstOrDefault()?.Email ?? "";
                customerOrderHistoryIndexView.SPStatus = COH.FirstOrDefault()?.SPStatus ?? "";
                customerOrderHistoryIndexView.SusrName = COH.FirstOrDefault()?.SusrName ?? "";
                customerOrderHistoryIndexView.UserName = COH.FirstOrDefault()?.UserName ?? "";
                return View(customerOrderHistoryIndexView);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult DetailPage(string id, string SPId)
        {

            if (!string.IsNullOrEmpty((string)TempData["Success"]))
            {
                ViewBag.Success = (string)TempData["Success"];
                ViewBag.Message = TempData["Message"];
            }



            //int custId = Convert.ToInt32(CryptoEngine.Decrypt(id));
            int custId1 = Convert.ToInt32(HttpUtility.UrlDecode(CryptoEngine.Decrypt(id)));
            int socialProfileId = Convert.ToInt32(HttpUtility.UrlDecode(CryptoEngine.Decrypt(SPId)));
            var model = _customerManager.GetSpecificUserData(custId1, socialProfileId);
            ViewBag.SocailProfile = this._customerManager.GetSocialProfileById(socialProfileId);
            //model.Countries = CommonManager.GetCountries();
            if (!string.IsNullOrEmpty(model.Country))
            {
                model.cities = CommonManager.GetCities().Where(m => m.CountryId == Convert.ToInt16(model.Country)).ToList();
            }
            else
            {
                model.cities = CommonManager.GetCities();
            }
            model.usersList = CommonManager.GetTeamMembers();
            model.CustomerTitles = CommonManager.GetTitle();
            model.statusDTOs = this.ApplicationStatuses;
            model.Comment = string.Empty;

            return View(model);
        }

        [HttpPost]
        public ActionResult SaveUpdateUserDataIndividually(string value, string fieldName, string customerId, string SocialProfileId)
        {
            try
            {
                if (!string.IsNullOrEmpty(customerId))
                {
                    if (fieldName == "Comment")
                    {
                        value = this.CDT.UserName + ":" + value;
                    }

                    var Cus = HttpUtility.UrlDecode(customerId);
                    var SPId = HttpUtility.UrlDecode(SocialProfileId);
                    //int custId = Convert.ToInt32(CryptoEngine.Decrypt(customerId.ToString()));
                    int custId = Convert.ToInt32((CryptoEngine.Decrypt(Cus)));
                    int socialPrpfileId = Convert.ToInt32((CryptoEngine.Decrypt(SPId)));
                    var User = _customerManager.SaveUpdateUserDataIndividually(value, fieldName, custId, socialPrpfileId);
                }
                //return _customerManager.IsEmailExist(EmailAddress, id) ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
                return Json(User, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public ActionResult ScheduleCall(string customerId, string schedule, string notes)
        {
            try
            {
                var jr = new JsonResult();
                if (!string.IsNullOrEmpty(customerId))
                {
                    var Cus = HttpUtility.UrlDecode(customerId);
                    //int custId = Convert.ToInt32(CryptoEngine.Decrypt(customerId.ToString()));
                    int custId = Convert.ToInt32((CryptoEngine.Decrypt(Cus)));

                    var User = _customerManager.ScheduleCall(custId,Convert.ToDateTime(  schedule), notes);
                    if (User)
                    {

                        jr.Data = new { ResultType = "Success", message = "Call Schedule Successfully." };
                    }
                    else
                    {
                        jr.Data = new { ResultType = "Error", message = "Error." };

                    }
                    return Json(jr, JsonRequestBehavior.AllowGet);

                }
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public ActionResult AssignJVBoxToCustomer(string customerId, string profileId)
        {
            try
            {
                var jr = new JsonResult();
                if (!string.IsNullOrEmpty(customerId))
                {
                    var Cus = HttpUtility.UrlDecode(customerId);
                    var SPId = HttpUtility.UrlDecode(profileId);
                    //int custId = Convert.ToInt32(CryptoEngine.Decrypt(customerId.ToString()));
                    int custId = Convert.ToInt32((CryptoEngine.Decrypt(Cus)));
                    int socialProfileId = Convert.ToInt32((CryptoEngine.Decrypt(SPId)));
                    //var User = _customerManager.AssignJVBoxToCustomer(custId, socialProfileId);
                    //if (User != null)
                    //{
                    //    jr.Data = new { ResultType = "Success", message = "JV Box assigned successfully.", User };
                    //}

                    //else
                    //{
                    //    string messages = "Something went wrong";
                    //    jr.Data = new { ResultType = "Error", message = messages };
                    //}

                }
                return Json(jr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult DeleteCustomerProfileSingle(string customerId, string profileId)
        {
            var jr = new JsonResult();
            string messages = string.Empty;
            try
            {

                if (!string.IsNullOrEmpty(customerId))
                {
                    var Cus = HttpUtility.UrlDecode(customerId);
                    var SPId = HttpUtility.UrlDecode(profileId);
                    //int custId = Convert.ToInt32(CryptoEngine.Decrypt(customerId.ToString()));
                    int custId = Convert.ToInt32((CryptoEngine.Decrypt(Cus)));
                    int socialProfileId = Convert.ToInt32((CryptoEngine.Decrypt(SPId)));
                    var User = _customerManager.DeleteCustomerAll(custId, socialProfileId);
                    if (User)
                    {
                        jr.Data = new { ResultType = "Success", message = "Customer deleted successfully.", User };
                    }

                    else
                    {
                        messages = "Something went wrong";
                        jr.Data = new { ResultType = "Error", message = messages };
                    }

                }

            }
            catch (Exception ex)
            {

                messages = ex.Message;
            }
            return Json(jr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCustomerProfile(string customerId, string profileId)
        {
            var jr = new JsonResult();
            try
            {

                var SPId = HttpUtility.UrlDecode(profileId);
                int socialProfileId = Convert.ToInt32((CryptoEngine.Decrypt(SPId)));

                SocialProfileDTO profileDTO = _customerManager.GetSocialProfileById(socialProfileId);
           
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

                            var subscription = service.Cancel(sub.Id, null);

                          

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
               
                if (_customerManager.DeleteProfile(0, socialProfileId))
                {
                    {
                        jr.Data = new { ResultType = "Success", message = "Profile has been successfully deleted." };
                    }
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return Json(jr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCustomerProfileJV(string customerId, string profileId)
        {
            var jr = new JsonResult();
            string messages = string.Empty;
            try
            {

                if (!string.IsNullOrEmpty(customerId))
                {
                    var Cus = HttpUtility.UrlDecode(customerId);
                    var SPId = HttpUtility.UrlDecode(profileId);
                    //int custId = Convert.ToInt32(CryptoEngine.Decrypt(customerId.ToString()));
                    int custId = Convert.ToInt32((CryptoEngine.Decrypt(Cus)));
                    int socialProfileId = Convert.ToInt32((CryptoEngine.Decrypt(SPId)));
                    var User = false;
                    if (User)
                    {
                        jr.Data = new { ResultType = "Success", message = "Customer deleted successfully.", User };
                    }

                    else
                    {
                        messages = "Something went wrong";
                        jr.Data = new { ResultType = "Error", message = messages };
                    }

                }

            }
            catch (Exception ex)
            {

                messages = ex.Message;
            }
            return Json(jr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TargettingInformation(string id, string SPId, int? success)
        {



            int custId = Convert.ToInt32(CryptoEngine.Decrypt(id));
            int socialProfileId = Convert.ToInt32(CryptoEngine.Decrypt(SPId));


            ViewBag.socialProfileId = socialProfileId;
            
            var SocailProfile = this._customerManager.GetSocialProfileById(socialProfileId);
            ViewBag.SocailProfile = SocailProfile;
            var customer = _customerManager.GetCustomerByCustomerId(SocailProfile.SocialProfile.CustomerId.Value);
            ViewBag.CurrentUser = customer;

            //ViewBag.Plans = _planmanager.GetallIntagramPaymentPlans(customer.IsBroker.HasValue ? customer.IsBroker.Value : false);

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


            return View(SocailProfile);

        }


        [HttpPost]
        public ActionResult TargettingInformation(SocialProfileDTO request)
        {

            this._customerManager.UpdateTargetProfile(request);
            //return RedirectToAction("targettinginformation", "crm", new { id = Url.Encode(CryptoEngine.Encrypt(Convert.ToString(request.SocialProfile.CustomerId))), SPId= Url.Encode(CryptoEngine.Encrypt(request.SocialProfile.SocialProfileId.ToString())), success = 1 });
            return Redirect("/sadmin/crm/targettinginformation?id=" + Url.Encode(CryptoEngine.Encrypt(Convert.ToString(request.SocialProfile.CustomerId))) + "&SPId=" + Url.Encode(CryptoEngine.Encrypt(request.SocialProfile_Instagram_TargetingInformation.SocialProfileId.ToString())) + "&success=1");
        }


        public ActionResult Lists(string id, string SPId, int? success)
        {

            int custId = Convert.ToInt32(CryptoEngine.Decrypt(id));
            int socialProfileId = Convert.ToInt32(CryptoEngine.Decrypt(SPId));


            ViewBag.socialProfileId = socialProfileId;

            var SocailProfile = this._customerManager.GetSocialProfileById(socialProfileId);
            ViewBag.SocailProfile = SocailProfile;

            ViewBag.socialProfileId = socialProfileId;
            ViewBag.CurrentUser = this.CDT;
           

            if (success.HasValue && success.Value == 1)
            {
                ViewBag.success = 1;
            }

            return View(SocailProfile);

        }


        [HttpPost]
        public ActionResult Lists(SocialProfileDTO request)
        {
            this._customerManager.UpdateTargetProfileLists(request);
            return Redirect("/sadmin/crm/lists?id=" + Url.Encode(CryptoEngine.Encrypt(Convert.ToString(0))) + "&SPId=" + Url.Encode(CryptoEngine.Encrypt(request.SocialProfile_Instagram_TargetingInformation.SocialProfileId.ToString())) + "&success=1");
        }


        public ActionResult Stats(string id, string SPId)
        {
            int custId = Convert.ToInt32(CryptoEngine.Decrypt(id));
            int socialProfileId = Convert.ToInt32(CryptoEngine.Decrypt(SPId));

            ViewBag.socialProfileId = socialProfileId;
            ViewBag.CurrentUser = this.CDT;
            ViewBag.socialProfile = this._customerManager.GetSocialProfileById(socialProfileId);

            ViewBag.actions = this._customerManager.ReturnLastActions(socialProfileId, 500);


            return View(this._statisticsManager.GetStatistics(socialProfileId));


        }

        public ActionResult FollowList(string id, string SPId)
        {
            int custId = Convert.ToInt32(CryptoEngine.Decrypt(id));
            int socialProfileId = Convert.ToInt32(CryptoEngine.Decrypt(SPId));

            ViewBag.socialProfileId = socialProfileId;
            ViewBag.CurrentUser = this.CDT;
            ViewBag.socialProfile = this._customerManager.GetSocialProfileById(socialProfileId);


            return View(new FollowListViewModel());


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

            ViewBag.socialProfile = socialProfileDTO;


            return View(followListViewModel);


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
                    item.Engagement = (item.FollowersTotal ?? 1) * 100 / ((item.Followings ??1) == 0 ? 1: item.Followings);
                    item.Unfollow = (item.Unfollow ?? 0);
                    item.StoryViews = (item.StoryViews ?? 0);
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
                            Unfollow = trends.Select(x => x.Unfollow.ToString()).ToArray()


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

        public ActionResult GlobalTarget(int? success)
        {


            int socialProfileId = -999;


            ViewBag.socialProfileId = socialProfileId;

            var SocailProfile = this._customerManager.GetSocialProfileById(socialProfileId);

            //var customer = _customerManager.GetCustomerByCustomerId(SocailProfile.SocialProfile.CustomerId.Value);
            //ViewBag.CurrentUser = customer;

            //ViewBag.Plans = _planmanager.GetallIntagramPaymentPlans(customer.IsBroker.HasValue ? customer.IsBroker.Value : false);

            if (success.HasValue && success.Value == 1)
            {
                ViewBag.success = 1;
            }

            var _stripeApiKey = SystemConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;
            var _stripePublishKey = SystemConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue2;

            ViewBag.stripeApiKey = _stripeApiKey;
            ViewBag.stripePublishKey = _stripePublishKey;
            


            return View(SocailProfile);

        }

        [HttpPost]
        public ActionResult GlobalTarget(SocialProfileDTO request)
        {

            this._customerManager.UpdateTargetProfile(request);
            return RedirectToAction("GlobalTarget", "CRM", new {success = 1 });
        }

        public ActionResult UserDetail()
        {
            return View();
        }

        public ActionResult TargetPrefrences()
        {
            return View();
        }

        public ActionResult AddFreeDaysToCustomer(string profileId, int addFreeDays)
        {

            var _stripeApiKey = SystemConfig.GetConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;
            StripeConfiguration.SetApiKey(_stripeApiKey);
            var subscriptionService = new SubscriptionService();
            Subscription stripeSubscription = null;
            var jr = new JsonResult();
            string messages = string.Empty;
            try
            {

                if (!string.IsNullOrEmpty(profileId))
                {
                    var pro = HttpUtility.UrlDecode(profileId);

                    //int custId = Convert.ToInt32(CryptoEngine.Decrypt(customerId.ToString()));
                    int proId = Convert.ToInt32((CryptoEngine.Decrypt(pro)));
                    SocialProfileDTO profileDTO = _customerManager.GetSocialProfileById(proId);

                    if (profileDTO != null)
                    {

                        Subscription subscriptionItemUpdate = subscriptionService.Get(profileDTO.SocialProfile.StripeSubscriptionId);


                        var items = new List<SubscriptionItemUpdateOption> {
                                        new SubscriptionItemUpdateOption {
                                        Id= subscriptionItemUpdate.Items.Data[0].Id,
                                        Plan = subscriptionItemUpdate.Plan.Id,
                                        Quantity= 1,

                                       },
                                    };

                        var subscriptionUpdateoptions = new SubscriptionUpdateOptions
                        {
                            Items = items,
                            //Billing = Billing.ChargeAutomatically,

                            //BillingThresholds = { },
                            Prorate = false,
                            //BillingCycleAnchorUnchanged = false,
                            TrialEnd = subscriptionItemUpdate.CurrentPeriodEnd.Value.AddDays(addFreeDays),
                            //EndTrialNow = true,

                        };
                        stripeSubscription = subscriptionService.Update(profileDTO.SocialProfile.StripeSubscriptionId, subscriptionUpdateoptions);
                        jr.Data = new { ResultType = "Success", message = "Free days successfully added to customer Current subscription." };
                    }

                    else
                    {
                        messages = "Something went wrong";
                        jr.Data = new { ResultType = "Error", message = messages };
                    }

                }

            }
            catch (Exception ex)
            {

                messages = ex.Message;
            }
            return Json(jr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RefundToCustomer(string customerId,string profileId)
        {

            var _stripeApiKey = SystemConfig.GetConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;
            StripeConfiguration.SetApiKey(_stripeApiKey);
            var subscriptionService = new SubscriptionService();
            var jr = new JsonResult();
            string messages = string.Empty;
            try
            {

                if (!string.IsNullOrEmpty(profileId))
                {
                    var pro = HttpUtility.UrlDecode(profileId);

                    //int custId = Convert.ToInt32(CryptoEngine.Decrypt(customerId.ToString()));
                    int proId = Convert.ToInt32((CryptoEngine.Decrypt(pro)));
                    SocialProfileDTO profileDTO = _customerManager.GetSocialProfileById(proId);

                    if (profileDTO != null)
                    {
                       //not cancelling the subscription.
                        //if (profileDTO != null)
                        //{
                        //    if (profileDTO.SocialProfile .StatusId != 18)
                        //    {
                        //        if (!string.IsNullOrEmpty(profileDTO.SocialProfile.StripeSubscriptionId))
                        //        {
                        //            StripeConfiguration.SetApiKey(_stripeApiKey);
                        //            var service = new SubscriptionService();
                        //            var subscription = service.Cancel(profileDTO.SocialProfile.StripeSubscriptionId, null);
                        //        }
                        //        else
                        //        {
                        //            SocialProfile_PaymentsDTO cancelledSub = new SocialProfile_PaymentsDTO();
                        //            cancelledSub = _customerManager.GetLastCancelledSubscription(profileDTO.SocialProfile .SocialProfileId, DateTime.Now);
                        //            profileDTO.SocialProfile.StripeSubscriptionId = cancelledSub.StripeSubscriptionId;
                        //            //profileDTO.StripeInvoiceId = cancelledSub.StripeInvoiceId;

                        //        }

                        //    }
                        //}

                        Subscription subscriptionItemUpdate = subscriptionService.Get(profileDTO.SocialProfile.StripeSubscriptionId);
                        var invoiceService = new InvoiceService();
                        var invoice = invoiceService.Get(profileDTO.LastSocialProfile_Payments.First().StripeInvoiceId);
                        var chargeService = new ChargeService();
                        var charge = chargeService.Get(invoice.ChargeId);
                        var refundOptions = new RefundCreateOptions
                        {
                            Amount = charge.Amount,
                            Reason = RefundReasons.RequestedByCustomer,
                            Charge = charge.Id
                        };
                        var refundService = new RefundService();
                        Refund refund = refundService.Create(refundOptions);

                        if (refund.Status == "succeeded")
                        {


                            if (!string.IsNullOrEmpty(customerId))
                            {

                                string value = this.CDT.UserName + " (" + DateTime.Now.ToString("dd MMM, yyyy HH:mm") + "): " + " Amount " + refund.Amount + " refunded successfully.";


                                var Cus = HttpUtility.UrlDecode(customerId);

                                //int custId = Convert.ToInt32(CryptoEngine.Decrypt(customerId.ToString()));
                                int custId = Convert.ToInt32((CryptoEngine.Decrypt(Cus)));
                                var User = _customerManager.SaveUpdateUserDataIndividually(value, "Comment", custId, profileDTO.SocialProfile. SocialProfileId);


                            }

                            jr.Data = new { ResultType = "Success", message = "Amount refunded successfully." };
                        }
                        else
                        {
                            messages = "Something went wrong";
                            jr.Data = new { ResultType = "Error", message = messages };
                        }

                    }

                    else
                    {
                        messages = "Something went wrong";
                        jr.Data = new { ResultType = "Error", message = messages };
                    }

                }

            }
            catch (Exception ex)
            {

                messages = ex.Message;
                jr.Data = new { ResultType = "Error", message = messages };
            }
            return Json(jr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCityByCountryId(short countryId)
        {
            var Cities = CommonManager.GetCities().Where(m => m.CountryId == countryId).ToList();
            return Json(Cities, JsonRequestBehavior.AllowGet);
        }

    }
}