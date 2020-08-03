using System;

namespace SG2.CORE.MODAL.DTO.Customers
{
    public class CustomerSocialProfileDTO
    {
        public int SocialProfileId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public int? SocialProfileTypeId { get; set; }
        public Nullable<int> JVBoxId { get; set; }
        public Nullable<int> JVBoxStatusId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string StripeCustomerId { get; set; }
        public string SocialUsername { get; set; }
        public string SocialPassword { get; set; }
        public string ProfileName { get; set; }
        public Nullable<int> PrefferedCityId { get; set; }
        public Nullable<int> PrefferedCountryId { get; set; }
        public string StatusName { get; set; }
        public string JVBoxStatusName { get; set; }
        public string SocialProfileTypeName { get; set; }
        public string BlockedStatus { get; set; }

        public string SubscriptionName { get; set; }
        public string SubScriptionStatus { get; set; }

        public Nullable<bool> FollowOn { get; set; }
        public Nullable<bool> UnFollFollowersAfterMinDays { get; set; }
        public Nullable<bool> AfterFollLikeuserPosts { get; set; }
        public Nullable<bool> AfterFollViewUserStory { get; set; }

        public string AppConnStatus { get; set; }

    }
}
