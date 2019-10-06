using SG2.CORE.MODAL.DTO.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SG2.CORE.MODAL.ViewModals.Home
{
    public class SignupWizardViewModel
    {
        [Required(ErrorMessage = "Hastag required")]
        [Display(Name = "Choose the hashtags you want to engage with: ")]
        public string Preference1 { get; set; }

        [Display(Name = "Choose the hashtags you DO NOT want to engage with (optional):")]
        public string Preference2 { get; set; }

        [Display(Name = "Choose up to 8 locations to focus on. Cities work best(optional):")]
        public string Preference3 { get; set; }

        [Required(ErrorMessage = "Prefer location required")]
        [Display(Name = "Which city do you mostly login from?:")]
        public string CityId { get; set; }

        [Required(ErrorMessage = "Please select any option.")]
        [Display(Name = "A very effective growth strategy is to incorporate an amount of Follow & Unfollow activities on target accounts. Do you permit this activity?")]
        public int? Preference5 { get; set; }

        [Required(ErrorMessage = "Please select any option.")]
        [Display(Name = "Do you want to engage with Males, Females or Both")]
        public int? Preference6 { get; set; }

        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Your Instagram Username:")]
        public string InstaUser { get; set; }

        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Instagram Password:")]
        [DataType(DataType.Password)]
        public string InstaPassword { get; set; }
        

        //-- Sign up model --//
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "Should not greater then 50 characters.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        //[Required(ErrorMessage = "Phone Number is Required")]
        [StringLength(50, ErrorMessage = "Maximum 10 charactars allowed.")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Last name is Required")]
        [StringLength(50, ErrorMessage = "Maximum 50 charactars allowed.")]
        [Display(Name = "Last name")]
        public string Lastname { get; set; }

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

        [DataType(DataType.CreditCard)]
        public string CardMumber { get; set; }
        public string CVC { get; set; }      
        public short Year { get; set; }
        public short Month { get; set; }
        public string stripeToken { get; set; }

        public IList<CitiesAndCountriesDTO> Cities { get; set; }
    }
}
