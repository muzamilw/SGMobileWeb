using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend.TeamMember
{
    public class SuperAdminViewModal
    {

        [Required]
        [StringLength(50, ErrorMessage = "Should not greater then 50 characters.")]
        [RegularExpression(@"^[\w-\._\+%]+@(?:[\w-]+\.)+[\w]{2,6}$", ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email")]
       
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter valid email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password, ErrorMessage = "Please enter valid Password")]
       

        public string Password { get; set; }
    }
}
