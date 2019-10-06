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

        public IEnumerable<StatisticsDTO> StatisticsListing { get; set; }

        public Int64? TotalFollowers { get; set; }
        public Int64? TotalFollowersGain { get; set; }
        public Int64? TotalFollowings { get; set; }
        public Int64? TotalFollowingsRatio { get; set; }
        public Int64? TotalLike { get; set; }
        public Int64? TotalLikeComment { get; set; }
		public Int64? TotalComment { get; set; }
        public Int64?  TotalEngagement { get; set; }
    }
}
