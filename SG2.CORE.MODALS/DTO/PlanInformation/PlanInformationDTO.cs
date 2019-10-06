using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.PlanInformation
{
    public class PlanInformationDTO
    {
        public int PlanId { get; set; }
        public int? Likes { get; set; }
        public double? PlanPrice { get; set; }
        public double? DisplayPrice { get; set; }
        public string PlanName { get; set; }
        public string StripePlanId { get; set; }
        public string PlanDescription { get; set; }
        public string PlanType { get; set; }
        public int? StatusId { get; set; }
        public string PlantypeName { get; set; }
        public string StatusName { get; set; }
        public short? SortOrder { get; set; }
        public int NoOfLikesDuration { get; set; }

        public int? SocialPlanTypeId { get; set; }
        public bool? IsDefault { get; set; }
    }
}
