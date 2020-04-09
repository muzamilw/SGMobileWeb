using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.Customers
{
   public class CustomerDTO
    {
        public int CustomerId { get; set; }
        public int SocialProfileId { get; set; }
        public int DefaultSocialProfileId { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public short StatusId { get; set; }
        public System.DateTime LastLoginDate { get; set; }
        public byte LoginAttempts { get; set; }
        public string LastLoginIP { get; set; }
        public string Tocken { get; set; }
        public string GUID { get; set; }
        
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneCode { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PostCode { get; set; }

        public string Notes { get; set; }

        public bool IsOptedEducationalEmailSeries { get; set; }
        public bool IsOptedMarketingEmail { get; set; }
        public string StripeCustomerId { get; set; }
        public string StripeSubscriptionId { get; set; }
        public string StripePlanId { get; set; }
        public string ActivePlanId { get; set; }
        public Nullable<bool> IsBroker { get; set; }
        public Nullable<int> BrokerPaymentPlanID { get; set; }

    }
}
