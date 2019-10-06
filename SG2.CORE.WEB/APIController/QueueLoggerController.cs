using SG2.CORE.BAL.Managers;
using SG2.CORE.MODAL.DTO.QueueLogger;
using System.Web.Http;

namespace SG2.CORE.WEB.APIController
{
    
    [RoutePrefix("api/QueueLogger")]
    public class QueueLoggerController : ApiController
    {
        protected readonly QueueLoggerManager _queueLoggerManager;

        public QueueLoggerController()
        {
            _queueLoggerManager = new QueueLoggerManager();
        }

        [Route("InsertQueueAudit")]
        [HttpPost]
        public void InsertQueueAudit(QueueAuditDTO auditData)
        {
            _queueLoggerManager.InsertQueueAudit(auditData);
        }

        [Route("InsertQueueAuditDetail")]
        [HttpPost]
        public bool InsertQueueAuditDetail(QueueAuditDetailDTO auditdetailData)
        {          
            _queueLoggerManager.InsertQueueAuditDetail(auditdetailData);
            return true;
        }

        [AcceptVerbs("GET")]
        [Route("UpdateQueueAuditStatus/{transactionId}/{statudId}")]
        [HttpGet]
        public bool UpdateQueueAuditStatus(string transactionId, int statudId)
        {
            return _queueLoggerManager.UpdateQueueAuditStatus(transactionId, statudId);
        }

    }
}
