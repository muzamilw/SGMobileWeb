//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SG2.CORE.DAL.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class SG2_SocialProfile_PaymentPlan
    {
        public int PlanId { get; set; }
        public Nullable<int> NoOfLikes { get; set; }
        public string PlanName { get; set; }
        public string PlanShortDescription { get; set; }
        public Nullable<int> PlanTypeId { get; set; }
        public string StripePlanId { get; set; }
        public Nullable<int> NoOfLikesDuration { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<short> SortOrder { get; set; }
        public Nullable<int> SocialPlanTypeId { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public Nullable<double> DisplayPrice { get; set; }
        public Nullable<double> StripePlanPrice { get; set; }
    }
}
