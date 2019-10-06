using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Statistics
{
    public class FollowersStatisticsListViewModel
    {
        public int SocialProfileId { get; set; }
        public string Username { get; set; }
        public DateTime Date { get; set; }
        public Int64? FollowersGain { get; set; }
        public Int64? Followers { get; set; }
        public Int64? Followings { get; set; }
        public Int64? AVGFollowers { get; set; }
        public decimal? FollowingsRatio { get; set; }
        public string WeekDays { get; set; }
    }
}
