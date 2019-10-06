using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend.SystemSettings
{
    public class SystemSettingsViewModal
    {
        public short ConfigId { get; set; }

   
        [Display(Name = "Name")]
        public string ConfigKey  { get; set; }

        [Required]
        [Display(Name = "Developer Key")]
        public string ConfigValue { get; set; }

        [Required]
        [Display(Name = "Publications Key")]
        public string ConfigValue2 { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
