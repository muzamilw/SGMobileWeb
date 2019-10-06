using SG2.CORE.MODAL.DTO.TeamMember;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SG2.CORE.MODAL.ViewModals.Backend.TeamMember
{
    public class TeamMemberProfileUpdateViewModal
    {
        public TeamMemberProfileDataViewModel TMProfileUpdateVM { get; set; }
        public TeamMemberPasswordDataViewModel TMPasswordVM { get; set; }

       
    }
}
