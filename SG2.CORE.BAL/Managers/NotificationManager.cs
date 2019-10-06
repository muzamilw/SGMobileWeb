using SG2.CORE.DAL.Repositories;
using SG2.CORE.MODAL.DTO.Notification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SG2.CORE.BAL.Managers
{
    public class NotificationManager
    {
        private readonly NotificationRepository _notificationRep;

        public NotificationManager()
        {
            _notificationRep = new NotificationRepository();
        }

        public List<NotificationDTO> GetNotificationByStatusId(short StatusId)
        {
            try
            {
                return _notificationRep.GetNotificationByStatusId(StatusId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<NotificationDTO> AddNotification(NotificationDTO model)
        {
            try
            {
                return await _notificationRep.AddNotification(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
             public bool DeleteNotification(int notificationId, Int16 statusId)
        {
            try
            {
                return _notificationRep.DeleteNotification(notificationId);
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
                return  _notificationRep.UpdateNotification(notificationId, statusId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
