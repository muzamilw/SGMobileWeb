using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.Customers
{
    public class CustomerTargetProfileDTO
    {
        public int TargetingInformationId { get; set; }
        public string Preference1 { get; set; }
        public string Preference2 { get; set; }
        public string Preference3 { get; set; }
        public string Preference4 { get; set; }
        public int? Preference5 { get; set; }
        public int? Preference6 { get; set; }
        public int? Preference7 { get; set; }
        public string Preference8 { get; set; }
        public string Preference9 { get; set; }
        public int? Preference10 { get; set; }
        public int SocialProfileId { get; set; }
        public string SocialProfileName { get; set; }
        public string SocialUsername { get; set; }
        public string SocialPassword { get; set; }
        public int? PrefferedCountryId { get; set; }
        public int? PrefferedCityId { get; set; }
        public int? SocialProfileTypeId { get; set; }
        public string SocialProfileType { get; set; }
        public string ProfileStatus { get; set; }
        public int? ProfileStatusId { get; set; }
        public int? JVboxId { get; set; }
        public string JVBoxStatusName { get; set; }
        public int? PlanId { get; set; }
        public string PlanName { get; set; }
        public string StripePlanId { get; set; }
        public int? SubscriptionId { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public int? SubscriptionStatusId { get; set; }
        public string SubscriptionStatus { get; set; }
        public string ProxyIPNumber { get; set; }
        public string ProxyPort { get; set; }
        public string ProxyIPName { get; set; }
        public string StripeSubscriptionId { get; set; }
        public int? JVStatusId { get; set; }

        public short? JVAttempts { get; set; }
        public short? JVAttemptStatus { get; set; }
        public string JVBoxExchangeName { get; set; }
        public DateTime? JVAttemptsBlockedTill { get; set; }

        public bool IsOptedMarketingEmail { get; set; }

        public int? SocialAccAS { get; set; }

        public string StripeInvoiceId { get; set; }

        public int? ProxyId { get; set; }

        public bool IsJVServerRunning { get; set; }
    }
}
