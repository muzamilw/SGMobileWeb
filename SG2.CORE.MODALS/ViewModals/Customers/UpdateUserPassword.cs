using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SG2.CORE.MODAL.ViewModals.Customers
{
    public class CustomerUpdatePasswordViewModel
    {
        public int CustomerId { get; set; }
        public string GUID { get; set; }

        [Display(Name ="Current Password")]
        [Required(ErrorMessage = "Current password required.")]
        [Remote("CheckCurrentPassword", "User", ErrorMessage = "Current password is not correct.")]
        public string CurrentPassword { get; set; }
        
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "New password is required.")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password not matched.")]
        [Required(ErrorMessage = "Confirm password is required.")]
        public string Confirm_Password { get; set; }
               

    }
}
