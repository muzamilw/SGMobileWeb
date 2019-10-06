using SG2.CORE.DAL.DB;
using SG2.CORE.MODAL.DTO.VPSSupplier;
using SG2.CORE.MODAL.ViewModals.Backend.VPSSupplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SG2.CORE.DAL.Repositories
{
   public class VPSSupplierRepository
    {
        public VPSSupplierDTO AddUpdateVPSSupplier(VPSSupplierDTO vPS)
        {
            try
            {
                using (var _db=new SocialGrowth2Entities())
                {
                    var result=_db.SG2_usp_VPSSupplier_SaveUpdate(vPS.VPSSId, vPS.IPManageBy, vPS.SupportPhone, vPS.SupportEmail, vPS.IssuingISPName, vPS.IssuingISPPhone, vPS.IssuingISPWebsite, vPS.IssuingISPAccount,
                        vPS.IssuingISPPassword, vPS.IssuingISPMemo, vPS.StatusId);
                   
                    if (result != null)
                    {
                        vPS.VPSSId = result.Select(m => m.VPSSId).FirstOrDefault();

                    }
                    return vPS;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool ImportCSV(List<VPSSCSVRecordViewModel> vPSSCSVRecordViewModels)
        {
            try
            {
               var xEle = new XElement("Proxes",
               from emp in vPSSCSVRecordViewModels
               select new XElement("Proxy",
                            new XElement("ProxyUserName", emp.ProxyUserName),
                              new XElement("City", emp.City),
                              new XElement("Country", emp.Country),
                              new XElement("Host", emp.Host),
                              new XElement("HostWebsite", emp.HostWebsite),
                              new XElement("ProxyPassword", emp.ProxyPassword),
                               new XElement("IP", emp.IP),
                               new XElement("ProxyPort", emp.ProxyPort)
                          ));
                string ProxyXml = xEle.ToString();
                using (var _db = new SocialGrowth2Entities())
                {
                    var result = _db.SG2_usp_VPSSupplier_BulkInsert(ProxyXml);

                    if (result != null)
                    {
                        List<GeoPointsDTO> GeoPointsDTOs = new List<GeoPointsDTO>();
                        foreach (var item in result)
                        {
                            GeoPointsDTO geoPointsDTO = new GeoPointsDTO();
                            geoPointsDTO.ProxyId = item.ProxyId;
                            geoPointsDTO.CityCountryName = item.FullCityCountryName;

                            GeoPointsDTOs.Add(geoPointsDTO);
                        }
                         

                    }
                    //return vPS;
                }
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public string SaveGeoPoints(List<GeoPointsDTO> geoPointsDTOs)
        {
            try
            {
                var xEle = new XElement("Proxes",
              from emp in geoPointsDTOs
              select new XElement("Proxy",
                           new XElement("GeoPoint", emp.GeoPint),
                             new XElement("ProxyId", emp.ProxyId)
                         ));
                string ProxyXml = xEle.ToString();

                using (var _db = new SocialGrowth2Entities())
                {
                    var result = _db.SG2_usp_ProxyIP_SaveGeoPoints(ProxyXml);

                    if (result == null)
                    {

                        return "Success";
                    }
                    else
                    {
                        return "Error";
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }




        public List<GeoPointsDTO> GetGeoPoints()
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var result = _db.SG2_usp_ProxyIP_GetGeoPoints();
                    if (result != null)
                    {

                        List<GeoPointsDTO> GeoPointsDTOs = new List<GeoPointsDTO>();
                        foreach (var item in result)
                        {
                            GeoPointsDTO geoPointsDTO = new GeoPointsDTO();
                            geoPointsDTO.ProxyId = item.ProxyId;
                            geoPointsDTO.CityCountryName = item.FullCityCountryName;

                            GeoPointsDTOs.Add(geoPointsDTO);
                        }

                        return GeoPointsDTOs;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IList<VPSSupplierListingViewModel> GetSupplierData(string SearchCriteria, int PageNumber, string PageSize, int? statusId)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var proxydata = _db.SG2_usp_VPSSupplier_GetAll(SearchCriteria, PageNumber, PageSize, statusId).ToList();
                    if (proxydata != null)
                    {
                        List<VPSSupplierListingViewModel> proxyListingViewModalsList = new List<VPSSupplierListingViewModel>();

                        foreach (var item in proxydata)
                        {
                            VPSSupplierListingViewModel vPSSListingViewModal = new VPSSupplierListingViewModel();
                            vPSSListingViewModal.VPSSId = item.VPSId;
                            vPSSListingViewModal.IssuingISPName = item.ISPName;
                            vPSSListingViewModal.NoOfAssignedIPs = item.NoOfAssignedIP ?? 0;
                            vPSSListingViewModal.Status = item.StatusName;
                            vPSSListingViewModal.PageSize = item.TotalRecord;
                            proxyListingViewModalsList.Add(vPSSListingViewModal);
                        }
                        return proxyListingViewModalsList;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public VPSSupplierDTO GetSupplierDTO(int VPSSId)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var result = _db.SG2_usp_GETVPSSupplierData(VPSSId);
                    
                    if (result != null)
                    {
                        
                        VPSSupplierDTO vPSSupplierDTO = new VPSSupplierDTO();
                        foreach (var item in result)
                        {
                            vPSSupplierDTO.VPSSId = item.VPSSId;
                            vPSSupplierDTO.SupportPhone = item.SupportPhone;
                            vPSSupplierDTO.SupportEmail = item.SupportEmail;
                            vPSSupplierDTO.StatusId = item.StatusId;
                            vPSSupplierDTO.IssuingISPWebsite = item.IssuingISPWebsite;
                            vPSSupplierDTO.IssuingISPPhone = item.IssuingISPPhone;
                            vPSSupplierDTO.IssuingISPPassword = item.IssuingISPPassword;
                            vPSSupplierDTO.IssuingISPName = item.IssuingISPName;
                            vPSSupplierDTO.IssuingISPMemo = item.IssuingISPMemo;
                            vPSSupplierDTO.IssuingISPAccount = item.IssuingISPAccount;
                            vPSSupplierDTO.IPManageBy = item.IPManageBy;

                        }

                        var VPSSL = _db.SG2_usp_GetProxyIpDataAgainstSupplier(vPSSupplierDTO.VPSSId);

                        List<VPSSupplierListingDTO> vPSSupplierListingDTOs = new List<VPSSupplierListingDTO>();

                        foreach (var item in VPSSL)
                        {
                            VPSSupplierListingDTO vPSSupplierListingDTO = new VPSSupplierListingDTO();
                            vPSSupplierListingDTO.JVBox = item.JVBoxes;
                            vPSSupplierListingDTO.NoOfFreeSlots = item.FreeSlots;
                            vPSSupplierListingDTO.ProxyIPNumber = item.ProxyIPNumber;
                            vPSSupplierListingDTO.VPSSId = item.VPSSId;
                            vPSSupplierListingDTO.VPSSName = item.VPSSName;
                            vPSSupplierListingDTO.Region = item.Region;
                            //vPSSupplierListingDTO.TotalRecord=item.
                            vPSSupplierListingDTOs.Add(vPSSupplierListingDTO);
                        }
                        vPSSupplierDTO.VPSSList = vPSSupplierListingDTOs;
                        return vPSSupplierDTO;
                    }

                    return null;
                }
                
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteSuppliers(int VPSSid)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var PxIPId = _db.SG2_usp_Supplier_Delete(VPSSid);//TODO
                    if (PxIPId == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

      public bool IsSupplierExist(string IssuingISPName, int VPSSId)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {

                    if (VPSSId == 0) return !_db.SG2_VPSSupplier.Any(u => u.IssuingISPName.Equals(IssuingISPName));
                    var user = _db.SG2_VPSSupplier.Find(VPSSId);
                    if (user.IssuingISPName.Equals(IssuingISPName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                    return !_db.SG2_VPSSupplier.Any(r => r.IssuingISPName.Equals(IssuingISPName) && r.VPSSId != VPSSId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
