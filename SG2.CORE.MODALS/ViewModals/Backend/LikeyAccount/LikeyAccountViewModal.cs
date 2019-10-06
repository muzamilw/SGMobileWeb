using SG2.CORE.MODAL.DTO.Common;
using SG2.CORE.MODAL.DTO.Customers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SG2.CORE.MODAL.ViewModals.Backend.LikeyAccount
{
    public class LikeyAccountViewModal
    {
        public int LikeyAccountId { get; set; }

        [Required]
        [Display(Name = "Insta Username")]
        [Remote("IsInstaUserNameExists", "LikeAccts", ErrorMessage = "Insta User Name already exist.", AdditionalFields = "LikeyAccountId")]
        public string InstaUserName { get; set; }

        [Required]
        [Display(Name = "Insta Password")]
        public string InstaPassword { get; set; }
        
        [Display(Name = "Country")]
        [Required(ErrorMessage = "This field is Required. ")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        public short Gender { get; set; }

        [Required]
        public string HashTag { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        public IList<CountryDTO> Countries { get; set; }
        public IList<CityDTO> Cities { get; set; }

        public IList<StatusDTO> Statuses { get; set; }

    }
}
