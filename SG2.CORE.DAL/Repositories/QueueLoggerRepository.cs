using SG2.CORE.DAL.DB;
using SG2.CORE.MODAL.DTO.QueueLogger;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SG2.CORE.DAL.Repositories
{
    public class QueueLoggerRepository
    {

        //public void InsertQueueAudit(string transactionId, short queueType, short queueStatus, string queueData, string errorDescription, int profileId,string jVBoxData, DateTime createdDate, string createdBy, DateTime modifiedDate, string modifiedBy, int noOfAttempts, string QueueAction , int jVServerId)
        //{
        //    try
        //    {

        //        using (var _db = new SocialGrowth2Entities())
        //        {
        //            _db.SG2_usp_QueueAudit_InsertLog(transactionId, queueType, queueStatus, queueData, errorDescription, profileId, jVBoxData, createdDate, createdBy, modifiedDate, modifiedBy, noOfAttempts, QueueAction, jVServerId);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void InsertQueueAuditDetail(string transactionId, string stepName, string stepDetail, int stepStatus, string stepError, DateTime createdDate, string createdBy, string base64Image)
        //{
        //    try
        //    {

        //        using (var _db = new SocialGrowth2Entities())
        //        {
        //            _db.SG2_usp_QueueAudit_Detail_InsertLog(transactionId, stepName, stepDetail, stepStatus, stepError, createdDate, createdBy, base64Image);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        
        public void InsertQueueAudit(QueueAuditDTO queueAuditDTO)
        {
            try
            {

                using (var _db = new SocialGrowth2Entities())
                {
                    _db.SG2_usp_QueueAudit_InsertLog(queueAuditDTO.TransactionId, queueAuditDTO.QueueType, queueAuditDTO.QueueStatus, queueAuditDTO.QueueData, queueAuditDTO.ErrorDescription, queueAuditDTO.ProfileId, queueAuditDTO.JVBoxData, queueAuditDTO.CreatedDate, queueAuditDTO.CreatedBy, queueAuditDTO.ModifiedDate, queueAuditDTO.ModifiedBy, queueAuditDTO.NoOfAttempts, queueAuditDTO.QueueAction, queueAuditDTO.JVServerId);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public bool UpdateQueueAuditStatus(string transactionId, int statudId)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                   var result = _db.SG2_QueueAudit.SqlQuery("Update SG2_QueueAudit set QueueStatus=@statudId where TransactionId=@transactionId;select top 1 * from SG2_QueueAudit where TransactionId=@transactionId "
                                                    , new SqlParameter ("@statudId", statudId)
                                                    ,new SqlParameter("@transactionId", transactionId)
                                                ).ToList();
                    if (result != null && result.Count > 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public string getimageData(string id)
        {
            try
            {
                string image = null;
                using (var _db = new SocialGrowth2Entities())
                {

                    var result= _db.SG2_usp_QueueAudit_GetimageData(id).FirstOrDefault();
                    if (result != null)
                    {
                        image = System.Text.Encoding.UTF8.GetString(result);
                    }
                    
                }
                return image;
            }
            catch (Exception )
            {

                throw ;
            }


        }

        public void InsertQueueAuditDetail(QueueAuditDetailDTO queueAuditDetailDTO)
        {
            try
            {
                byte[] url = null;
                if (queueAuditDetailDTO.Base64Image is null)
                {

                }
                else
                {
                    url = System.Text.Encoding.UTF8.GetBytes(queueAuditDetailDTO.Base64Image);
                }

                using (var _db = new SocialGrowth2Entities())
                {
                    _db.SG2_usp_QueueAudit_Detail_InsertLog(queueAuditDetailDTO.TransactionId, queueAuditDetailDTO.StepName, queueAuditDetailDTO.StepDetail, queueAuditDetailDTO.StepStatus, queueAuditDetailDTO.StepError, queueAuditDetailDTO.CreatedDate, queueAuditDetailDTO.CreatedBy, url);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<QueueAuditDTO> GetQueueAudit(int jVServerId)
        {
            try
            { 
                using (var _db = new SocialGrowth2Entities())
                {
                    var proDet = _db.SG2_usp_QueueAudit_GetDetail(jVServerId).ToList();
                    List<QueueAuditDTO> queueAuditDTOs = new List<QueueAuditDTO>();
                    if (proDet != null && proDet.Count !=0)
                    {
                        foreach (var pro in proDet)
                        {
                            QueueAuditDTO queueAudit = new QueueAuditDTO()
                            {
                                SocialUsername = pro.SocialUsername,
                                TransactionId = pro.TransactionId,
                                QueueAction = pro.QueueAction,
                                QueueStatusName = pro.QueueStatus,
                                QueueTypeName = pro.QueueType,
                                TotalError = pro.TotalError,
                                TotalInProgress = pro.TotalInProgress,
                                TotalPending = pro.TotalPending,
                                CreatedDate = pro.CreatedDate
                            };
                            queueAuditDTOs.Add(queueAudit);
                        }
                       
                        return queueAuditDTOs;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteBotData(string id)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    _db.SG2_usp_Delete_QueueAuditAndDetail(id);
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool DeleteBotDetailData(int id)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    _db.SG2_QueueAuditDetail.RemoveRange(_db.SG2_QueueAuditDetail.Where(x => x.QueueAuditDetailId == id));
                    _db.SaveChanges();

                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<QueueAuditDetialListViewDTO> GetQueueAuditDetail(string transactionId)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var auditDetialData = _db.SG2_usp_QueueAuditDetail_GetDetail(transactionId).ToList();
                    if (auditDetialData != null)
                    {
                        List<QueueAuditDetialListViewDTO> listQueueAuditDetialListViewDTO = new List<QueueAuditDetialListViewDTO>();
                        foreach (var item in auditDetialData)
                        {
                            QueueAuditDetialListViewDTO queueAuditDetial = new QueueAuditDetialListViewDTO();
                            queueAuditDetial.QueueAuditDetailId = item.QueueAuditDetailId;
                            queueAuditDetial.TransactionId = item.TransactionId;
                            queueAuditDetial.StepStatus = item.StepStatus;
                            queueAuditDetial.StepName = item.StepName;
                            queueAuditDetial.StepError = item.StepError;
                            queueAuditDetial.StepDetail = item.StepDetail;
                            queueAuditDetial.CreatedDate = item.CreatedDate;
                            queueAuditDetial.CreatedBy = item.CreatedBy;
                            queueAuditDetial.Base64Image = item.Base64Image;
                            queueAuditDetial.IsImageExists = item.IsImageExists;
                            listQueueAuditDetialListViewDTO.Add(queueAuditDetial);
                        }
                        return listQueueAuditDetialListViewDTO;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
