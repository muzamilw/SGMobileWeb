using SG2.CORE.DAL.Repositories;
using SG2.CORE.MODAL.DTO.QueueLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.BAL.Managers
{
    public class QueueLoggerManager
    {

        private readonly QueueLoggerRepository _queueLoggerRepository;
        private readonly SessionManager _sessionManager;

        public QueueLoggerManager()
        {
            _queueLoggerRepository = new QueueLoggerRepository();
            _sessionManager = new SessionManager();
        }



        public void InsertQueueAudit(QueueAuditDTO queueAuditDTO)
        {
            try
            {

                _queueLoggerRepository.InsertQueueAudit(queueAuditDTO);
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
                return _queueLoggerRepository.UpdateQueueAuditStatus(transactionId, statudId);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public void InsertQueueAuditDetail(QueueAuditDetailDTO queueAuditDetailDTO)
        {
            try
            {

                _queueLoggerRepository.InsertQueueAuditDetail(queueAuditDetailDTO);
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
                return _queueLoggerRepository.getimageData(id);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<QueueAuditDTO> GetQueueAudit(int jVServerId)
        {
            try
            {
                var queueAuditDTO = _queueLoggerRepository.GetQueueAudit(jVServerId);

                return queueAuditDTO;
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
                var queueAuditDTO = _queueLoggerRepository.DeleteBotData(id);
                return queueAuditDTO;
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
                var queueAuditDTO = _queueLoggerRepository.DeleteBotDetailData(id);
                return queueAuditDTO;
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
                var queueAuditDTO = _queueLoggerRepository.GetQueueAuditDetail(transactionId);
                return queueAuditDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<QueueAuditDetialListViewDTO> GetQueueAuditData(string TransactionId)
        //{
        //    try
        //    {

        //        var queueAuditDTO = _queueLoggerRepository.GetQueueAuditDetail(TransactionId);
        //        //if (queueAuditDTO != null)
        //        //{
        //        //    //List<QueueAuditDetialListViewDTO> queueAuditDetilList = _queueLoggerRepository.GetQueueAuditDetail(queueAuditDTO.TransactionId);

        //        //    if (queueAuditDetilList != null)
        //        //    {
        //        //        queueAuditDTO.QueueAuditDetialLists = queueAuditDetilList;
        //        //    }
        //        //}
        //        return queueAuditDTO;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
}
