using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.MobileViewModels
{
    public class MobileLoginRequest
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

    public class MobileLoginResponse
    {
        public int SocialProfileId { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }

        public bool SocialPasswordNeeded { get; set; }
    }


    public class MobileManifestResponse
    {
        public int CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public long LicenseExpiredDateUTC { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }

        public SocialProfile Profile { get; set; }

        public SocialProfile_Instagram_TargetingInformation TargetInformation { get; set; }
    }


    public class MobileManifestRequest
    {
        [Required]
        public int SocialProfileId { get; set; }
        
        public string SocialPassword { get; set; }

    }
}
