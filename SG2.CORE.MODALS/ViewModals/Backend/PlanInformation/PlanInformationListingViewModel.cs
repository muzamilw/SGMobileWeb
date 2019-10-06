using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend.PlanInformation
{
    public class PlanInformationListingViewModel
    {
        public int PlanId { get; set; }
        public int? Likes { get; set; }
        public double? PlanPrice { get; set; }

        public double? DisplayPrice { get; set; }


        public string  PlanName { get; set; }
        public string PlanType { get; set; }

        public string StripPlanId { get; set; }
        public int? TotatRecord { get; set; }
    }
}
