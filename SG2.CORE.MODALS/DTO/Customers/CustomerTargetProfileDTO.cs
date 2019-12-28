using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.Customers
{
    public class SocialProfileDTO
    {

        public SocialProfile SocialProfile { get; set; }

        public SocialProfile_Instagram_TargetingInformation SocialProfile_Instagram_TargetingInformation { get; set; }

        public PaymentPlan CurrentPaymentPlan { get; set; }

        public List<SocialProfile_PaymentsDTO> LastSocialProfile_Payments { get; set; }

        public List<SocialProfile_FollowedAccounts> SocialProfile_FollowedAccounts { get; set; }
		public Customer socialcustomer { get; set; }
	}
}
