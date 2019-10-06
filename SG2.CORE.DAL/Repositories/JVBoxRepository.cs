using SG2.CORE.DAL.DB;
using SG2.CORE.MODAL.DTO.JVBox;
using SG2.CORE.MODAL.ViewModals.Backend.JVBox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SG2.CORE.DAL.Repositories
{
    public class JVBoxRepository
    {
        public JVBoxDTO AddJVBox(JVBoxDTO entity)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {

                    _db.SG2_usp_JVBox_Save(entity.JVBoxId, entity.BoxName, entity.AdminName, entity.AdminPassword, entity.BoxManagedBy,  entity.SupportEmail, entity.SupportPhone, entity.HostedBy, entity.HostingPhone, entity.HostingWebsite, entity.HostingAccount, entity.HostingPassword, entity.HostingPriceInfo, entity.StatusId,entity.JVBoxMaxLimit,entity.JVBSRTypeId,entity.JVBoxExchangeName,1);
                    return entity;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JVBoxDTO UpdateJVBox(JVBoxDTO entity)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    _db.SG2_usp_JVBox_Save(entity.JVBoxId, entity.BoxName, entity.AdminName, entity.AdminPassword, entity.BoxManagedBy,  entity.SupportEmail, entity.SupportPhone, entity.HostedBy, entity.HostingPhone, entity.HostingWebsite, entity.HostingAccount, entity.HostingPassword, entity.HostingPriceInfo, entity.StatusId,entity.JVBoxMaxLimit,entity.JVBSRTypeId, entity.JVBoxExchangeName,entity.ServerRunningStatus);
                    return entity;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsJVBoxExists(string JVBoxName, int id = 0)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {

                    if (id == 0) return !_db.SG2_JVBox.Any(u => u.BoxName.Equals(JVBoxName));
                    var user = _db.SG2_JVBox.Find(id);
                    if (user.BoxName.Equals(JVBoxName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                    return !_db.SG2_JVBox.Any(r => r.BoxName.Equals(JVBoxName) && r.JVBoxId != id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public JVBoxDTO GetJVBoxById(int JVBoxId)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var pro = _db.SG2_usp_JVBox_GetById(JVBoxId).FirstOrDefault();
                    if (pro != null)
                    {
                        JVBoxDTO box = new JVBoxDTO()
                        {
                            JVBoxId = pro.JVBoxId,
                            BoxName = pro.BoxName,
                            AdminName = pro.AdminName,
                            AdminPassword = pro.AdminPassword,
                            BoxManagedBy = pro.BoxManagedBy,
                            SupportPhone = pro.SupportPhone,
                            SupportEmail = pro.SupportEmail,
                            HostedBy = pro.HostedBy,
                            HostingPhone = pro.HostingPhone,
                            HostingWebsite = pro.HostingWebsite,
                            HostingAccount = pro.HostingAccount,
                            HostingPassword = pro.HostingPassword,
                            HostingPriceInfo = pro.HostingPriceInfo,
                            StatusId = pro.StatusId,
                            JVBoxMaxLimit=pro.MaxLimit??0,
                            JVBSRTypeId=pro.JVBoxType??0,
                            JVBoxExchangeName=pro.ExchangeName,
                            ServerRunningStatus=pro.ServerRunningStatusId

                        };
                        return box;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteJVBox(int JVBoxId)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                   var DJVB= _db.SG2_usp_JVBox_Delete(JVBoxId);
                    if (DJVB == 1)
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
                
        public IList<JVBoxCustomerHistoryDTO> GetJVBoxCustomers(short JVBoxId)
        {
            try
            {
                using (var _db=new SocialGrowth2Entities())
                {
                    var JVBoxHisData = _db.SG2_usp_JVBox_GetCustomerHistory(JVBoxId).ToList();

                    List<JVBoxCustomerHistoryDTO> jVBoxCustomerHistoryDTOs = new List<JVBoxCustomerHistoryDTO>();

                    foreach (var item in JVBoxHisData)
                    {
                        JVBoxCustomerHistoryDTO jVBox = new JVBoxCustomerHistoryDTO();
                        jVBox.CustomerId = Convert.ToString(item.CustomerId);
                        jVBox.CustomerName = item.CusName;
                        jVBox.InstaUser = item.SocialUsername;
                        jVBox.Status = item.Status;
                        jVBoxCustomerHistoryDTOs.Add(jVBox);
                    }
                    return jVBoxCustomerHistoryDTOs;

                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<JVBoxListingViewModal> GetJVBoxData(string SearchCriteria, int PageNumber, string PageSize, int? StatusId)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var jvBoxdata = _db.SG2_usp_JVBox_GetAll(SearchCriteria, PageNumber, PageSize, StatusId).ToList();
                    if (jvBoxdata != null)
                    {
                        List<JVBoxListingViewModal> jVBoxListingViewModalsList = new List<JVBoxListingViewModal>();

                        foreach (var item in jvBoxdata)
                        {
                            JVBoxListingViewModal jVBoxListingViewModal = new JVBoxListingViewModal();
                            jVBoxListingViewModal.JVBoxId = item.JVBoxId;
                            jVBoxListingViewModal.BoxName = item.BoxName;
                            jVBoxListingViewModal.JVBoxMaxLimit = item.MaxLimit;
                            jVBoxListingViewModal.Status = item.JVBStatus;
                            jVBoxListingViewModal.LiveUser = item.LiveUser??0;
                            jVBoxListingViewModal.TotalRecord = item.TotalRecord;
                            jVBoxListingViewModal.ServerRunningStatusId = item.ServerRunningStatusId;
                            jVBoxListingViewModalsList.Add(jVBoxListingViewModal);
                        }
                        return jVBoxListingViewModalsList;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool JVBoxSetQueueStatus(short QueueStatus, int profileId, int jvboxid)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var jVBox = _db.SG2_JVBox.FirstOrDefault(x => x.JVBoxId == jvboxid);
                    if (jVBox != null)
                    {
                        jVBox.QueueStatusId = QueueStatus;

                        _db.SaveChanges();
                        return true;
                    }
                    return false;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool JVBoxSetServerRunningStatus(int jvboxid, int serverrunningstatusId)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var jVBox = _db.SG2_JVBox.FirstOrDefault(x => x.JVBoxId == jvboxid);
                    if (jVBox != null)
                    {
                        jVBox.ServerRunningStatusId = serverrunningstatusId;

                        _db.SaveChanges();
                        return true;
                    }
                    return false;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool JVBoxGetServerRunningStatus(int jvboxid)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var jVBox = _db.SG2_JVBox.FirstOrDefault(x => x.JVBoxId == jvboxid);
                    if (jVBox != null)
                    {
                        if(jVBox.ServerRunningStatusId == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
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
