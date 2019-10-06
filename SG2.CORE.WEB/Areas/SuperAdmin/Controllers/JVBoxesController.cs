using PagedList;
using SG2.CORE.BAL.Managers;
using SG2.CORE.COMMON;
using SG2.CORE.MODAL.DTO.JVBox;
using SG2.CORE.MODAL.ViewModals.Backend.JVBox;
using SG2.CORE.WEB.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SG2.CORE.WEB.Areas.SuperAdmin.Controllers
{
    [SuperAdminAuthorizeFilters]
    public class JVBoxesController : BaseController
    {
        protected readonly JVBoxManager _jVBoxManager;
        private readonly string _PageSize = string.Empty;
        protected readonly CustomerManager _customerManager;

        public JVBoxesController()
        {
            _customerManager = new CustomerManager();
            _PageSize = WebConfigurationManager.AppSettings["PageSize"];
            _jVBoxManager = new JVBoxManager();
            ViewBag.SetMenuActiveClass = "JVBoxes";
        }
        
        public ActionResult Index(int page = 1, string SearchCriteria = "", int? StatusId = null, string ProductId = null)
        {
            try
            {
                var jvBoxes = _jVBoxManager.GetJVBoxData(SearchCriteria, _PageSize, page, StatusId);
                var totatRecord = jvBoxes.FirstOrDefault()?.TotalRecord ?? 0;
                IPagedList<JVBoxListingViewModal> pageOrders = new StaticPagedList<JVBoxListingViewModal>(jvBoxes, page, Convert.ToInt32(_PageSize), totatRecord);
                var model = new JVBoxIndexViewModal()
                {
                    JVBoxListing = pageOrders,
                    ApplicationStatuses = this.ApplicationStatuses,
                    TotalRecord = pageOrders.PageCount,
                    PageNumber= pageOrders.PageNumber,
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

        public ActionResult GetJVBoxCustomers(short id)
        {
            var jvBoxCusHistory = _jVBoxManager.GetJVBoxCustomers(id);

            foreach (var item in jvBoxCusHistory)
            {
                item.CustomerId = Url.Encode(CryptoEngine.Encrypt(item.CustomerId));
            }
            return Json(jvBoxCusHistory, JsonRequestBehavior.AllowGet);

        }

        public ActionResult UpdateJVBox(string id)
        {
            JVBoxDTO jvBoxDTO = new JVBoxDTO();
            UpdateJVBoxViewModal model = new UpdateJVBoxViewModal();

            model.Statuses = this.ApplicationStatuses;

            int likeyAccountId = 0;
            if (id != null)
                likeyAccountId = Convert.ToInt32(CryptoEngine.Decrypt(id));

            jvBoxDTO = _jVBoxManager.GetJVBoxById(likeyAccountId);
            if (jvBoxDTO != null)
            {
                model.JVBoxId = jvBoxDTO.JVBoxId;
                model.BoxName = jvBoxDTO.BoxName;
                model.BoxManagedBy = jvBoxDTO.BoxManagedBy;
                model.AdminName = jvBoxDTO.AdminName;
                model.AdminPassword = jvBoxDTO.AdminPassword;
                model.IPNumber = jvBoxDTO.IPNumber;
                model.SupportPhone = jvBoxDTO.SupportPhone;
                model.SupportEmail = jvBoxDTO.SupportEmail;
                model.HostedBy = jvBoxDTO.HostedBy;
                model.HostingAccount = jvBoxDTO.HostingAccount;
                model.HostingPassword = jvBoxDTO.HostingPassword;
                model.HostingWebsite = jvBoxDTO.HostingWebsite;
                model.HostingPhone = jvBoxDTO.HostingPhone;
                model.HostingPriceInfo = jvBoxDTO.HostingPriceInfo;
                model.StatusId = jvBoxDTO.StatusId;
                model.JVBoxMaxLimit = jvBoxDTO.JVBoxMaxLimit;
                model.JVBSRTypeId = jvBoxDTO.JVBSRTypeId;
                model.JVBoxExchangeName = jvBoxDTO.JVBoxExchangeName;
                model.ServerRunningStatus = jvBoxDTO.ServerRunningStatus;
            }

            return View(model);

        }

        [HttpPost]
        public ActionResult UpdateJVBox(UpdateJVBoxViewModal model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dl = _jVBoxManager.UpdateJVBox(new JVBoxDTO()
                    {
                        JVBoxId = model.JVBoxId,
                        BoxName = model.BoxName,
                        BoxManagedBy = model.BoxManagedBy,
                        AdminName = model.AdminName,
                        AdminPassword = model.AdminPassword,
                        SupportPhone = model.SupportPhone,
                        SupportEmail = model.SupportEmail,
                        HostedBy = model.HostedBy,
                        HostingAccount = model.HostingAccount,
                        HostingPassword = model.HostingPassword,
                        HostingWebsite = model.HostingWebsite,
                        HostingPhone = model.HostingPhone,
                        HostingPriceInfo = model.HostingPriceInfo,
                        StatusId = model.StatusId,
                        JVBoxMaxLimit=model.JVBoxMaxLimit,
                        JVBSRTypeId=model.JVBSRTypeId,
                        JVBoxExchangeName=model.JVBoxExchangeName,
                        ServerRunningStatus=model.ServerRunningStatus
                    });
                    TempData["Success"] = "Yes";
                    TempData["Message"] = "JVBox updated successfully.";
                    return RedirectToAction("Index", "JVBoxes");
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

        public JsonResult IsJVBoxExists(string BoxName, int JVBoxId = 0)
        {
            try
            {
                return _jVBoxManager.IsJVBoxExists(BoxName, JVBoxId) ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Detail(string id)
        {
            JVBoxDTO jvBoxDTO = new JVBoxDTO();
            JVBoxViewModal model = new JVBoxViewModal();
            model.Statuses= this.ApplicationStatuses;
            return View(model);
        }

        public ActionResult Delete(string id)
        {
            var jr = new JsonResult();
            int  JVBoxId = Convert.ToInt32(CryptoEngine.Decrypt(id));
            var DJVB = _jVBoxManager.DeleteJVBox(JVBoxId);
            if (DJVB)
                jr.Data = new { ResultType = "Success", message = "Item deleted successfully." };
            else
                jr.Data = new { ResultType = "Error", message = "Error. Item can not be deleted. Active user assigned to this JVBox." };
            return Json(jr,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Detail(JVBoxViewModal model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dl = _jVBoxManager.UpdateJVBox(new JVBoxDTO()
                    {
                        JVBoxId = model.JVBoxId,
                        BoxName = model.BoxName,
                        BoxManagedBy = model.BoxManagedBy,
                        AdminName = model.AdminName,
                        AdminPassword = model.AdminPassword,
                        SupportPhone = model.SupportPhone,
                        SupportEmail = model.SupportEmail,
                        HostedBy = model.HostedBy,
                        HostingAccount = model.HostingAccount,
                        HostingPassword = model.HostingPassword,
                        HostingWebsite = model.HostingWebsite,
                        HostingPhone = model.HostingPhone,
                        HostingPriceInfo = model.HostingPriceInfo,
                        StatusId = model.StatusId,
                        JVBoxMaxLimit=model.JVBoxMaxLimit,
                        JVBSRTypeId=model.JVBSRTypeId,
                        JVBoxExchangeName=model.JVBoxExchangeName
                        
                });
                    TempData["Success"] = "Yes";
                    TempData["Message"] = "JVBox updated successfully.";
                    return RedirectToAction("Index", "JVBoxes");
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

    }
}