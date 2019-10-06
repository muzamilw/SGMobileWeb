using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SG2.CORE.MODAL.ViewModals.Backend.TeamMember
{
    public class TeamMemberPasswordDataViewModel
    {
        public int SystemUserId { get; set; }

        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "New password is required.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "Minimum eight characters, at least one letter, one number and one special character required")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password not matched.")]
        [Required(ErrorMessage = "Confirm password is required.")]
        public string Confirm_Password { get; set; }

        //[Display(Name = "Current Password")]
        //[Required(ErrorMessage = "Current password required.")]
        //[Remote("CheckCurrentPassword", "UserProfile", ErrorMessage = "Current password is not correct.", AdditionalFields = "VerifyPasswordValidation") ]
        ////[Remote("CheckCurrentPassword", "User", ErrorMessage = "Current password is not correct.")]
        //public string CurrentPassword { get; set; }

        public string VerifyPasswordValidation { get; set; }
    }
}
