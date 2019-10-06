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
    public class AddTeamMemberViewModal
    {
        public IEnumerable<SystemRoleDTO> RoleListing { get; set; }
        public int SystemUserId { get; set; }

        
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required]
        [StringLength(50, ErrorMessage = "Should not greater then 50 characters.")]
        [Remote("IsEmailExist", "TeamMember", ErrorMessage = "Email already exist.")]
        [RegularExpression(@"^[\w-\._\+%]+@(?:[\w-]+\.)+[\w]{2,6}$", ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter valid email")]
        public string Email { get; set; }

        //[Required]
        [Display(Name = "Role")]
        public short SystemRoleId { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "Minimum eight characters, at least one letter, one number and one special character required")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password not matched.")]
        [Required(ErrorMessage = "Confirm password is required.")]
        public string Confirm_Password { get; set; }

        [Required]
        [Display(Name = "Status")]
        public short StatusId { get; set; }
        //public DateTime CreatedOn { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTime ModifiedOn { get; set; }
        //public string ModifiedBy { get; set; }
        //public bool HostUser { get; set; }

    }
}
