using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Customers
{
    public class CustomerProfileViewModel
    {
        public CustomerProfileUpdateViewModel CustomerProfileUpdateVM { get; set; }
        public CustomerUpdatePasswordViewModel CustomerUpdatePasswordVM { get; set; }
        public bool IsOptedEducationalEmailSeries { get; set; }
        public bool IsOptedMarketingEmail { get; set; }

    }

    public class CustomerAddProfileRequest
    {
        public String IntagramUserName { get; set; }
        public int customerId { get; set; }
      

    }

    public class CustomerBrokerProfileRequest
    {
        public string BrokerLogo { get; set; }
        public string BrokerAppName { get; set; }
        public string BrokerStrapLine { get; set; }
        public string BrokerAspectColor { get; set; }
        public string BrokerTrainingLink { get; set; }
        public string BrokerTermsOfUse { get; set; }
        public string BrokerPrivacyPolicy { get; set; }
        public string BrokerFeedbackPage { get; set; }
        public string BrokerHomePage { get; set; }
        public string BrokerTrustPilotCode { get; set; }
        public int cid { get; set; }


    }


    public class ProfilesSearchRequest
    {
        public String searchString { get; set; }
        public int SocialType { get; set; }

        public int Block { get; set; }

        public int Plan { get; set; }


    }
}
