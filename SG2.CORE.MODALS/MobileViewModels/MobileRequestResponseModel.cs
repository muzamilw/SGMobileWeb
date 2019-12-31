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
    }


    public class MobileActionResponse
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }

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
        public int InitialFollowings { get; set; }
        [Required]
        public int InitialFollowers { get; set; }
        [Required]
        public int InitialPosts { get; set; }

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
        public int SocialProfileId { get; set; }
        
        public bool FollowOn { get; set; }
        public bool LikeOn { get; set; }
        public bool UnFollowOn { get; set; }
        public bool StoryViewerOn { get; set; }
        public bool ContactMembersOn { get; set; }
        public string HashTagsToEngage { get; set; }
        public string HashTagsToNotEngage { get; set; }
        public string LocationsToEngage { get; set; }
        public string DirectCompetitors { get; set; }
        public int GenderEngagmentPref { get; set; }
        public int IncludeBusinessAccounts { get; set; }
        public bool MonOper { get; set; }
        public bool TueOper { get; set; }
        public bool WedOper { get; set; }
        public bool ThuOper { get; set; }
        public bool FriOper { get; set; }
        public bool SatOper { get; set; }
        public bool SunOper { get; set; }
        public string ExecutionIntervals { get; set; }
        public bool RandomizeIntervalsDaily { get; set; }
       
        public int SocialAccAs { get; set; }
      
        public int FollMaxPerDayLim { get; set; }
   
        public int UnFollMaxPerDayLim { get; set; }
     
        public int LikeMaxPerDayLim { get; set; }
       
        public int ViewStoriesMaxPerDayLim { get; set; }
       
        public int CommentMaxPerDayLim { get; set; }
      
        //public int DMMaxPerDayLim { get; set; }


        public bool FollUserProfileImage { get; set; }
        public bool FollUserMinPosts { get; set; }
        public int FollUserMinPostsVal { get; set; }
        public bool FollUserPostsLastXDays { get; set; }
        public int FollUserPostsLastXDaysVal { get; set; }
        public bool FollUseBlackList { get; set; }
        public bool FollCheckPostsCapBlackList { get; set; }
        public bool FollDoNotFollowUsernamewithdigits { get; set; }
        public int FollDoNotFollowUsernamewithdigitsVal { get; set; }
        public bool FollUserLangs { get; set; }
        public string FollUserLangsList { get; set; }
        public int FollowWaitBetweenActionsVal1 { get; set; }
        public int FollowWaitBetweenActionsVal2 { get; set; }
        public bool FollowCompetitorsFollowers { get; set; }
        public int FollowCompetitorsFollowersVal { get; set; }
        public bool FollowUsersfromHashtagResults { get; set; }
        public int FollowUsersfromHashtagResultsVal { get; set; }
        public bool AfterFollLikeuserPosts { get; set; }
        public bool AfterFollCommentUserPosts { get; set; }
        public bool AfterFollViewUserStory { get; set; }
        public bool AfterFollMuteUser { get; set; }
        public bool FollEngageDaily { get; set; }
        public bool FollEngageDailyfollCountFrmUnFollowList { get; set; }
        public int FollEngageDailyfollCountFrmUnFollowListVal { get; set; }
        public bool FollEngageLikeRecentPost { get; set; }
        public bool FollEngageEnableLikeCommAfterPostLike { get; set; }
        public bool FollEngageSendDMAfterLike { get; set; }
        public bool FollEngageViewUserStoryAfterLike { get; set; }
        public bool UnFollFollowersAfterMinDays { get; set; }
        public int UnFollFollowersAfterMinDaysVal { get; set; }
        public bool UnFollDoNotUnFollowLikersOfPosts { get; set; }
        public int UnFollDoNotUnFollowLikersOfPostsCount { get; set; }
        public bool UnFollDoNotUnFollowCommThatCommented { get; set; }
        public int UnFollDoNotUnFollowCommThatCommentedCount { get; set; }
        public bool UnFollUseWhiteList { get; set; }
        public bool StorVwOnlyPostXMin { get; set; }
        public int StorVwOnlyPostXMinVal { get; set; }
        public bool StorVwAfterReply { get; set; }
        public bool StorVwAfterLikeSendDM { get; set; }
        public bool CommUsrRecentPosts { get; set; }
        public int CommUsrRecentPostsVal { get; set; }
        public int CommUsrRecentPostsTypes { get; set; }
        public bool CommUserPostedWithinXDays { get; set; }
        public int CommUserPostedWithinXDaysVal { get; set; }
        public bool CommFltrPostsByLike { get; set; }
        public int CommFltrPostsByLikeCount { get; set; }
        public bool CommDelCommAfterXDays { get; set; }
        public int CommDelCommAfterXDaysCount { get; set; }
        public string CommLine1 { get; set; }
        public string CommLine2 { get; set; }
        public string CommLine3 { get; set; }
        public string CommLine4 { get; set; }
        public string CommLine5 { get; set; }
        public bool DMShowUnReadMsg { get; set; }
        public bool DMShowPendingReq { get; set; }
        public bool LEXLikeUsrMostRecentPosts { get; set; }
        public int LEXLikeUsrMostRecentPostsCount { get; set; }
        public bool LEXUseBlackList { get; set; }
        public bool LEXChkPostCaptionsforBlackList { get; set; }
        public bool LEXPostedWithinLastXDays { get; set; }
        public int LEXPostedWithinLastXDaysVal { get; set; }
        public bool LEXPostActionEmpty { get; set; }
        public bool LEXDonotLikePvtUsers { get; set; }
        public bool LEXSkipBizAccount { get; set; }
        public string WhistListManualUsers { get; set; }
        public string WhilstListImportedUsers { get; set; }
        public string BlackListUsers { get; set; }
        public string BlackListLocations { get; set; }
        public string BlackListHashtags { get; set; }
        public string BlackListWordsManual { get; set; }

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
