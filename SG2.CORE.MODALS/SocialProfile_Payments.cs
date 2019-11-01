//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SG2.CORE.MODAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class SocialProfile_Payments
    {
        public int PaymentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SubscriptionType { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<System.DateTime> PaymentDateTime { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int SocialProfileId { get; set; }
        public string StripeSubscriptionId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string StripePlanId { get; set; }
        public Nullable<int> PaymentPlanId { get; set; }
        public string StripeInvoiceId { get; set; }
    
        public virtual PaymentPlan PaymentPlan { get; set; }
        public virtual SocialProfile SocialProfile { get; set; }
    }
}