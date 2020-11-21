using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace SG2.CORE.MODAL.ViewModals.Customers
{
    public class CustomerLoginViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }


    public class CustomerAutoLoginViewModel
    {
        public string EmailAddress { get; set; }

    }
}
