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
        public Int64? LikesRecent { get; set; }

        public Int64? UnFollowTotal { get; set; }
        public Int64? UnFollowRecent { get; set; }

        public Int64? StoryViewsTotal { get; set; }
        public Int64? StoryViewsRecent { get; set; }

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
