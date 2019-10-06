using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.JVBox
{
    public class JVBoxDTO
    {

        public int JVBoxId { get; set; }
        public  string BoxName { get; set; }
        public string  AdminName { get; set; }
        public string  AdminPassword { get; set; }
        public string  BoxManagedBy { get; set; }
        public string HostingPassword { get; set; }
        public string SupportEmail { get; set; }
        public string IPNumber { get; set; }
        public string HostedBy { get; set; }
        public string HostingPhone { get; set; }
        public string  HostingWebsite { get; set; }
        public string HostingAccount { get; set; }
        public string SupportPhone { get; set; }
        public string HostingPriceInfo { get; set; }
        public int JVBoxMaxLimit { get; set; }
        public int StatusId { get; set; }
        public int JVBSRTypeId { get; set; }
        public string JVBoxExchangeName { get; set; }

        public int ServerRunningStatus { get; set; }
    }
}
