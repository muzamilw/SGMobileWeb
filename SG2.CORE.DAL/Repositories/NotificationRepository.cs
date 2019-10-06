using SG2.CORE.DAL.DB;
using SG2.CORE.MODAL.DTO.Notification;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SG2.CORE.DAL.Repositories
{
    public class NotificationRepository
    {

        public List<NotificationDTO> GetNotificationByStatusId(short StatusId)
        {
            try
            {
                List<NotificationDTO> results = null;
                using (var _db = new SocialGrowth2Entities())
                {
                    var notes = _db.SG2_usp_SocialProfile_GetNotificationsByStatus(StatusId);
                    if (notes != null)
                    {
                        results = new List<NotificationDTO>();
                        foreach (var item in notes)
                        {
                            NotificationDTO nt = new NotificationDTO();
                            nt.Id = item.Id;
                            nt.Notification = item.Notification;
                            nt.SocialProfileId = item.SocialProfileId;
                            nt.SocialUsername = item.SocialUsername;
                            nt.StatusId = item.StatusId;
                            nt.StatusName = item.StatusId == 51 ? "Unread" : "Read";
                            nt.Updatedby = item.Updatedby;
                            nt.UpdateOn = item.UpdateOn;
                            nt.CreatedOn = item.CreatedOn;
                            nt.CreatedBy = item.CreatedBy;
                            nt.Mode = item.Mode;
                            results.Add(nt);
                        }
                    }
                    return results;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<NotificationDTO> AddNotification (NotificationDTO model)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var dt = _db.SG2_SocialProfile_Notification.Add(new SG2_SocialProfile_Notification()
                    {
                        Notification = model.Notification,
                        SocialProfileId = model.SocialProfileId,
                        StatusId = model.StatusId,
                        UpdatedBy = model.Updatedby,
                        UpdateOn = model.UpdateOn,
                        CreatedOn = model.CreatedOn,
                        CreatedBy = model.CreatedBy,
                        Mode = model.Mode
                    });
                    await _db.SaveChangesAsync();
                    if (dt != null)
                    {
                        model.Id = model.Id;
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteNotification(int notificationId)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                   
                    _db.SG2_SocialProfile_Notification.RemoveRange(_db.SG2_SocialProfile_Notification.Where(x => x.Id == notificationId));
                    _db.SaveChanges();
                  
                        return true;
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateNotification(int notificationId, Int16 statusId)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var result = _db.SG2_SocialProfile_Notification.SqlQuery("Update SG2_SocialProfile_Notification set StatusId=@statudId where id=@notificationId;select top 1 * from SG2_SocialProfile_Notification where id=@notificationId "
                                                   , new SqlParameter("@statudId", statusId)
                                                   , new SqlParameter("@notificationId", notificationId)
                                               ).ToList();
                    _db.SG2_SocialProfile_Notification.RemoveRange(_db.SG2_SocialProfile_Notification.Where(x => x.Id == notificationId));
                    _db.SaveChanges();
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
        
    }
}
