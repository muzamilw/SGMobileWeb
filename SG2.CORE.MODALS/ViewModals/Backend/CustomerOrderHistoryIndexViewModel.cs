using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend
{
   public class CustomerOrderHistoryIndexViewModel
    {
        public IEnumerable<CustomerOrderHistoryViewModel> CustomerOrderHListing { get; set; }

        public string Id { get; set; }
        public string  SocialProfileId { get; set; }
        public int TotalRecord { get; set; }
        public int PageNumber { get; set; }
        public string SPName { get; set; }
        public string Email { get; set; }
        public string JVStatus { get; set; }
        public string SPStatus { get; set; }
        public string SusrName { get; set; }
        public string UserName { get; set; }

        public string SocialProfileName { get; set; }
        public string InstaUsrName { get; set; }
        public string Status { get; set; }

    }
}
