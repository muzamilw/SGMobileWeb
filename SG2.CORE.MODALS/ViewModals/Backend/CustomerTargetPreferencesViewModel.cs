using SG2.CORE.MODAL.DTO.Common;
using SG2.CORE.MODAL.DTO.Customers;
using SG2.CORE.MODAL.DTO.JVBox;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SG2.CORE.MODAL.ViewModals.Backend
{
    public class CustomerTargetPreferencesViewModel
    {
        public string Id { get; set; }

        public string SPId { get; set; }

        public int? Status { get; set; }

        public string UserName { get; set; }

        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Choose the hashtags you want to engage with: ")]
        public string Preference1 { get; set; }

        [Display(Name = "Choose the hashtags you DO NOT want to engage with (optional):")]
        public string Preference2 { get; set; }

        [Display(Name = "Choose up to 8 locations to focus on. Cities work best (optional):")]
        public string Preference3 { get; set; }

        [Display(Name = "Choose up to 8 locations to skip (optional):")]
        public string Preference4 { get; set; }
        //[Required(ErrorMessage = "Required")]
        [Display(Name = "A very effective growth strategy is to incorporate an amount of Follow & Unfollow activities on target accounts. Do you permit this activity?")]
        public int? Preference5 { get; set; }
        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Do you want to engage with Males, Females or Both")]
        public int? Preference6 { get; set; }
        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Do you want to include Business accounts?")]
        public int? Preference7 { get; set; }
        [Display(Name = "Targeted User Demographics(optional):")]
        public string Preference8 { get; set; }

        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Direct competitors or similar accounsts. Provide at least  10 accounsts with a following of at least 2000 followers:*")]
        public string Preference9 { get; set; }

        //  [Required(ErrorMessage = "Required")]
        //  [Display(Name = "Turn on AI targeting. We will seek out our competitors and match their hashtags")]
        public int? Preference10 { get; set; }

        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Your Instagram Username:")]
        public string InstaUser { get; set; }
        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Instagram Password:")]
        [DataType(DataType.Password)]
        public string InstaPassword { get; set; }

        [Display(Name = "What country do you mostly log in from?")]
        public int? Country { get; set; }

        [Display(Name = "What city do you mostly log in from?")]

        public string SPStatus { get; set; }
        public string JVStatus { get; set; }

        public string SocialProfileName { get; set; }
        public string Email { get; set; }
        public string CustomerUserName { get; set; }
        public int? City { get; set; }
        public int? NoOfProfile { get; set; }
        public IList<CountryDTO> Countries { get; set; }

        public IList<CityDTO> Cities { get; set; }

        public IList<StatusDTO> JarveeStatuses { get; set; }

        public List<JVBoxDTO> MPBoxList { get; set; }

        public List<ProxyIPDTO> ProxyIPs { get; set; }
        public int? ProxyIP { get; set; }

        public string JVName { get; set; }
        public string IP { get; set; }
        [Display(Name = "I want to grow my instagram account as")]
        public int SocialAccAS { get; set; }
        public string VerificationCode { get; set; }

        public int? MPBox { get; set; }

        public string Notes { get; set; }
        public int? ProxyId { get; set; }

        public string ProxyPort { get; set; }

        public bool IsArchived { get; set; }
    }
}
