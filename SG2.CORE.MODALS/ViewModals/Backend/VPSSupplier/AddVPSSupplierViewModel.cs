using SG2.CORE.MODAL.DTO.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SG2.CORE.MODAL.ViewModals.Backend.VPSSupplier
{
   public class AddVPSSupplierViewModel
    {
        public int VPSSId { get; set; }
        [Required(ErrorMessage = "Issuing ISP Name required.")]
        [Display(Name = "Issuing ISP Name")]
        [Remote("IsSupplierExist", "VPSSupplier", ErrorMessage = "Supplier already exist.", AdditionalFields = "VPSSId")]
        public string IssuingISPName { get; set; }

        [Required(ErrorMessage = "ISP Phone # required.")]
        [Display(Name = "ISP Phone #")]
        public string IssuingISPPhone { get; set; }

        [Required(ErrorMessage = "ISP Website required.")]
        [Display(Name = "ISP Website")]
        public string IssuingISPWebsite { get; set; }

        [Required(ErrorMessage = "ISP Account # required.")]
        [Display(Name = "ISP Account #")]
        public string IssuingISPAccount { get; set; }

        [Required(ErrorMessage = "ISP Password required.")]
        [Display(Name = "ISP Password")]
        public string IssuingISPPassword { get; set; }

        [Required(ErrorMessage = "ISP / VPS Memo required.")]
        [Display(Name = "ISP / VPS Memo")]
        public string IssuingISPMemo { get; set; }

        [Required(ErrorMessage = "IP managed by required.")]
        [Display(Name = "IP managed by (IT support)")]
        public string IPManageBy { get; set; }

        [Required(ErrorMessage = "Support Phone # required.")]
        [Display(Name = "Support Phone #")]
        public string SupportPhone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Support Email address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter valid email address.")]
        [RegularExpression(@"^[\w-\._\+%]+@(?:[\w-]+\.)+[\w]{2,6}$", ErrorMessage = "Please enter a valid email address")]
        public string SupportEmail { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public int StatusId { get; set; }

        public IList<StatusDTO> Statuses { get; set; }
    }
}
