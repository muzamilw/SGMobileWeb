using SG2.CORE.MODAL.DTO.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Statistics
{
    public class AdminReportViewModel
    {
        public Int64? AllSlotsOnJVBox { get; set; }
        public Int64? UsedSlotsOnJVBox { get; set; }
        public Int64? FreeSlotsOnJVServer { get; set; }
        public Int64? AllAvailableIPs { get; set; }
        public Int64? TotalUsedIPs { get; set; }
        public Int64? RemainingProxyIPs { get; set; }

      
       

    }
}
