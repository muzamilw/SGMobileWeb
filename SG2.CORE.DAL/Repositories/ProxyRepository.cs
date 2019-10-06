using SG2.CORE.COMMON;
using SG2.CORE.DAL.DB;
using SG2.CORE.MODAL.DTO.Proxy;
using SG2.CORE.MODAL.ViewModals.Backend.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.DAL.Repositories
{
    public class ProxyRepository
    {

        public ProxyDTO AddProxyIP(ProxyDTO entity)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {

                    _db.SG2_usp_Proxy_Save(entity.ProxyId, entity.ProxyIPNumber, entity.ProxyIPName, entity.BaseCity, entity.BaseCountry, entity.GeoPoints, entity.IssuingISPNameId, null, null, null, entity.ProxyPort, entity.StatusId);
                    return entity;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ProxyDTO UpdateProxyIP(ProxyDTO entity)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    _db.SG2_usp_Proxy_Save(entity.ProxyId, entity.ProxyIPNumber, entity.ProxyIPName, entity.BaseCity, entity.BaseCountry, entity.GeoPoints, entity.IssuingISPNameId, null, null, null, entity.ProxyPort, entity.StatusId);
                    return entity;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public bool IsProxyIPNumberExists(string ProxyIPNumber, int ProxyId = 0)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {

                    if (ProxyId == 0) return !_db.SG2_Proxy.Any(u => u.ProxyIPNumber.Equals(ProxyIPNumber));
                    var user = _db.SG2_Proxy.Find(ProxyId);
                    if (user.ProxyIPNumber.Equals(ProxyIPNumber, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                    return !_db.SG2_Proxy.Any(r => r.ProxyIPNumber.Equals(ProxyIPNumber) && r.ProxyId != ProxyId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SaveBadProxyIP(int ProxyId, int SocialProfileId)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var pro = _db.SG2_usp_SocialProfile_BadProxy(ProxyId, SocialProfileId, (int)GlobalEnums.ProxyIP.BadIP).FirstOrDefault();
                    if (pro != null)
                    {

                        return pro.ProxyId.ToString();
                    }

                }
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public ProxyDTO GetProxyById(int proxyId)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var pro = _db.SG2_usp_Proxy_GetById(proxyId).FirstOrDefault();
                    if (pro != null)
                    {
                        ProxyDTO proxy = new ProxyDTO()
                        {
                            ProxyId = pro.ProxyId,
                            ProxyIPNumber = pro.ProxyIPNumber,
                            ProxyIPName = pro.ProxyIPName,
                            BaseCity = pro.BaseCity,
                            BaseCountry = pro.BaseCountry,
                            GeoPoints = pro.GeoPoints,
                            IssuingISPNameId = pro.VPSSId,
                            AssignedCustomerID1 = pro.AssignedCustomerID1,
                            AssignedCustomerID2 = pro.AssignedCustomerID2,
                            AssignedCustomerID3 = pro.AssignedCustomerID3,
                            AssignedCustomer1City = pro.AssignedCustomerID1City,
                            AssignedCustomer2City = pro.AssignedCustomerID2City,
                            AssignedCustomer3City = pro.AssignedCustomerID3City,
                            StatusId = pro.StatusId,
                            ProxyPort = pro.ProxyPort

                        };
                        return proxy;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsProxyAssignedIPExists(int ProxyId)
        {
            try
            {
                bool? IsCustomer = false;
                using (var _db = new SocialGrowth2Entities())
                {
                    IsCustomer = Convert.ToBoolean(_db.SG2_usp_Proxy_IsCustomerIPExist(ProxyId));
                }
                return Convert.ToBoolean(IsCustomer);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IList<BadProxyViewModel> GetBadProxyIPs(string PageSize, int PageNumber)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var proxydata = _db.SG2_usp_GetBadProxyIPs(PageSize, PageNumber).ToList();
                    if (proxydata != null)
                    {
                        List<BadProxyViewModel> badProxyList = new List<BadProxyViewModel>();

                        foreach (var item in proxydata)
                        {
                            BadProxyViewModel badProxyViewModal = new BadProxyViewModel();
                            badProxyViewModal.ProxyId = item.ProxyId;
                            badProxyViewModal.ProxyIP = item.ProxyIPNumber;
                            badProxyViewModal.TotalRecord = item.TotalRecord;
                            badProxyViewModal.Profiles = item.Profiles;

                            badProxyList.Add(badProxyViewModal);
                        }
                        return badProxyList;
                    }

                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<ProxyListingViewModal> GetProxyData(string SearchCriteria, int PageNumber, string PageSize, int? statusId, int? SupplierId)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var proxydata = _db.SG2_usp_Proxy_GetAll(SearchCriteria, PageNumber, PageSize, statusId, SupplierId).ToList();
                    if (proxydata != null)
                    {
                        List<ProxyListingViewModal> proxyListingViewModalsList = new List<ProxyListingViewModal>();

                        foreach (var item in proxydata)
                        {
                            ProxyListingViewModal proxyListingViewModal = new ProxyListingViewModal();
                            proxyListingViewModal.ProxyId = item.ProxyId;
                            proxyListingViewModal.ProxyIPNumber = item.ProxyIPNumber;
                            proxyListingViewModal.NoOfFreeSlots = item.FreeSlots ?? 0;
                            proxyListingViewModal.JVBox = item.JVBoxes;
                            proxyListingViewModal.Region = item.Region;
                            proxyListingViewModal.TotalRecord = item.TotalRecord;
                            proxyListingViewModal.ProxyStatus = item.ProxyIPStatus;
                            proxyListingViewModal.VPSSId = item.VPSSId;
                            proxyListingViewModal.VPSSName = item.VPSSName;
                            proxyListingViewModalsList.Add(proxyListingViewModal);
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

        public bool DeleteProxyIP(int ProxyIpId)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var PxIPId = _db.SG2_usp_Proxy_Delete(ProxyIpId);
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

    }
}
