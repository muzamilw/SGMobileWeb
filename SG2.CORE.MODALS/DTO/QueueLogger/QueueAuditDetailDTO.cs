using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.QueueLogger
{
    public class QueueAuditDetailDTO
    {
        public int QueueAuditDetailId { get; set; }
        public string TransactionId { get; set; }
        public string StepName { get; set; }
        public string StepDetail { get; set; }
        public int StepStatus { get; set; }
        public string StepError { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Base64Image { get; set; }

    }
}
