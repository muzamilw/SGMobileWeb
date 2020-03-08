using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend
{
    public class CustomerListingViewModel
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string InstaUsrName { get; set; }
        public string Products { get; set; }
        
        public bool BrokerAccount { get; set; }
        public string Status { get; set; }
        public Nullable<int> TotalRecord { get; set; }

        public string JVBoxStatus { get; set; }

        public string CustomerEmail { get; set; }
        public string SocialProfileId { get; set; }
        public string SocialProfileName { get; set; }
        public Nullable<bool> FollowOn { get; set; }
        public Nullable<bool> UnFollFollowersAfterMinDays { get; set; }
        public Nullable<bool> AfterFollLikeuserPosts { get; set; }
        public Nullable<bool> AfterFollViewUserStory { get; set; }

        public string BlockStatus { get; set; }

        public string BrokerAppName { get; set; }
    }
}
