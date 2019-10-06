using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.MobileViewModels
{
    public class Settings
    {
        public bool UserHasProfileImage { get; set; }
        public int UserHasMinimumNoOfPost { get; set; }
        public int UserPostedWithLastXDays { get; set; }
        public List<string> UsersBlackList { get; set; }
        public bool CheckPostForBlackListWords { get; set; }
        public bool FollowPrivateAccount { get; set; }
        public bool SkipBusinessAcounts { get; set; }
        public int DoNotFollowAccountHavingDigits { get; set; }
        public List<string> FollowAccountGender { get; set; }
        public List<string> FollowOnlyTheseLanguages { get; set; }
        public bool LikeUserLatesPosts { get; set; }
        public bool CommentsUserLatesPosts { get; set; }
        public bool ViewUserLatestStory { get; set; }
        public bool MuteUserAfterFollow { get; set; }
    }

    public class FollowData
    {
        public List<string> HashTags { get; set; }
        public List<string> GeoLocations { get; set; }
        public List<string> CompetitorAccounts { get; set; }
        public List<string> BlackListAccount { get; set; }
    }

    public class FollowModule
    {
        public bool Enabled { get; set; }
        public Settings Settings { get; set; }
        public FollowData FollowData { get; set; }
    }

    public class UnFollowSettings
    {
        public int UnflollowAfterNoOfDays { get; set; }
        public int DoNotUnfollowLikersThatLikesNoOfPosts { get; set; }
        public int DoNotUnfollowLikersThatCommentsNoOfPosts { get; set; }
        public List<string> WhiteList { get; set; }
    }

    public class UnFollowModule
    {
        public bool Enabled { get; set; }
        public UnFollowSettings Settings { get; set; }
    }

    public class StoryViewModuleSettings
    {
        public int PostedNotMoreThanMinutesAgo { get; set; }
        public int LikeUserRecentPosts { get; set; }
        public bool ReplyToUserStoryAfterView { get; set; }
        public bool SendDirectMessageAfterLike { get; set; }
    }

    public class StoryViewModule
    {
        public bool Enabled { get; set; }
        public StoryViewModuleSettings Settings { get; set; }
    }

    public class CommentsModuleSettings
    {
        public int CommentsMostRecentPosts { get; set; }
        public List<string> CommentsOnly { get; set; }
        public int PostedWithinLastXDays { get; set; }
        public int FilterPostsByNumberOfLikes { get; set; }
    }

    public class CommentsModule
    {
        public bool Enabled { get; set; }
        public CommentsModuleSettings Settings { get; set; }
    }

    public class DirectMessageModuleSettings
    {
        public bool SendToAllNewFollowers { get; set; }
    }

    public class DirectMessageData
    {
        public string Message { get; set; }
        public string ImageBase64String { get; set; }
    }

    public class DirectMessageModule
    {
        public bool Enabled { get; set; }
        public DirectMessageModuleSettings Settings { get; set; }
        public DirectMessageData DirectMessageData { get; set; }
    }

    public class EngagementModuleSettings
    {
        public int EngageEveryDayWithFollowingNo { get; set; }
        public int EngageEveryDayWithUnFollowingNo { get; set; }
        public bool LikeMostRecentPost { get; set; }
        public bool EnableLikeCommentsAfterPostIsLike { get; set; }
        public bool SendDirectMessageAfterLike { get; set; }
        public bool ViewUserStoriesAfterLike { get; set; }
    }

    public class EngagementModuleData
    {
        public string Message { get; set; }
        public string ImageBase64String { get; set; }
    }

    public class EngagementModule
    {
        public bool Enabled { get; set; }
        public EngagementModuleSettings Settings { get; set; }
        public EngagementModuleData DirectMessageData { get; set; }
    }

    public class Action
    {
        public string Type { get; set; }
        public int Count { get; set; }
        public string WaitEachAction { get; set; }
    }

    public class Profile
    {
        public int ProfileId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int ProfileStatusId { get; set; }
        public bool FollowModuleEnabled { get; set; }
        public bool UnfollowModuleEnabled { get; set; }
        public bool LikeModuleEnabled { get; set; }
        public bool CommentsModuleEnabled { get; set; }
        public bool StoryViewModuleEnabled { get; set; }
        public bool EngageMouduleEnabled { get; set; }
        public bool DirectMessagesModuleEnabled { get; set; }
        public string UsernameHashtagsBioNameNotContains { get; set; }
        public FollowModule FollowModule { get; set; }
        public UnFollowModule UnFollowModule { get; set; }
        public StoryViewModule StoryViewModule { get; set; }
        public CommentsModule CommentsModule { get; set; }
        public DirectMessageModule DirectMessageModule { get; set; }
        public EngagementModule EngagementModule { get; set; }
        public List<Action> Actions { get; set; }
    }

    public class Platform
    {
        public int PlatfromId { get; set; }
        public string Name { get; set; }
        public string PlatformStatus { get; set; }
        public List<Profile> Profiles { get; set; }
        public List<string> ProfileUserNames { get; set; }
    }

    public class MobileJsonRootObject
    {
        public int CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public long DocumentDateUTC { get; set; }
        public string DeviceId { get; set; }
        public long LicenseExpiredDateUTC { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public List<Platform> Platforms { get; set; }
    }
}
