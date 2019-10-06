using PagedList;
using SG2.CORE.BAL.Managers;
using SG2.CORE.COMMON;
using SG2.CORE.MODAL.DTO.PlanInformation;
using SG2.CORE.MODAL.ViewModals.Backend.PlanInformation;
using SG2.CORE.WEB.App_Start;
using SG2.CORE.WEB.Architecture;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SG2.CORE.WEB.Areas.SuperAdmin.Controllers
{
    [SuperAdminAuthorizeFilters]
    public class PlanInformationController : BaseController
    {
        protected readonly PlanInformationManager _planInformationManager;
        private readonly string _PageSize = string.Empty;

        public PlanInformationController()
        {
            _PageSize = WebConfigurationManager.AppSettings["PageSize"];
            _planInformationManager = new PlanInformationManager();

        }
        // GET: SuperAdmin/PlanInformation
        public ActionResult Index(int page = 1, string SearchCriteria = "", int? StatusId = null)

        {
            try
            {
                var planInformation = _planInformationManager.GetPlanInformationData(SearchCriteria, _PageSize, page, StatusId);
                var totatRecord = planInformation.FirstOrDefault()?.TotatRecord ?? 0;
                IPagedList<PlanInformationListingViewModel> pageOrders = new StaticPagedList<PlanInformationListingViewModel>(planInformation, page, Convert.ToInt32(_PageSize), totatRecord);
                var model = new PlanInformationIndexViewModel()
                {
                    PlanInformationListing = pageOrders,
                    ApplicationStatuses = this.ApplicationStatuses,
                    TotalRecord = pageOrders.PageCount,
                    PageNumber = pageOrders.PageNumber,
                    SearchCriteria = SearchCriteria,
                    StatusId = StatusId
                };

                return View(model);

            }
            catch (Exception EX)
            {

                throw EX;
            }
        }

        public ActionResult SavePlan()
        {
            PlanInformationSaveViewModel model = new PlanInformationSaveViewModel();
            model.PlanTypes = this.ApplicationStatuses;
           
            return View(model);
        }

        [HttpPost]
        public ActionResult SavePlan(PlanInformationSaveViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Plan subscription = new Plan();
                    var StripeInstagramProductId = SystemConfig.GetConfigs.First(x => x.ConfigKey == "StripeInstagramProductId").ConfigValue;
                    var Stripe = SystemConfig.GetConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;
                    StripeConfiguration.SetApiKey(Stripe);
                    var options = new PlanCreateOptions
                    {
                        ProductId = StripeInstagramProductId,
                        Nickname = model.PlanName,
                        Amount = Convert.ToInt32(model.PlanPrice * 100),
                        Active = true,
                        Currency = "usd",
                        Interval = "month",
                    };

                    var service = new PlanService();
                    try
                    {
                        subscription = service.Create(options);
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    
                    var dl = _planInformationManager.SavePlan(new PlanInformationDTO()
                    {
                        PlanId = model.PlanId,
                        PlanName = model.PlanName,
                        Likes = model.Likes,
                        PlanType = model.PlanType,
                        PlanPrice = model.PlanPrice,
                        DisplayPrice=model.DisplayPrice,
                        PlanDescription = model.PlanDescription,
                        NoOfLikesDuration = model.NoOfLikesDuration,
                        StatusId = model.StatusId,
                        SortOrder = model.SortOrder,
                        IsDefault=model.IsDefault,
                        StripePlanId= subscription.Id,
                        SocialPlanTypeId=model.SocialPlanTypeId

                    });
                    TempData["Success"] = "Yes";
                    TempData["Message"] = "Plan added successfully.";
                    return RedirectToAction("Index", "PlanInformation");
                }
                else
                {
                    string messages = string.Join(", ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    //TempData["Success"] = "False";
                    //TempData["Message"] = messages;
                    return View(model);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult UpdatePlan(string id)
        {
            PlanInformationDTO planInformationDTO = new PlanInformationDTO();
           
            int planInformationId = 0;
            if (id != null)
                planInformationId = Convert.ToInt32(CryptoEngine.Decrypt(id));

            PlanInformationUpdateViewModel model = new PlanInformationUpdateViewModel();
            planInformationDTO = _planInformationManager.GetPlanInformationById(planInformationId);

            model.PlanTypes = this.ApplicationStatuses;

            if (planInformationDTO != null)
            {
                model.PlanId = planInformationDTO.PlanId;
                model.PlanName = planInformationDTO.PlanName;
                model.PlanType = planInformationDTO.PlanType;
                model.PlanPrice = planInformationDTO.PlanPrice;
                model.DisplayPrice = planInformationDTO.DisplayPrice;
                model.Likes = planInformationDTO.Likes;
                model.StripePlanId = planInformationDTO.StripePlanId;
                model.NoOfLikesDuration = planInformationDTO.NoOfLikesDuration;
                model.PlanDescription = planInformationDTO.PlanDescription;
                model.StatusId = planInformationDTO.StatusId;
                model.SortOrder = planInformationDTO.SortOrder;
                model.IsDefault = planInformationDTO.IsDefault;
                model.SocialPlanTypeId = planInformationDTO.SocialPlanTypeId;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdatePlan(PlanInformationUpdateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Plan subscription = new Plan();
                    if (string.IsNullOrEmpty(model.StripePlanId))
                    {
                        var StripeInstagramProductId = SystemConfig.GetConfigs.First(x => x.ConfigKey == "StripeInstagramProductId").ConfigValue;
                        var Stripe = SystemConfig.GetConfigs.First(x => x.ConfigKey == "Stripe").ConfigValue;

                        StripeConfiguration.SetApiKey(Stripe);
                        var options = new PlanCreateOptions
                        {
                            ProductId = StripeInstagramProductId,
                            Nickname = model.PlanName,
                            Amount = Convert.ToInt32(model.PlanPrice * 100),
                            Active = true,
                            Currency = "usd",
                            Interval = "month",
                        };
                        var service = new PlanService();
                        try
                        {
                            subscription = service.Create(options);
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }
                    }

                    var dl = _planInformationManager.SavePlan(new PlanInformationDTO()
                    {
                        PlanId = model.PlanId,
                        PlanName = model.PlanName,
                        Likes = model.Likes,
                        PlanType = model.PlanType,
                        PlanPrice = model.PlanPrice,
                        DisplayPrice = model.DisplayPrice,
                        PlanDescription = model.PlanDescription,
                        NoOfLikesDuration=model.NoOfLikesDuration,
                        StatusId=model.StatusId,
                        SortOrder=model.SortOrder,
                        IsDefault=model.IsDefault,
                        StripePlanId= model.StripePlanId==null? subscription.Id : model.StripePlanId,
                        SocialPlanTypeId = model.SocialPlanTypeId

                    });
                    TempData["Success"] = "Yes";
                    TempData["Message"] = "Plan Updated successfully.";
                    return RedirectToAction("Index", "PlanInformation");
                }
                else
                {
                    string messages = string.Join(", ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    //TempData["Success"] = "False";
                    //TempData["Message"] = messages;
                    return View(model);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public JsonResult IsPlanNameExists(string PlanName, int PlanId = 0)
        {
            try
            {
                return _planInformationManager.IsPlanNameExists(PlanName, PlanId) ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Delete(string id)
        {
            var jr = new JsonResult();

            int PlanInformationId = Convert.ToInt32(CryptoEngine.Decrypt(id));
            var DJVB = _planInformationManager.DeletePlanInformation(PlanInformationId);
            if (DJVB)
            {
                jr.Data = new { ResultType = "Success", message = "Item deleted successfully." };

            }
            else
            {
                jr.Data = new { ResultType = "Error", message = "Error. Item no deleted. Please contact admin." };

            }
            return Json(jr, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TestPlanCreate()
        {
            try
            {
                Plan subscription = new Plan();

                StripeConfiguration.SetApiKey("sk_test_V839rr4Lo2a3QO7ASjHFpusK000oc2bBhe");

                var options = new PlanCreateOptions
                {
                    //Product = new PlanProductCreateOptions
                    //{
                        
                    //    Name = "LikeyXA"
                    //},
                    ProductId = "prod_Ev6ss2wq4CxHA1",
                    Nickname= "LikeyXAB",
                    Amount = 5000,
                    Active = true,
                    Currency = "usd",
                    Interval = "month",
                };

                var service = new PlanService();
                Plan plan = service.Create(options);

                //Plan subscriptionupdate = new Plan();
                //StripeConfiguration.SetApiKey("sk_test_V839rr4Lo2a3QO7ASjHFpusK000oc2bBhe");
                //var options1 = new PlanUpdateOptions
                //{
                //    Metadata = new Dictionary<string, string>
                //     {
                //        { "order_id", "6735" },
                //     },
                //};
                //var planService = new PlanService();
                //subscriptionupdate = planService.Update("plan_FHvNgK6Zt5IP3p", options1);
                return new EmptyResult();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ActionResult TestPlanUpdate()
        {
            Plan subscriptionupdate = new Plan();
            StripeConfiguration.SetApiKey("sk_test_V839rr4Lo2a3QO7ASjHFpusK000oc2bBhe");
            var options = new PlanUpdateOptions
                {
                    Metadata = new Dictionary<string, string>
                     {
                        { "order_id", "6735" },
                     },
                };
            var planService = new PlanService();
             subscriptionupdate= planService.Update("plan_Ev718shqgYet13", options);
            return new EmptyResult();

        }
    }
}