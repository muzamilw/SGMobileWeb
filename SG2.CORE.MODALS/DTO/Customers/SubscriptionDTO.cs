using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.Customers
{
    public class SocialProfile_PaymentsDTO
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

       
        public string PaymentPlanName { get; set; }

      
        public string Status { get; set; }
    }
}

