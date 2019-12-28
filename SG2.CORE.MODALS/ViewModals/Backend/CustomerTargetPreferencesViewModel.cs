using SG2.CORE.MODAL.DTO.Common;
using SG2.CORE.MODAL.DTO.Customers;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SG2.CORE.MODAL.ViewModals.Backend
{
    public class CustomerTargetPreferencesViewModel
    {
        public string Id { get; set; }

        public string SPId { get; set; }

        public int? Status { get; set; }

        public string UserName { get; set; }


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



        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Choose the hashtags you want to engage with: ")]
        public string Preference1 { get; set; }

        [Display(Name = "Choose the hashtags you DO NOT want to engage with (optional):")]
        public string Preference2 { get; set; }

        [Display(Name = "Choose up to 8 locations to focus on. Cities work best (optional):")]
        public string Preference3 { get; set; }

        [Display(Name = "Choose up to 8 locations to skip (optional):")]
        public string Preference4 { get; set; }
        //[Required(ErrorMessage = "Required")]
        [Display(Name = "A very effective growth strategy is to incorporate an amount of Follow & Unfollow activities on target accounts. Do you permit this activity?")]
        public int? Preference5 { get; set; }
        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Do you want to engage with Males, Females or Both")]
        public int? Preference6 { get; set; }
        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Do you want to include Business accounts?")]
        public int? Preference7 { get; set; }
        [Display(Name = "Targeted User Demographics(optional):")]
        public string Preference8 { get; set; }

        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Direct competitors or similar accounsts. Provide at least  10 accounsts with a following of at least 2000 followers:*")]
        public string Preference9 { get; set; }

        //  [Required(ErrorMessage = "Required")]
        //  [Display(Name = "Turn on AI targeting. We will seek out our competitors and match their hashtags")]
        public int? Preference10 { get; set; }






















        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Your Instagram Username:")]
        public string InstaUser { get; set; }
        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Instagram Password:")]
        [DataType(DataType.Password)]
        public string InstaPassword { get; set; }

        [Display(Name = "What country do you mostly log in from?")]
        public int? Country { get; set; }

        [Display(Name = "What city do you mostly log in from?")]

        public string SPStatus { get; set; }
        public string JVStatus { get; set; }

        public string SocialProfileName { get; set; }
        public string Email { get; set; }
        public string CustomerUserName { get; set; }
        public int? City { get; set; }
        public int? NoOfProfile { get; set; }
        public IList<CountryDTO> Countries { get; set; }

        public IList<CityDTO> Cities { get; set; }

        public IList<StatusDTO> JarveeStatuses { get; set; }

    
        public List<ProxyIPDTO> ProxyIPs { get; set; }
        public int? ProxyIP { get; set; }

        public string JVName { get; set; }
        public string IP { get; set; }
        [Display(Name = "I want to grow my instagram account as")]
        public int SocialAccAS { get; set; }
        public string VerificationCode { get; set; }

        public int? MPBox { get; set; }

        public string Notes { get; set; }
        public int? ProxyId { get; set; }

        public string ProxyPort { get; set; }

        public bool IsArchived { get; set; }
    }
}
