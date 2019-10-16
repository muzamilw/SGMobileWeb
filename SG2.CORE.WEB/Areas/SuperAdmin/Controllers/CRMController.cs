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


        public CRMController()
        {
            _customerManager = new CustomerManager();
            _commonManager = new CommonManager();
            _PageSize = WebConfigurationManager.AppSettings["PageSize"];
            //_targetPreferenceManager = new TargetPreferencesManager();
            ViewBag.SetMenuActiveClass = "CRM";
            SystemConfigs = SystemConfig.GetConfigs;

        }

        // GET: SuperAdmin/CRM
        public ActionResult Index(int page = 1, string SearchCriteria = "", int? StatusId = null, string ProductId = null, string JVStatus = null, int? Subscription=null)
        {
            try
            {
                var customers = _customerManager.GetUserData(SearchCriteria, _PageSize, page, StatusId, ProductId, JVStatus, Subscription);
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
                    StatusId = StatusId
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

        public ActionResult ScheduleCall(string customerId, DateTime schedule, string notes)
        {
            try
            {
                var jr = new JsonResult();
                if (!string.IsNullOrEmpty(customerId))
                {
                    var Cus = HttpUtility.UrlDecode(customerId);
                    //int custId = Convert.ToInt32(CryptoEngine.Decrypt(customerId.ToString()));
                    int custId = Convert.ToInt32((CryptoEngine.Decrypt(Cus)));

                    var User = _customerManager.ScheduleCall(custId, schedule, notes);
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
                    var User = _customerManager.DeleteCustomerAll(custId, 0);
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

        public ActionResult TargettingInformation(string id, string SPId)
        {
            int custId = Convert.ToInt32(CryptoEngine.Decrypt(id));
            int SocialPId = Convert.ToInt32(CryptoEngine.Decrypt(SPId));
            var model = _customerManager.GetSpecificUserTargettingInformation(custId, SocialPId);

            if (model == null)
            {

                CustomerTargetPreferencesViewModel model1 = new CustomerTargetPreferencesViewModel();
                model1.SPId = SocialPId.ToString();
                model1.Id = custId.ToString();
                model1.Countries = CommonManager.GetCountries();
                if (model1.Country != null)
                {
                    model1.Cities = CommonManager.GetCities().Where(m => m.CountryId == Convert.ToInt16(model.Country)).ToList();

                }
                else
                {
                    model1.Cities = CommonManager.GetCities();
                }
                model1.ProxyIPs = _customerManager.GetProxyIPs(model.Country ?? 0, model.City ?? 0);
                model1.JarveeStatuses = this.ApplicationStatuses;
                model.MPBoxList = _customerManager.GetMPBoxes();
                return View(model1);
            }
            else
            {
                model.Countries = CommonManager.GetCountries();
                if (model.Country != null)
                {
                    model.Cities = CommonManager.GetCities().Where(m => m.CountryId == Convert.ToInt16(model.Country)).ToList();
                }
                else
                {
                    model.Cities = CommonManager.GetCities();
                }
                model.ProxyIPs = _customerManager.GetProxyIPs(model.Country ?? 0, model.City ?? 0);
                model.JarveeStatuses = this.ApplicationStatuses;
                model.MPBoxList = _customerManager.GetMPBoxes();
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult UpdateTargettingInformation(CustomerTargetPreferencesViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dl = _targetPreferenceManager.SaveTargetPreferences(new TargetPreferencesDTO()
                    {
                        Preference1 = model.Preference1,
                        Preference2 = model.Preference2,
                        Preference3 = model.Preference3,
                        Preference4 = model.Preference4,
                        Preference5 = model.Preference5,
                        Preference6 = model.Preference6,
                        Preference7 = model.Preference7,
                        Preference8 = model.Preference8,
                        Preference9 = model.Preference9,
                        Preference10 = model.Preference10,
                        //Country = model.Country,
                        //City = model.City,
                        //InstaUser = model.InstaUser,
                        //InstaPassword = model.InstaPassword,
                        SocialProfileId = Convert.ToInt32(model.SPId),
                        Id = Convert.ToInt32(model.Id),
                        SocialAccAs = model.SocialAccAS
                    });

                    TempData["Success"] = "Yes";
                    TempData["Message"] = "Profile updated successfully.";
                    //return RedirectToAction("TargettingInformation", "CRM", new { @id = CryptoEngine.Encrypt(Convert.ToString(dl.Id)), @SPId= model.SPId });
                    return Redirect("/sadmin/crm/targettinginformation?id=" + Url.Encode(CryptoEngine.Encrypt(Convert.ToString(dl.Id))) + "&SPId=" + Url.Encode(CryptoEngine.Encrypt(model.SPId)));
                }
                else
                {
                    string messages = string.Join(", ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    TempData["Success"] = "False";
                    TempData["Message"] = messages;
                    return RedirectToAction("TargettingInformation");
                }
            }
            catch (Exception)
            {

                throw;
            }
            //int custId = Convert.ToInt32(CryptoEngine.Decrypt(id));
            //var model = _customerManager.GetSpecificUserTargettingInformation(custId);
            //return View(Model);

        }

        [HttpPost]
        public ActionResult UpdateTarget(CustomerTargetPreferencesViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int? CityId = model.City;
                    int? CountryId = model.Country;
                    var dl = _targetPreferenceManager.SaveSocialProfileData(
                        model.InstaUser,
                        model.InstaPassword,
                        CityId ?? 0,
                        CountryId ?? 0,
                        Convert.ToInt32(model.SPId),
                        Convert.ToInt32(model.Status)
                    );
                    return Redirect("/sadmin/crm/targettinginformation?id=" + Url.Encode(CryptoEngine.Encrypt(model.Id)) + "&SPId=" + Url.Encode(CryptoEngine.Encrypt(model.SPId)));
                }
                else
                {
                    string messages = string.Join(", ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    TempData["Success"] = "False";
                    TempData["Message"] = messages;
                    return Redirect("/sadmin/crm/targettinginformation?id=" + Url.Encode(CryptoEngine.Encrypt(model.Id)) + "&SPId=" + Url.Encode(CryptoEngine.Encrypt(model.SPId)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //int custId = Convert.ToInt32(CryptoEngine.Decrypt(id));
            //var model = _customerManager.GetSpecificUserTargettingInformation(custId);
            //return View(Model);

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
                    CustomerTargetProfileDTO profileDTO = _customerManager.GetSocialProfilesById(proId);

                    if (profileDTO != null)
                    {

                        Subscription subscriptionItemUpdate = subscriptionService.Get(profileDTO.StripeSubscriptionId);


                        var items = new List<SubscriptionItemUpdateOption> {
                                        new SubscriptionItemUpdateOption {
                                        Id= subscriptionItemUpdate.Items.Data[0].Id,
                                        PlanId = subscriptionItemUpdate.Plan.Id,
                                        Quantity= 1,

                                       },
                                    };

                        var subscriptionUpdateoptions = new SubscriptionUpdateOptions
                        {
                            Items = items,
                            //Billing = Billing.ChargeAutomatically,

                            //BillingThresholds = { },
                            Prorate = false,
                            BillingCycleAnchorUnchanged = false,
                            TrialEnd = subscriptionItemUpdate.CurrentPeriodEnd.Value.AddDays(addFreeDays),
                            //EndTrialNow = true,

                        };
                        stripeSubscription = subscriptionService.Update(profileDTO.StripeSubscriptionId, subscriptionUpdateoptions);
                        jr.Data = new { ResultType = "Success", message = "Free days successfully added to customer Current subscription.", User };
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
                    CustomerTargetProfileDTO profileDTO = _customerManager.GetSocialProfilesById(proId);

                    if (profileDTO != null)
                    {
                       
                        if (profileDTO != null)
                        {
                            if (profileDTO.ProfileStatusId != 18)
                            {
                                if (!string.IsNullOrEmpty(profileDTO.StripeSubscriptionId))
                                {
                                    StripeConfiguration.SetApiKey(_stripeApiKey);
                                    var service = new SubscriptionService();
                                    var subscription = service.Cancel(profileDTO.StripeSubscriptionId, null);
                                }
                                else
                                {
                                    SubscriptionDTO cancelledSub = new SubscriptionDTO();
                                    cancelledSub = _customerManager.GetLastCancelledSubscription(profileDTO.SocialProfileId, DateTime.Now);
                                    profileDTO.StripeSubscriptionId = cancelledSub.StripeSubscriptionId;
                                    profileDTO.StripeInvoiceId = cancelledSub.StripeInvoiceId;

                                }

                            }
                        }

                        Subscription subscriptionItemUpdate = subscriptionService.Get(profileDTO.StripeSubscriptionId);
                        var invoiceService = new InvoiceService();
                        var invoice = invoiceService.Get(profileDTO.StripeInvoiceId);
                        var chargeService = new ChargeService();
                        var charge = chargeService.Get(invoice.ChargeId);
                        var refundOptions = new RefundCreateOptions
                        {
                            Amount = charge.Amount,
                            Reason = RefundReasons.RequestedByCustomer,
                            ChargeId = charge.Id
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
                                var User = _customerManager.SaveUpdateUserDataIndividually(value, "Comment", custId, profileDTO.SocialProfileId);


                            }

                            jr.Data = new { ResultType = "Success", message = "Amount refunded successfully.", User };
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