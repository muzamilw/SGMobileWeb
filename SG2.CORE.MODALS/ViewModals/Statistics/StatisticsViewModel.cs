using SG2.CORE.MODAL.DTO.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Statistics
{
    public class StatisticsViewModel
    {

        public IEnumerable<SocialProfile_Statistics> StatisticsListing { get; set; }

        public Int64? PostsTotal { get; set; }
        public Int64? PostsInitial { get; set; }

        public Int64? FollowersTotal { get; set; }
        public Int64? FollowersInitial { get; set; }

        public Int64? FollowingsTotal { get; set; }
        public Int64? FollowingsInitial { get; set; }

        public Int64? FollowsTotal { get; set; }
        public Int64? FollowsRecent { get; set; }

        public Int64? LikesTotal { get; set; }
        public Int64? LikesInitial { get; set; }

        public Int64? UnFollowTotal { get; set; }
        public Int64? UnFollowInitial { get; set; }

        public Int64? StoryViewsTotal { get; set; }
        public Int64? StoryViewsInitial { get; set; }


        public int FollowingToday { get; set; }
        public int FollowingTodayLimit { get; set; }

        public int UnFollowToday { get; set; }
        public int UnFollowLimit { get; set; }

        public int LikeToday { get; set; }
        public int LikeLimit { get; set; }

        public int StoryViewToday { get; set; }
        public int StoryViewLimit { get; set; }


        public double? LastSessionIndex { get; set; }
        public string AppStatus { get; set; }
        public double? FollowersGrowthRate { get; set; }
        public double? LikesGrowthRate { get; set; }
        public double? FollowersChange { get; set; }
        public double? LikesChange { get; set; }
        public double? FollowersAverageChange { get; set; }
        public double? LikesAverageChange { get; set; }
    }

    public class Intervals
    {
        public string FollAccSearchTags { get; set; }
        public string UnFoll16DaysEngage { get; set; }
        public string LikeFollowingPosts { get; set; }
        public string VwStoriesFollowing { get; set; }
        public string CommFollowingPosts { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
    }


    public partial class SocialProfile_ActionsViewModel
    {
        public long SPSHId { get; set; }
        public Nullable<int> SocialProfileId { get; set; }
        public Nullable<int> ActionID { get; set; }

        public string Action { get; set; }
        public string TargetProfile { get; set; }
        public Nullable<System.DateTime> ActionDateTime { get; set; }
        public string Message { get; set; }

       
    }
}
