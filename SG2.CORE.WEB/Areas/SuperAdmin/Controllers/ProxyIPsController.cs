using PagedList;
using SG2.CORE.BAL.Managers;
using SG2.CORE.COMMON;
using SG2.CORE.MODAL.DTO.Proxy;
using SG2.CORE.MODAL.ViewModals.Backend.Proxy;
using SG2.CORE.MODAL.ViewModals.Proxy;
using SG2.CORE.WEB.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using static SG2.CORE.COMMON.GlobalEnums;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Data;
using System.Text;
using SG2.CORE.MODAL.DTO.SystemSettings;
using SG2.CORE.WEB.Architecture;

namespace SG2.CORE.WEB.Areas.SuperAdmin.Controllers
{
    [SuperAdminAuthorizeFilters]
    public class ProxyIPsController : BaseController
    {
        protected readonly ProxyManager _proxyManager;
        private readonly string _PageSize = string.Empty;
        protected readonly CustomerManager _customerManager;
        protected readonly List<SystemSettingsDTO> SystemConfigs;

        public ProxyIPsController()
        {
            _customerManager = new CustomerManager();
            _PageSize = WebConfigurationManager.AppSettings["PageSize"];
            _proxyManager = new ProxyManager();
            SystemConfigs = SystemConfig.GetConfigs;
            ViewBag.SetMenuActiveClass = "ProxyIPs";
        }

        public ActionResult Index(int page = 1, string SearchCriteria = "", int? StatusId = null, int? VPSSId = null)
        {
            try
            {
                var jvBoxes = _proxyManager.GetProxyData(SearchCriteria, _PageSize, page, StatusId ?? 0, VPSSId ?? 0);
                var totatRecord = jvBoxes.FirstOrDefault()?.TotalRecord ?? 0;
                IPagedList<ProxyListingViewModal> pageOrders = new StaticPagedList<ProxyListingViewModal>(jvBoxes, page, Convert.ToInt32(_PageSize), totatRecord);
                var model = new ProxyIndexViewModal()
                {
                    ProxyListing = pageOrders,
                    ApplicationStatuses = this.ApplicationStatuses,
                    TotalRecord = pageOrders.PageCount,
                    PageNumber = pageOrders.PageNumber,
                    SearchCriteria = SearchCriteria,
                    StatusId = StatusId,
                    vPSSDTOs = CommonManager.GetVPSDTO()
                };

                return View(model);


            }
            catch (Exception EX)
            {

                throw EX;
            }
        }

        public ActionResult GetBadProxyIPs(int PageNumber = 1)
        {
            var badProxy = _proxyManager.GetBadProxyIPs(_PageSize, PageNumber);
            var totatRecord = badProxy.FirstOrDefault()?.TotalRecord ?? 0;
            IPagedList<BadProxyViewModel> pageOrders = new StaticPagedList<BadProxyViewModel>(badProxy, PageNumber, Convert.ToInt32(_PageSize), totatRecord);
            var model = new BadProxyIndexViewModel()
            {
                BadProxyListing = pageOrders,
                TotalRecord = pageOrders.PageCount,
                PageNumber = pageOrders.PageNumber
            };

            return View(model);
        }

        public ActionResult UpdateProxyIPs(string id)
        {
            ProxyDTO proxyDTO = new ProxyDTO();

            UpdateProxyIPsViewModel model = new UpdateProxyIPsViewModel();

            // var customers = _customerManager.GetCustomers();

            int proxyId = 0;
            if (id != null)
                proxyId = Convert.ToInt32(CryptoEngine.Decrypt(id));

            proxyDTO = _proxyManager.GetProxyByID(proxyId);
            // model.Customers = customers;
            model.StatusIds = this.ApplicationStatuses;
            model.Countries = CommonManager.GetCountries();
            model.Cities = CommonManager.GetCities();
            model.vPSSDTOs = CommonManager.GetVPSDTO();
            if (proxyDTO != null)
            {
                model.ProxyId = proxyDTO.ProxyId;

                if (proxyDTO.ProxyIPNumber != string.Empty)
                {
                    string[] IPs = proxyDTO.ProxyIPNumber.Split('.');
                    model.P1 = IPs[0];
                    model.P2 = IPs[1];
                    model.P3 = IPs[2];
                    model.P4 = IPs[3];
                }

                // model.Customers = customers;
                model.ProxyIPNumber = proxyDTO.ProxyIPNumber;
                model.ProxyIPName = proxyDTO.ProxyIPName;
                model.BaseCity = proxyDTO.BaseCity;
                model.BaseCountry = proxyDTO.BaseCountry;
                model.GeoPoints = proxyDTO.GeoPoints;
                model.IssuingISPNameId = proxyDTO.IssuingISPNameId;
                model.AssignedCustomerID1 = proxyDTO.AssignedCustomerID1;
                model.AssignedCustomerID2 = proxyDTO.AssignedCustomerID2;
                model.AssignedCustomerID3 = proxyDTO.AssignedCustomerID3;
                model.AssignedCustomerID1City = proxyDTO.AssignedCustomer1City;
                model.AssignedCustomerID2City = proxyDTO.AssignedCustomer2City;
                model.AssignedCustomerID3City = proxyDTO.AssignedCustomer3City;
                model.StatusId = proxyDTO.StatusId;
                model.ProxyPort = proxyDTO.ProxyPort == null ? "3177" : proxyDTO.ProxyPort;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateProxyIPs(UpdateProxyIPsViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dl = _proxyManager.UpdateProxy(new ProxyDTO()
                    {
                        ProxyId = model.ProxyId,
                        ProxyIPNumber = model.P1 + "." + model.P2 + "." + model.P3 + "." + model.P4,
                        ProxyIPName = model.ProxyIPName,
                        //IPManageBy = model.IPManageBy,
                        //SupportPhone = model.SupportPhone,
                        //SupportEmail = model.SupportEmail,
                        BaseCity = model.BaseCity,
                        BaseCountry = model.BaseCountry,
                        GeoPoints = model.GeoPoints,
                        IssuingISPNameId = model.IssuingISPNameId,
                        //IssuingISPPhone = model.IssuingISPPhone,
                        //IssuingISPWebsite = model.IssuingISPWebsite,
                        //IssuingISPAccount = model.IssuingISPAccount,
                        //IssuingISPPassword = model.IssuingISPPassword,
                        //IssuingISPMemo = model.IssuingISPMemo,
                        AssignedCustomerID1 = model.AssignedCustomerID1,
                        AssignedCustomerID2 = model.AssignedCustomerID2,
                        AssignedCustomerID3 = model.AssignedCustomerID3,
                        StatusId = model.StatusId,
                        ProxyPort = model.ProxyPort
                    });
                    TempData["Success"] = "Yes";
                    TempData["Message"] = "Proxy updated successfully.";
                    return RedirectToAction("Index", "ProxyIPs");
                }
                else
                {
                    string messages = string.Join(", ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    //TempData["Success"] = "False";
                    //TempData["Message"] = messages;
                    return RedirectToAction("TargettingInformation");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Detail()
        {
            ProxyDTO proxyDTO = new ProxyDTO();

            ProxyViewModal model = new ProxyViewModal();

            var customers = _customerManager.GetCustomers();

            model.Customers = customers;
            model.StatusIds = this.ApplicationStatuses;
            model.Countries = CommonManager.GetCountries();
            model.Cities = CommonManager.GetCities();
            model.vPSSDTOs = CommonManager.GetVPSDTO();

            return View(model);
        }

        [HttpPost]
        public ActionResult Detail(ProxyViewModal model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dl = _proxyManager.UpdateProxy(new ProxyDTO()
                    {
                        ProxyId = model.ProxyId,
                        ProxyIPNumber = model.P1 + "." + model.P2 + "." + model.P3 + "." + model.P4,
                        ProxyIPName = model.ProxyIPName,
                        BaseCity = model.BaseCity,
                        BaseCountry = model.BaseCountry,
                        GeoPoints = model.GeoPoints,
                        IssuingISPNameId = model.IssuingISPNameId,
                        AssignedCustomerID1 = model.AssignedCustomerID1,
                        AssignedCustomerID2 = model.AssignedCustomerID2,
                        AssignedCustomerID3 = model.AssignedCustomerID3,
                        StatusId = model.StatusId,
                        ProxyPort = model.ProxyPort
                    });
                    TempData["Success"] = "Yes";
                    TempData["Message"] = "Proxy updated successfully.";
                    return RedirectToAction("Index", "ProxyIPs");
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

        public JsonResult IsProxyIPNumberExists(string ProxyIPNumber, int ProxyId = 0)
        {
            try
            {
                return _proxyManager.IsProxyIPNumberExists(ProxyIPNumber, ProxyId) ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult SaveBadProxyIp(int ProxyId, int SocialProfileId)
        {
            var jr = new JsonResult();
            try
            {
                var BadProxy = _proxyManager.SaveBadProxyIP(ProxyId, SocialProfileId);
                if (!string.IsNullOrEmpty(BadProxy))
                {

                    jr.Data = new { ResultType = "Success", message = "Item saved successfully." };
                }
                else
                {
                    jr.Data = new { ResultType = "Error", message = "Error. Item no saved. Please contact admin." };
                }
                return Json(jr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult Delete(string id)
        {
            var jr = new JsonResult();
            int ProxyId = Convert.ToInt32(CryptoEngine.Decrypt(id));
            var DJVB = _proxyManager.DeleteProxyIP(ProxyId);
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

        //public ActionResult SaveUpdateProxy(ProxyViewModal model)
        //{

        //    ProxyDTO proxyDTO = new ProxyDTO();
        //    proxyDTO.ProxyId = 1;
        //    proxyDTO.ProxyIPNumber = "198.168.246";
        //    proxyDTO.ProxyIPName = "Test Proxy IP";
        //    proxyDTO.IPManageBy = "OC CH";
        //    proxyDTO.SupportPhone = "03004003426";
        //    proxyDTO.SupportEmail = "raz_syedsaqib@hotmail.com";
        //    proxyDTO.BaseCity = "Lahore";
        //    proxyDTO.BaseCountry = "Pakistain";
        //    proxyDTO.GeoPoints = "37.4219999,-122.08405749999997";
        //    proxyDTO.IssuingISPNameId = "Winhost";
        //    proxyDTO.IssuingISPPhone = "1234";
        //    proxyDTO.IssuingISPWebsite = "Winhost.com";
        //    proxyDTO.IssuingISPAccount = "corvtech";
        //    proxyDTO.IssuingISPPassword = "P@ssword123";
        //    proxyDTO.IssuingISPMemo = "Hello this cloude base hosting";
        //    proxyDTO.AssignedCustomerID1 = 2;
        //    proxyDTO.AssignedCustomerID2 = 1;
        //    proxyDTO.AssignedCustomerID3 = 3;

        //    ProxyDTO proDTO = _proxyManager.UpdateProxy(proxyDTO);



        //    ProxyDTO getProxyDTO = new ProxyDTO();
        //    getProxyDTO = _proxyManager.GETProxyByID(1);

        //    //        TempData["Success"] = "Yes";
        //    //        TempData["Message"] = "Proxy updated successfully.";
        //    //        return RedirectToAction("index", "Proxy");
        //    //    }
        //    //    else
        //    //    {
        //    //        string messages = string.Join(", ", ModelState.Values
        //    //                                .SelectMany(x => x.Errors)
        //    //                                .Select(x => x.ErrorMessage));
        //    //        TempData["Success"] = "False";
        //    //        TempData["Message"] = messages;
        //    //        return RedirectToAction("SaveUpdateProxy");
        //    //    }


        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    throw ex;
        //    //}
        //    return View();
        //}

        public string GetLatitudeAndLongitude(string address)
        {
            var _googleApiKey = SystemConfigs.First(x => x.ConfigKey == "GoogleMapApiKey").ConfigValue;
            string urlAddress = "https://maps.googleapis.com/maps/api/geocode/xml?address=" + address + "&key="+ _googleApiKey;
            string returnValue = "";
            try
            {
                XmlDocument objXmlDocument = new XmlDocument();
                objXmlDocument.Load(urlAddress);
                XmlNodeList objXmlNodeList = objXmlDocument.SelectNodes("/GeocodeResponse/result/geometry/location");
                foreach (XmlNode objXmlNode in objXmlNodeList)
                {
                    // GET  LATITUDE 
                    returnValue = objXmlNode.ChildNodes.Item(0).InnerText;

                    // GET LONGITUDE
                    returnValue += "," + objXmlNode.ChildNodes.Item(1).InnerText;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
    }
}