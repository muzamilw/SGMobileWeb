using PagedList;
using SG2.CORE.BAL.Managers;
using SG2.CORE.COMMON;
using SG2.CORE.MODAL.DTO.SystemSettings;
using SG2.CORE.MODAL.ViewModals.Backend.SystemSettings;
using SG2.CORE.WEB.App_Start;
using SG2.CORE.WEB.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SG2.CORE.WEB.Areas.SuperAdmin.Controllers
{
    [SuperAdminAuthorizeFilters]
    public class SystemSettingsController : BaseController
    {
        protected readonly SystemSettingsManager _systemSettingsManager;
        private readonly string _PageSize = string.Empty;
        protected readonly CustomerManager _customerManager;
        public SystemSettingsController()
        {
            _customerManager = new CustomerManager();
            _PageSize = WebConfigurationManager.AppSettings["PageSize"];
            _systemSettingsManager = new SystemSettingsManager();
            ViewBag.SetMenuActiveClass = "SystemSettings";
        }
        public ActionResult Index(int page = 1, string SearchCriteria = "", int? StatusId = null)
        {
            try
            {
                var systemSettings = _systemSettingsManager.GetSystemSettingsData(SearchCriteria, _PageSize, page, StatusId);
                var totatRecord = systemSettings.FirstOrDefault()?.TotalRecord ?? 0;
                IPagedList<SystemSettingsListingViewModal> pageOrders = new StaticPagedList<SystemSettingsListingViewModal>(systemSettings, page, Convert.ToInt32(_PageSize), totatRecord);
                var model = new SystemSettingsIndexViewModal()
                {
                    SystemSettingsListing = pageOrders,
                    PageNumber = pageOrders.PageNumber,
                    TotalRecord = pageOrders.PageCount,
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
        public ActionResult Detail(string id)
        {
            SystemSettingsDTO systemSettingsDTO = new SystemSettingsDTO();
            SystemSettingsViewModal model = new SystemSettingsViewModal();
            short configId = 0;
            if (id != null)
                configId = Convert.ToSByte(CryptoEngine.Decrypt(id));

            systemSettingsDTO = _systemSettingsManager.GetSystemSettingsId(configId);
            if (systemSettingsDTO != null)
            {
                model.ConfigId = systemSettingsDTO.ConfigId;
                model.ConfigKey = systemSettingsDTO.ConfigKey;
                model.ConfigValue = systemSettingsDTO.ConfigValue;
                model.ConfigValue2 = systemSettingsDTO.ConfigValue2;
                model.CreatedBy = systemSettingsDTO.CreatedBy;
                model.CreatedOn = systemSettingsDTO.CreatedOn;
                model.ModifiedBy = systemSettingsDTO.ModifiedBy;
                model.ModifiedOn = systemSettingsDTO.ModifiedOn;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Detail(SystemSettingsViewModal model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dl = _systemSettingsManager.UpdateLikeyAccount(new SystemSettingsDTO()
                    {
                        ConfigId = model.ConfigId,
                        ConfigKey = model.ConfigKey,
                        ConfigValue = model.ConfigValue,
                        ConfigValue2 = model.ConfigValue2,
                        ModifiedOn = DateTime.Now,
                        ModifiedBy = "test"//this.CDT.UserName
                    });
                    SystemConfig.ResetConfig();
                    TempData["Success"] = "Yes";
                    TempData["Message"] = "System settings updated successfully.";
                    return RedirectToAction("Index", "SystemSettings");
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