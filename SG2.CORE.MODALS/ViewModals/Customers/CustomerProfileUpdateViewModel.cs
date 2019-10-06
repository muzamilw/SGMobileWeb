using SG2.CORE.MODAL.DTO.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SG2.CORE.MODAL.ViewModals.Customers
{
    public class CustomerProfileUpdateViewModel
    {
        public int CustomerId { get; set; }

        public string GUID { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        [StringLength(50, ErrorMessage = "Should not greater then 50 characters.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        //[Required(ErrorMessage = "Phone Number is Required")]
        [StringLength(50, ErrorMessage = "Maximum 12 charactars allowed.")]
        [Display(Name = "Phone Number")]        
        public string PhoneNumber { get; set; }

        public string PhoneCode { get; set; }                      

        // [Required(ErrorMessage = "User Name is Required")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [StringLength(50, ErrorMessage = "Maximum 50 charactars allowed.")]
        [Display(Name = "Last name")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(50, ErrorMessage = "Should not greater then 50 characters.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter valid email address.")]
        [RegularExpression(@"^[\w-\._\+%]+@(?:[\w-]+\.)+[\w]{2,6}$", ErrorMessage = "Please enter a valid email address")]
        //[Remote("IsEmailExist", "Account", ErrorMessage = "Email already exist.")]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        public IList<CountryDTO> Countries { get; set; }

    }
}
