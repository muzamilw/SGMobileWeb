using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.TeamMember
{
    public class SystemUserDTO
    {
        public int SystemUserId { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public short SystemRoleId { get; set; }

        public string Password { get; set; }

        public short StatusId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        public bool HostUser { get; set; }

        public string UserRoleName { get; set; }

        public string UserName { get; set; }

    }
}
