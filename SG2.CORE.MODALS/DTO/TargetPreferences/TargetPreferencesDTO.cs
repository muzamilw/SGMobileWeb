using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.TargetPreferences
{
    public class TargetPreferencesDTO
    {
        public int Id { get; set; }
        public string ProfileName { get; set; }
        public int? SocialProfileId { get; set; }
        public int CustomerId { get; set; }
        public int JVServerId { get; set; }
        public string Preference1 { get; set; }
        public string Preference2 { get; set; }
        public string Preference3 { get; set; }
        public string Preference4 { get; set; }
        public int? Preference5 { get; set; }
        public int? Preference6 { get; set; }
        public int? Preference7 { get; set; }
        public string Preference8 { get; set; }
        public string Preference9 { get; set; }
        public int? Preference10 { get; set; }
        public string InstaUser { get; set; }
        public string InstaPassword { get; set; }
        public int? Country { get; set; }
        public int? City { get; set; }
        public Int16? QueueStatusId { get; set; }
        public string JvBoxExchangeName { get; set; }

        public int? SocialAccAs { get; set; }

    }
}
