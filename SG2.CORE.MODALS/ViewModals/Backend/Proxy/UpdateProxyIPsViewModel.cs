using SG2.CORE.MODAL.DTO.Common;
using SG2.CORE.MODAL.DTO.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SG2.CORE.MODAL.ViewModals.Backend.Proxy
{
    public class UpdateProxyIPsViewModel
    {
        public int ProxyId { get; set; }

        [Remote("IsProxyIPNumberExists", "ProxyIPs", ErrorMessage = "ProxyIPNumber already exist.", AdditionalFields = "ProxyId")]
        public String ProxyIPNumber { get; set; }

        [Required]
        [Display(Name = "Proxy")]
        public string ProxyIPName { get; set; }

        [Required]
        [Display(Name = "Base City")]
        public string BaseCity { get; set; }

        [Required]
        [Display(Name = "Base Country")]
        public string BaseCountry { get; set; }

        [Required]
        [Display(Name = "Geo points")]
        public string GeoPoints { get; set; }

        [Required]
        [Display(Name = "Issuing ISP Name")]
        public int? IssuingISPNameId { get; set; }

        public string ProxyPort { get; set; }

        

        public IList<CustomerDTO> Customers { get; set; }

        public string AssignedCustomerID1 { get; set; }

        public string AssignedCustomerID2 { get; set; }

        public string AssignedCustomerID3 { get; set; }

        public string AssignedCustomerID1City { get; set; }

        public string AssignedCustomerID2City { get; set; }

        public string AssignedCustomerID3City { get; set; }

        public IList<StatusDTO> StatusIds { get; set; }

        [Required]
        public int? StatusId { get; set; }

        [Required]
        [StringLength(3, ErrorMessage = "Maximum 3 charactars allowed.")]
        public string P1 { get; set; }

        [Required]
        [StringLength(3, ErrorMessage = "Maximum 3 charactars allowed.")]
        public string P2 { get; set; }

        [Required]
        [StringLength(3, ErrorMessage = "Maximum 3 charactars allowed.")]
        public string P3 { get; set; }

        [Required]
        [StringLength(3, ErrorMessage = "Maximum 3 charactars allowed.")]
        public string P4 { get; set; }

        public IList<CountryDTO> Countries { get; set; }

        public IList<CityDTO> Cities { get; set; }

        public IList<VPSSDTO> vPSSDTOs { get; set; }
    }
}
