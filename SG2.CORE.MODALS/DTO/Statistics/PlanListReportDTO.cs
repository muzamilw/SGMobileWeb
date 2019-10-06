using SG2.CORE.MODAL.DTO.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.Statistics
{
    public class PlanListReportDTO
    {
        
        public string name { get; set; }
        public string value { get; set; }
        public Int64? TotalPlanSold { get; set; }
    }
}
