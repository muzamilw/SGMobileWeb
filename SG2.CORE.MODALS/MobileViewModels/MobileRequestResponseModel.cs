using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.MobileViewModels
{
    public class MobileLoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Pin { get; set; }

        [Required]
        public string IMEI { get; set; }

        [Required]
        public bool ForceSwitchDevice { get; set; }

        public string AppVersion { get; set; }

        public string AppTimeZoneOffSet { get; set; }

    }

    public class MobileLoginResponse
    {
        public int SocialProfileId { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }

        public int BlockCode { get; set; }

        public DateTime BlockDateTimeUTC { get; set; }

        public bool SocialPasswordNeeded { get; set; }

        public bool IsBroker { get; set; }

        public Nullable<int> BrokerPaymentPlanID { get; set; }
        public string BrokerLogo { get; set; }
        public string BrokerAppName { get; set; }
        public string BrokerStrapLine { get; set; }
        public string BrokerAspectColor { get; set; }
        public string BrokerTrainingLink { get; set; }
        public string BrokerTermsOfUse { get; set; }
        public string BrokerPrivacyPolicy { get; set; }
        public string BrokerFeedbackPage { get; set; }
        public string BrokerHomePage { get; set; }
        public string BrokerTrustPilotCode { get; set; }

        public bool InitialStatsReceived { get; set; }
    }


    public class MobileActionResponse
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }

        public bool ManifestUpdated { get; set; }

    }


    public class MobileManifestResponse
    {
        public int CustomerId { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }

        public MobileSocialProfile Profile { get; set; }

        public MobilePaymentPlan CurrentPlan { get; set; }

        public MobileSocialProfile_Instagram_TargetingInformation TargetInformation { get; set; }

        public List<MobileSocialProfile_FollowedAccounts> FollowersToUnFollow { get; set; }

        public List<MobileSocialProfile_FollowedAccounts> FollowersToComment { get; set; }

        public List<MobileSocialProfile_FollowedAccounts> FollowList { get; set; }

        public List<MobileSocialProfile_FollowedAccounts> LikeList { get; set; }
    }


    public class MobileManifestRequest
    {
        [Required]
        public int SocialProfileId { get; set; }
        
        public string SocialPassword { get; set; }


      

    }

    public class MobileIniitalStatsRequest
    {
        [Required]
        public int SocialProfileId { get; set; }
        [Required]
        public string InitialFollowings { get; set; }
        [Required]
        public string InitialFollowers { get; set; }
        [Required]
        public string InitialPosts { get; set; }

    }


    public class MobileActionRequest
    {
        [Required]
        public int SocialProfileId { get; set; }
        [Required]
        public int ActionId { get; set; }
        
        public string TargetSocialUserName { get; set; }
        
        public string Message { get; set; }

        public string ActionDateTime { get; set; }
    }


    public class MobileSocialProfile
    {
        public int SocialProfileId { get; set; }
        public int CustomerId { get; set; }
        public string SocialProfileType { get; set; }
        public string Status { get; set; }
        public string SocialUsername { get; set; }
        public string SocialPassword { get; set; }
        public string verificationCode { get; set; }
        public string DeviceIMEI { get; set; }
        public int DeviceStatus { get; set; }
       
      
        public Nullable<System.DateTime> LastConnectedDateTime { get; set; }


        public Int64 StatsFollowersIncrease { get; set; }

        public Int64 StatsFollowingsIncrease { get; set; }
        


    }


    public class MobileSocialProfile_Instagram_TargetingInformation
    {
        public int TargetingInformationId { get; set; }
        public Nullable<int> SocialProfileId { get; set; }
        public Nullable<bool> IsSystem { get; set; }
       
        public Nullable<bool> FollowOn { get; set; }
        public Nullable<bool> UnFollowOn { get; set; }
        public string HashTagsToEngage { get; set; }
        public string LocationsToEngage { get; set; }
        public string DirectCompetitors { get; set; }
        public Nullable<int> GenderEngagmentPref { get; set; }
        public Nullable<int> IncludeBusinessAccounts { get; set; }
        public Nullable<bool> MonOper { get; set; }
        public Nullable<bool> TueOper { get; set; }
        public Nullable<bool> WedOper { get; set; }
        public Nullable<bool> ThuOper { get; set; }
        public Nullable<bool> FriOper { get; set; }
        public Nullable<bool> SatOper { get; set; }
        public Nullable<bool> SunOper { get; set; }
        public string ExecutionIntervals { get; set; }
       
        public Nullable<bool> FollUserProfileImage { get; set; }
        public Nullable<bool> FollUserMinPosts { get; set; }
        public Nullable<int> FollUserMinPostsVal { get; set; }
        public Nullable<bool> FollUserPostsLastXDays { get; set; }
        public Nullable<int> FollUserPostsLastXDaysVal { get; set; }
        public Nullable<bool> FollDoNotFollowUsernamewithdigits { get; set; }
        public Nullable<int> FollDoNotFollowUsernamewithdigitsVal { get; set; }
        public Nullable<bool> FollUserLangs { get; set; }
        public string FollUserLangsList { get; set; }
        public Nullable<bool> AfterFollLikeuserPosts { get; set; }
        public Nullable<bool> AfterFollCommentUserPosts { get; set; }
        public Nullable<bool> AfterFollViewUserStory { get; set; }
        public Nullable<bool> AfterFollCommentUserStory { get; set; }
        public Nullable<bool> AfterFollMuteUser { get; set; }
        public Nullable<bool> UnFollFollowersAfterMinDays { get; set; }
        public Nullable<int> UnFollFollowersAfterMinDaysVal { get; set; }
        public Nullable<bool> UnFollDoNotUnFollowLikersOfPosts { get; set; }
        public Nullable<int> UnFollDoNotUnFollowLikersOfPostsCount { get; set; }
        public Nullable<bool> UnFollDoNotUnFollowCommThatCommented { get; set; }
        public Nullable<int> UnFollDoNotUnFollowCommThatCommentedCount { get; set; }
        public Nullable<bool> UnFollUseWhiteList { get; set; }
        public string WhistListManualUsers { get; set; }
        public string WhilstListImportedUsers { get; set; }
        public string BlackListUsers { get; set; }
        public string BlackListLocations { get; set; }
        public string BlackListHashtags { get; set; }
        public string BlackListWordsManual { get; set; }

        public Nullable<int> LikeExchangeDailyLimit { get; set; }
        public Nullable<int> FollowExchangeDailyLimit { get; set; }

    }


    public partial class MobilePaymentPlan
    {
        
       

        public int PaymentPlanId { get; set; }
        public int NoOfFollow { get; set; }
        public int NoOfStoryView { get; set; }
        public int NoOfComments { get; set; }
        public double DisplayPrice { get; set; }
        public string PlanName { get; set; }
       
        public bool IsBrokerPlan { get; set; }
       
        public int NoOfLikesDuration { get; set; }
      
        public int SocialPlatform { get; set; }


       
    }

    public partial class MobileSocialProfile_FollowedAccounts
    {
        public string FollowedSocialUsername { get; set; }
        public Nullable<System.DateTime> FollowedDateTime { get; set; }

        
    }
}
