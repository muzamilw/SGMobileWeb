using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.QueueLogger
{
    public class QueueAuditDTO
    {
        public List<QueueAuditDetialListViewDTO> QueueAuditDetialLists = new List<QueueAuditDetialListViewDTO>();
        public int QueueAuditId { get; set; }
        public string TransactionId { get; set; }
        public short? QueueType { get; set; }
        public short? QueueStatus { get; set; }
        public string QueueData { get; set; }
        public string ErrorDescription { get; set; }
        public int ProfileId { get; set; }
        public string JVBoxData { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public int NoOfAttempts { get; set; }
        public string QueueAction { get; set; }
        public int JVServerId { get; set; }
        public int? TotalError { get; set; }
        public int? TotalInProgress { get; set; }
        public int? TotalPending { get; set; }
        public string QueueTypeName { get; set; }
        public string QueueStatusName { get; set; }
        public string SocialUsername { get; set; }
    }
}
