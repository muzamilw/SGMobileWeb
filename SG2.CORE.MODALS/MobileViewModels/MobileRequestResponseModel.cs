﻿using System;
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

        public bool SocialPasswordNeeded { get; set; }
    }


    public class MobileManifestResponse
    {
        public int CustomerId { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }

        public MobileSocialProfile Profile { get; set; }

        public MobileSocialProfile_Instagram_TargetingInformation TargetInformation { get; set; }
    }


    public class MobileManifestRequest
    {
        [Required]
        public int SocialProfileId { get; set; }
        
        public string SocialPassword { get; set; }

    }


    public class MobileSocialProfile
    {
        public int SocialProfileId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> SocialProfileTypeId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string StripeSubscriptionId { get; set; }
        public Nullable<int> StripeCustomerId { get; set; }
        public Nullable<int> PaymentPlanId { get; set; }
        public string SocialUsername { get; set; }
        public string SocialPassword { get; set; }
        public string SocialProfileName { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string verificationCode { get; set; }
        public Nullable<bool> IsArchived { get; set; }
        public string IMSI { get; set; }
        public string DeviceIMEI { get; set; }
        public Nullable<int> DeviceStatus { get; set; }
        public string PinCode { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> LastConnectedDateTime { get; set; }
    }


    public class MobileSocialProfile_Instagram_TargetingInformation
    {
        public int TargetingInformationId { get; set; }
        public Nullable<int> SocialProfileId { get; set; }
        public Nullable<bool> IsSystem { get; set; }
        public Nullable<bool> FollowOn { get; set; }
        public Nullable<bool> LikeOn { get; set; }
        public Nullable<bool> UnFollowOn { get; set; }
        public Nullable<bool> StoryViewerOn { get; set; }
        public Nullable<bool> ContactMembersOn { get; set; }
        public string HashTagsToEngage { get; set; }
        public string HashTagsToNotEngage { get; set; }
        public string LocationsToEngage { get; set; }
        public string DirectCompetitors { get; set; }
        public Nullable<int> GenderEngagmentPref { get; set; }
        public Nullable<bool> IncludeBusinessAccounts { get; set; }
        public Nullable<bool> MonOper { get; set; }
        public Nullable<bool> TueOper { get; set; }
        public Nullable<bool> WedOper { get; set; }
        public Nullable<bool> ThuOper { get; set; }
        public Nullable<bool> FriOper { get; set; }
        public Nullable<bool> SatOper { get; set; }
        public Nullable<bool> SunOper { get; set; }
        public string ExecutionIntervals { get; set; }
        public Nullable<bool> RandomizeIntervalsDaily { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<int> SocialAccAs { get; set; }
        public Nullable<int> FollNewPerDayLim { get; set; }
        public Nullable<int> FollDailyIncreaseLim { get; set; }
        public Nullable<int> FollMaxPerDayLim { get; set; }
        public Nullable<int> UnFollNewPerDayLim { get; set; }
        public Nullable<int> UnFollDailyIncreaseLim { get; set; }
        public Nullable<int> UnFollMaxPerDayLim { get; set; }
        public Nullable<int> LikePerDayLim { get; set; }
        public Nullable<int> LikeDailyIncreaseLim { get; set; }
        public Nullable<int> LikeMaxPerDayLim { get; set; }
        public Nullable<int> ViewStoriesPerDayLim { get; set; }
        public Nullable<int> ViewStoriesDailyIncreaseLim { get; set; }
        public Nullable<int> ViewStoriesMaxPerDayLim { get; set; }
        public Nullable<int> CommentPerDayLim { get; set; }
        public Nullable<int> CommentDailyIncreaseLim { get; set; }
        public Nullable<int> CommentMaxPerDayLim { get; set; }
        public Nullable<int> DMPerDayLim { get; set; }
        public Nullable<int> DMDailyIncreaseLim { get; set; }
        public Nullable<int> DMMaxPerDayLim { get; set; }
        public Nullable<bool> FollUserProfileImage { get; set; }
        public Nullable<int> FollUserMinPosts { get; set; }
        public Nullable<int> FollUserPostsLastXDays { get; set; }
        public Nullable<bool> FollUseBlackList { get; set; }
        public Nullable<bool> FollCheckPostsCapBlackList { get; set; }
        public Nullable<bool> FollDoNotFollowUsernamewithdigits { get; set; }
        public Nullable<int> FollDoNotFollowUsernamewithdigitsVal { get; set; }
        public Nullable<bool> FollUserLangs { get; set; }
        public string FollUserLangsList { get; set; }
        public Nullable<int> FollowWaitBetweenActionsVal1 { get; set; }
        public Nullable<int> FollowWaitBetweenActionsVal2 { get; set; }
        public Nullable<bool> FollowCompetitorsFollowers { get; set; }
        public Nullable<int> FollowCompetitorsFollowersVal { get; set; }
        public Nullable<bool> FollowUsersfromHashtagResults { get; set; }
        public Nullable<int> FollowUsersfromHashtagResultsVal { get; set; }
        public Nullable<bool> AfterFollLikeuserPosts { get; set; }
        public Nullable<bool> AfterFollCommentUserPosts { get; set; }
        public Nullable<bool> AfterFollViewUserStory { get; set; }
        public Nullable<bool> AfterFollMuteUser { get; set; }
        public Nullable<bool> FollEngageDaily { get; set; }
        public Nullable<int> FollEngageDailyfollCountFrmUnFollowList { get; set; }
        public Nullable<bool> FollEngageLikeRecentPost { get; set; }
        public Nullable<bool> FollEngageEnableLikeCommAfterPostLike { get; set; }
        public Nullable<bool> FollEngageSendDMAfterLike { get; set; }
        public Nullable<bool> FollEngageViewUserStoryAfterLike { get; set; }
        public Nullable<bool> UnFollFollowersAfterMinDays { get; set; }
        public Nullable<int> UnFollFollowersAfterMinDaysVal { get; set; }
        public Nullable<bool> UnFollDoNotUnFollowLikersOfPosts { get; set; }
        public Nullable<int> UnFollDoNotUnFollowLikersOfPostsCount { get; set; }
        public Nullable<bool> UnFollDoNotUnFollowCommThatCommented { get; set; }
        public Nullable<int> UnFollDoNotUnFollowCommThatCommentedCount { get; set; }
        public Nullable<bool> UnFollUseWhiteList { get; set; }
        public Nullable<bool> StorVwOnlyPostXMin { get; set; }
        public Nullable<int> StorVwOnlyPostXMinVal { get; set; }
        public Nullable<bool> StorVwAfterReply { get; set; }
        public Nullable<int> StorVwAfterLikeSendDM { get; set; }
        public Nullable<bool> CommUsrRecentPosts { get; set; }
        public Nullable<int> CommUsrRecentPostsVal { get; set; }
        public Nullable<int> CommUsrRecentPostsTypes { get; set; }
        public Nullable<bool> CommUserPostedWithinXDays { get; set; }
        public Nullable<int> CommUserPostedWithinXDaysVal { get; set; }
        public Nullable<bool> CommFltrPostsByLike { get; set; }
        public Nullable<int> CommFltrPostsByLikeCount { get; set; }
        public Nullable<bool> CommDelCommAfterXDays { get; set; }
        public Nullable<int> CommDelCommAfterXDaysCount { get; set; }
        public string CommLine1 { get; set; }
        public string CommLine2 { get; set; }
        public string CommLine3 { get; set; }
        public string CommLine4 { get; set; }
        public string CommLine5 { get; set; }
        public Nullable<bool> DMShowUnReadMsg { get; set; }
        public Nullable<bool> DMShowPendingReq { get; set; }
        public Nullable<bool> LEXLikeUsrMostRecentPosts { get; set; }
        public Nullable<int> LEXLikeUsrMostRecentPostsCount { get; set; }
        public Nullable<bool> LEXUseBlackList { get; set; }
        public Nullable<bool> LEXChkPostCaptionsforBlackList { get; set; }
        public Nullable<bool> LEXPostedWithinLastXDays { get; set; }
        public Nullable<int> LEXPostedWithinLastXDaysVal { get; set; }
        public Nullable<bool> LEXPostActionEmpty { get; set; }
        public string LEXDonotLikePvtUsers { get; set; }
        public Nullable<bool> LEXSkipBizAccount { get; set; }
        public string WhistListManualUsers { get; set; }
        public string WhilstListImportedUsers { get; set; }
        public string BlackListUsers { get; set; }
        public string BlackListLocations { get; set; }
        public string BlackListHashtags { get; set; }
        public string BlackListWordsManual { get; set; }
        
    }
}
