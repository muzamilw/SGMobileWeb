using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SG2.CORE.MODAL.ViewModals.Customers
{
    public class CustomerSignUpViewModel
    {
        public int CustomerId { get; set; }
        public string GUID { get; set; }

        [Required(ErrorMessage = "First InstaUsrName is Required")]
        [StringLength(50, ErrorMessage = "Should not greater then 50 characters.")]
        [Display(Name = "First InstaUsrName")]
        public string FirstName { get; set; }

        //[Required(ErrorMessage = "Phone Number is Required")]
        [StringLength(50, ErrorMessage = "Maximum 10 charactars allowed.")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Instagram username is Required")]
        [Display(Name = "Intsagram username")]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "SurName is Required")]
        [StringLength(50, ErrorMessage = "Maximum 50 charactars allowed.")]
        [Display(Name = "Last InstaUsrName")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(50, ErrorMessage = "Should not greater then 50 characters.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter valid email address.")]
        [Remote("IsEmailExist", "Account", ErrorMessage = "Email already exist.")]
        [RegularExpression(@"^[\w-\._\+%]+@(?:[\w-]+\.)+[\w]{2,6}$", ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password not matched.")]
        [Required(ErrorMessage = "Confirm password is required.")]
        public string Re_Password { get; set; }
    }
}
