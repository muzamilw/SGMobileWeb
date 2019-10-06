using PagedList;
using SG2.CORE.BAL.Managers;
using SG2.CORE.MODAL.ViewModals.Backend.Proxy;
using SG2.CORE.WEB.App_Start;
using System;
using System.Web.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SG2.CORE.MODAL.ViewModals.Backend.VPSSupplier;
using SG2.CORE.MODAL.DTO.VPSSupplier;
using SG2.CORE.COMMON;
using System.IO;
using SG2.CORE.MODAL.DTO.SystemSettings;
using SG2.CORE.WEB.Architecture;

namespace SG2.CORE.WEB.Areas.SuperAdmin.Controllers
{
    [SuperAdminAuthorizeFilters]
    public class VPSSupplierController : BaseController
    {
        protected readonly VPSSupplierManager _VPSSManager;
        protected readonly ProxyManager _proxyManager;
        private readonly string _PageSize = string.Empty;
        protected readonly CustomerManager _customerManager;
        protected readonly CommonManager _commonManager;
        protected readonly List<SystemSettingsDTO> SystemConfigs;


        public VPSSupplierController()
        {
            _VPSSManager = new VPSSupplierManager();
            _PageSize = WebConfigurationManager.AppSettings["PageSize"];
            _proxyManager = new ProxyManager();
            _commonManager = new CommonManager();
            //  ViewBag.SetMenuActiveClass = "ProxyIPs";
            SystemConfigs = SystemConfig.GetConfigs;

        }

        // GET: SuperAdmin/VPSSupplier
        public ActionResult Index(int page = 1, string SearchCriteria = "", int? StatusId = null, string ProductId = null)
        {
            try
            {
                var vPSS = _VPSSManager.GetSupplierData(SearchCriteria, _PageSize, page, StatusId ?? 0);
                var totatRecord = vPSS.FirstOrDefault()?.PageSize ?? 0;
                IPagedList<VPSSupplierListingViewModel> pageOrders = new StaticPagedList<VPSSupplierListingViewModel>(vPSS, page, Convert.ToInt32(_PageSize), totatRecord);
                var model = new VPSSupplierIndexViewModel()
                {
                    VPSSupplierListingViewModel = pageOrders,
                    ApplicationStatuses = this.ApplicationStatuses,
                    TotalRecord = pageOrders.PageCount,
                    PageNumber = pageOrders.PageNumber,
                    SearchCriteria = SearchCriteria,
                    StatusId = StatusId
                };

                return View(model);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public ActionResult UpdateVPSSupplier(string id)
        {
            try
            {
                VPSSupplierDTO vPSSupplierDTO = new VPSSupplierDTO();
                UpdateVPSSupplierViewModel updateVPSSupplierViewModel = new UpdateVPSSupplierViewModel();
                int VPSSId = 0;
                
                if (id != null)
                {
                    VPSSId = Convert.ToInt32(CryptoEngine.Decrypt(id));
                }
                   

                vPSSupplierDTO = _VPSSManager.GetVPSSupplier(VPSSId);
                updateVPSSupplierViewModel.Statuses = this.ApplicationStatuses;

                if (vPSSupplierDTO != null)
                {
                    updateVPSSupplierViewModel.IPManageBy = vPSSupplierDTO.IPManageBy;
                    updateVPSSupplierViewModel.IssuingISPAccount = vPSSupplierDTO.IssuingISPAccount;
                    updateVPSSupplierViewModel.IssuingISPMemo = vPSSupplierDTO.IssuingISPMemo;
                    updateVPSSupplierViewModel.IssuingISPName = vPSSupplierDTO.IssuingISPName;
                    updateVPSSupplierViewModel.IssuingISPPassword = vPSSupplierDTO.IssuingISPPassword;
                    updateVPSSupplierViewModel.IssuingISPPhone = vPSSupplierDTO.IssuingISPPhone;
                    updateVPSSupplierViewModel.IssuingISPWebsite = vPSSupplierDTO.IssuingISPWebsite;
                    updateVPSSupplierViewModel.SupportEmail = vPSSupplierDTO.SupportEmail;
                    updateVPSSupplierViewModel.SupportPhone = vPSSupplierDTO.SupportPhone;
                    updateVPSSupplierViewModel.VPSSId = vPSSupplierDTO.VPSSId;
                    updateVPSSupplierViewModel.StatusId=vPSSupplierDTO.StatusId;
                    if (vPSSupplierDTO.VPSSList != null)
                    {
                        updateVPSSupplierViewModel.VPSSupplierListing = vPSSupplierDTO.VPSSList
                       .Select(x => new ProxyListingViewModal()
                       {
                           NoOfFreeSlots = x.NoOfFreeSlots,
                           VPSSName = x.VPSSName,
                           ProxyIPNumber = x.ProxyIPNumber,
                           JVBox = x.JVBox,
                           Region = x.Region,
                           VPSSId = x.VPSSId
                       }).ToList();
                    }
                    else
                    {
                        updateVPSSupplierViewModel.VPSSupplierListing = new List<ProxyListingViewModal>();
                    }
                    
                }
                return View(updateVPSSupplierViewModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        [HttpPost]
        public ActionResult UpdateVPSSupplier(UpdateVPSSupplierViewModel Model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var VPSS = _VPSSManager.AddUpdateVPSSupplier(new VPSSupplierDTO()
                    {
                        VPSSId = Model.VPSSId,
                        IPManageBy = Model.IPManageBy,
                        IssuingISPAccount = Model.IssuingISPAccount,
                        IssuingISPMemo = Model.IssuingISPMemo,
                        IssuingISPName = Model.IssuingISPName,
                        IssuingISPPassword = Model.IssuingISPPassword,
                        IssuingISPPhone = Model.IssuingISPPhone,
                        IssuingISPWebsite = Model.IssuingISPWebsite,
                        SupportEmail = Model.SupportEmail,
                        SupportPhone = Model.SupportPhone,
                        StatusId = Model.StatusId
                    });

                    TempData["Success"] = "Yes";
                    TempData["Message"] = "VPS created successfully.";
                    return RedirectToAction("Index", "VPSSupplier");

                }
                else
                {
                    string messages = string.Join(", ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    TempData["Success"] = "False";
                    TempData["Message"] = messages;
                    return RedirectToAction("Detail");
                }

            }
            catch (Exception)
            {

                throw;
            }
            

        }


        [HttpPost]
        public ActionResult UploadFiles()
        {
           
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];

                        var csvReader = new StreamReader(file.InputStream);
                        var uploadModelList = new List<VPSSCSVRecordViewModel>();
                        string inputDataRead;
                        var values = new List<string>();
                        while ((inputDataRead = csvReader.ReadLine()) != null)
                        {
                            values.Add(inputDataRead.Trim());

                        }
                        values.Remove(values[0]);
                        //values.Remove(values[values.Count - 1]);

                        List<VPSSCSVRecordViewModel> vPSSCSVRecordViewModels = new List<VPSSCSVRecordViewModel>();
                        foreach (var value in values)
                        {
                            var VPSSCSVRecordViewModel = new VPSSCSVRecordViewModel();
                            var eachValue = value.Split(',');
                            VPSSCSVRecordViewModel.Host = eachValue[0] != "" ? eachValue[0] : string.Empty;
                            VPSSCSVRecordViewModel.HostWebsite = eachValue[1] != "" ? eachValue[1] : string.Empty;
                            VPSSCSVRecordViewModel.Country = eachValue[2] != "" ? eachValue[2] : string.Empty;
                            VPSSCSVRecordViewModel.City = eachValue[3] != "" ? eachValue[3] : string.Empty;
                            VPSSCSVRecordViewModel.IP = eachValue[4] != "" ? eachValue[4] : string.Empty;
                            VPSSCSVRecordViewModel.ProxyUserName = eachValue[5] != "" ? eachValue[5] : string.Empty;
                            VPSSCSVRecordViewModel.ProxyPassword = eachValue[6] != "" ? eachValue[6] : string.Empty;
                            VPSSCSVRecordViewModel.ProxyPort = eachValue[7] != "" ? eachValue[7] : string.Empty;
                            vPSSCSVRecordViewModels.Add(VPSSCSVRecordViewModel);// newModel needs to be an object of type ContextTables.
                            
                        }
                        var result=_VPSSManager.ImportCSV(vPSSCSVRecordViewModels);

                        GetGeoPoints();


                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        public ActionResult GetGeoPoints()
        {
            try
            {
                var _googleApiKey = SystemConfigs.First(x => x.ConfigKey == "GoogleMapApiKey").ConfigValue;

                var VPSS = _VPSSManager.GetGeoPoints();

                if (VPSS != null) {
                    List<GeoPointsDTO> geoPointsDTOs = new List<GeoPointsDTO>();
                    foreach (var item in VPSS)
                    {
                        GeoPointsDTO geoPointsDTO = new GeoPointsDTO();
                        geoPointsDTO.ProxyId = item.ProxyId;
                        geoPointsDTO.GeoPint = _commonManager.GetLatitudeAndLongitude(item.CityCountryName, _googleApiKey);
                        geoPointsDTOs.Add(geoPointsDTO);
                    }
                    _VPSSManager.SaveGeoPoints(geoPointsDTOs);
                }

                return new EmptyResult();
                
            }
            catch (Exception)
            {

                throw;
            }

        }
        
        public ActionResult AddVPSSupplier()
        {
            try
            {
                AddVPSSupplierViewModel VPSSM = new AddVPSSupplierViewModel();
                VPSSM.Statuses = this.ApplicationStatuses;
                return View(VPSSM);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public ActionResult AddVPSSupplier(AddVPSSupplierViewModel Model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var VPSS = _VPSSManager.AddUpdateVPSSupplier(new VPSSupplierDTO()
                    {
                        VPSSId = Model.VPSSId,
                        IPManageBy = Model.IPManageBy,
                        IssuingISPAccount=Model.IssuingISPAccount,
                        IssuingISPMemo=Model.IssuingISPMemo,
                        IssuingISPName=Model.IssuingISPName,
                        IssuingISPPassword=Model.IssuingISPPassword,
                        IssuingISPPhone=Model.IssuingISPPhone,
                        IssuingISPWebsite=Model.IssuingISPWebsite,
                        SupportEmail=Model.SupportEmail,
                        SupportPhone=Model.SupportPhone,
                        StatusId=Model.StatusId
                    });

                    TempData["Success"] = "Yes";
                    TempData["Message"] = "VPS created successfully.";
                    //return RedirectToAction("UpdateVPSSupplier", "VPSSupplier", new { id = HttpUtility.UrlEncode(CryptoEngine.Encrypt(VPSS.VPSSId.ToString()))});
                    return RedirectToAction("Index", "VPSSupplier");


                }
                else
                {
                    string messages = string.Join(", ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    TempData["Success"] = "False";
                    TempData["Message"] = messages;
                    return RedirectToAction("Detail");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult Delete(string id)
        {
            var jr = new JsonResult();

            int VPSSId = Convert.ToInt32(CryptoEngine.Decrypt(id));
            var DJVB = _VPSSManager.DeleteSuppliers(VPSSId);
            if (DJVB)
            {
                jr.Data = new { ResultType = "Success", message = "Item deleted successfully." };

            }
            else
            {
                jr.Data = new { ResultType = "Error", message = "Error. Item no deleted. Please contact admin." };

            }
            return Json(jr, JsonRequestBehavior.AllowGet);


            // return RedirectToAction("Index", "LikeAccts");
        }

        public JsonResult IsSupplierExist(string IssuingISPName, int VPSSId = 0)
        {
            try
            {
                return _VPSSManager.IsSupplierExist(IssuingISPName, VPSSId) ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}