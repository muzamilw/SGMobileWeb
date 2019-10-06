using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.Notification
{
    public class NotificationDTO
    {
        public long Id { get; set; }
        public string Notification { get; set; }
        public short StatusId { get; set; }

        public string StatusName { get; set; }
        public int SocialProfileId { get; set; }
        public string SocialUsername { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime UpdateOn { get; set; }
        public string Updatedby { get; set; }

        public string Mode { get; set; }
    }
}
