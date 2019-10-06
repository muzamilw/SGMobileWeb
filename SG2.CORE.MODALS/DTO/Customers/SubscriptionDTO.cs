using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.Customers
{
    public class SubscriptionDTO
    {

        public string Name { get; set; }
        public int SubscriptionId { get; set; }
     
        public string Description { get; set; }

        public string SubscriptionType { get; set; }
        public long? Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CustomerId { get; set; }
        public int SocialProfileId { get; set; }
        public string StripeSubscriptionId { get; set; }
        public int StatusId { get; set; }
        public string StripePlanId { get; set; }
        public int PaymentPlanId { get; set; }
        public string StripeInvoiceId { get; set; }
    }
}
