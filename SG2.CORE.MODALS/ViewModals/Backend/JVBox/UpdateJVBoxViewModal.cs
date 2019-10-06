using SG2.CORE.MODAL.DTO.Common;
using SG2.CORE.MODAL.DTO.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SG2.CORE.MODAL.ViewModals.Backend.JVBox
{
    public class UpdateJVBoxViewModal
    {
        public int JVBoxId { get; set; }

        [Required]
        [Display(Name = "Box Name")]
        [Remote("IsJVBoxExists", "JVBoxes", ErrorMessage = "Box Name already exist.", AdditionalFields = "JVBoxId")]
        public string BoxName { get; set; }

        [Required]
        [Display(Name = "Admin Name")]
        public string AdminName { get; set; }

        [Required]
        [Display(Name = "Admin Password")]
        public string AdminPassword { get; set; }

        [Required]
        [Display(Name = "Support Manager")]
        public string BoxManagedBy { get; set; }

        [Required]
        [Display(Name = "Support Phone #")]
        public string SupportPhone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Support Email address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter valid email address.")]
        [RegularExpression(@"^[\w-\._\+%]+@(?:[\w-]+\.)+[\w]{2,6}$", ErrorMessage = "Please enter a valid email address")]
        public string SupportEmail { get; set; }


        [Display(Name = "IP Address")]
        public string IPNumber { get; set; }

        [Required]
        [Display(Name = "Hosted by :")]
        public string HostedBy { get; set; }

        [Required]
        [Display(Name = "Hosting Phone :")]
        public string HostingPhone { get; set; }

        [Required]
        [Display(Name = "Hosting Website :")]
        public string HostingWebsite { get; set; }

        [Required]
        [Display(Name = "Hosting Account :")]
        public string HostingAccount { get; set; }

        [Required]
        [Display(Name = "Hosting Password :")]
        public string HostingPassword { get; set; }


        [Display(Name = "Hosting Price Info / Memo")]
        public string HostingPriceInfo { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }

        [Display(Name = "MPBox Limit")]
        public int JVBoxMaxLimit { get; set; }

        public IList<StatusDTO> Statuses { get; set; }

        public int JVBSRTypeId { get; set; }
        [Required]
        [Display(Name = "MPBox Exchange Name")]
        public string JVBoxExchangeName { get; set; }

        [Display(Name = "Server Running Status")]
        public int ServerRunningStatus { get; set; }


    }
}
