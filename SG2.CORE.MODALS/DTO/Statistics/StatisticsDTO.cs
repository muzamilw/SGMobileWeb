using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.Statistics
{
    public class StatisticsDTO
    {

       public  Int64 SocialStatisticsId { get; set; }
       public  int SocialProfileId { get; set; }
       public  string Username { get; set; }
        public  DateTime Date { get; set; }
        public  Int64? FollowersGain { get; set; }
        public  Int64? Followers { get; set; }
        public  Int64? Followings { get; set; }
        public  decimal? FollowingsRatio { get; set; }
        public  Int64 Joiner { get; set; }
        public  Int64 Unjoiner { get; set; }
        public  Int64 Follow { get; set; }
        public  Int64 Unfollow { get; set; }
        public  Int64 ContactMassage { get; set; }
       
        public  Int64 REPinTweetBlog { get; set; }
        public  Int64 Bump { get; set; }
        public  Int64? Like { get; set; }
         public  Int64? Comment { get; set; }
        public  Int64? Engagement { get; set; }
        public  Int64 Repost { get; set; }
        public  Int64? LikeComments { get; set; }
        public  Int64 StoryViewer { get; set; }
        public  Int64 BlockedFollowers { get; set; }
        public  DateTime CreatedDate { get; set; }
        public  DateTime UpdateDate { get; set; }

        public string Monthly { get; set; }
        public string WeekDays { get; set; }

        public Int64? AVGFollowers { get; set; }

       

    }
}
