using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.MobileViewModels
{
    public class MobileLoginViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Pin { get; set; }

        [Required]
        public string IMEI { get; set; }

        [Required]
        public bool ForceSwitchDevice { get; set; }



    }


    public class MobileManifestRequest
    {
        [Required]
        public string SocialProfileId { get; set; }
        
        public string SocialPassword { get; set; }
       



    }
}
