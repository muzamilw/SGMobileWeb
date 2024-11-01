﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.TargetPreferences
{
    public class TargetPreferencesDTO
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
        public Nullable<bool> FollDoNotFollowPrivateUser { get; set; }
        public Nullable<bool> FollSkipBizAcct { get; set; }
        public Nullable<int> FollDoNotFollowUsernamewithdigits { get; set; }
        public Nullable<int> FollOnlyFollowGender { get; set; }
        public string FollUserLangs { get; set; }
        public Nullable<bool> AfterFollLikeuserPosts { get; set; }
        public Nullable<int> AfterFollLikeuserPostsWaitSecsFrom { get; set; }
        public Nullable<int> AfterFollLikeuserPostsWaitSecsTo { get; set; }
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
        public Nullable<bool> StorVwLikeUsrPostRcntPost { get; set; }
        public Nullable<int> StorVwLikeUsrPostRcntPostVal { get; set; }
        public Nullable<bool> StorVwReply { get; set; }
        public Nullable<bool> StorVwSendDMAfterLike { get; set; }
        public Nullable<bool> CommUsrRecentPosts { get; set; }
        public Nullable<int> CommUsrRecentPostsVal { get; set; }
        public Nullable<int> CommUsrRecentPostsTypes { get; set; }
        public Nullable<bool> CommFltrPostsByLikeCount { get; set; }
        public Nullable<bool> CommDelCommAfterXDays { get; set; }
        public Nullable<int> CommDelCommAfterXDaysCount { get; set; }
        public Nullable<bool> DMShowUnReadMsg { get; set; }
        public Nullable<bool> DMShowPendingReq { get; set; }
        public Nullable<bool> LEXLikeUsrMostRecentPosts { get; set; }
        public Nullable<int> LEXLikeUsrMostRecentPostsCount { get; set; }
        public Nullable<bool> LEXUseBlackList { get; set; }
        public Nullable<bool> LEXChkPostCaptionsforBlackList { get; set; }
        public int SocialProfileId1 { get; set; }
        public string SocialProfileName { get; set; }
        public string SocialUsername { get; set; }
        public string SocialPassword { get; set; }
        public Nullable<int> SocialProfileTypeId { get; set; }
        public string SocialProfileType { get; set; }
        public string ProfileStatus { get; set; }
        public Nullable<int> ProfileStatusId { get; set; }
        public Nullable<int> paymentPlanId { get; set; }
        public string PlanName { get; set; }
        public string StripePlanId { get; set; }
        public Nullable<int> SubscriptionId { get; set; }
        public Nullable<System.DateTime> SubscriptionStartDate { get; set; }
        public Nullable<System.DateTime> SubscriptionEndDate { get; set; }
        public Nullable<int> SubscriptionStatusId { get; set; }
        public string SubscriptionStatus { get; set; }
        public string StripeSubscriptionId { get; set; }
        public Nullable<int> SocialAccAS1 { get; set; }
        public string StripeInvoiceId { get; set; }
        public Nullable<bool> IsArchived { get; set; }
        public string InstaUser { get; set; }
        public string InstaPassword { get; set; }
        public int? Country { get; set; }
        public int? City { get; set; }
       

    }
}
