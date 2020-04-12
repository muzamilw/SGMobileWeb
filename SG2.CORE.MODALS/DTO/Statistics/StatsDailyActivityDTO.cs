using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.Statistics
{
    public class StatsDailyActivityDTO
    {
        public DateTime Date { get; set; }
        public long Followers { get; set; }
        public long FollowersDiff { get; set; }
        public long Followings { get; set; }
        public long FollowingsDiff { get; set; }
        public long Posts { get; set; }
        public long PostDiff { get; set; }
        public long EngagmentRate { get; set; }
        public long EngagmentRatePercentage { get; set; }
    }
}
