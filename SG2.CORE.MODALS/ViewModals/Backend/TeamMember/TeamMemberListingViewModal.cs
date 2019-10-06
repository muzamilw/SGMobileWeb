using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend.TeamMember
{
    public class TeamMemberListingViewModal
    {
        public int SystemUserId { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public Nullable<int> TotalRecord { get; set; }
        public int PageNumber { get; set; }
    }
}
