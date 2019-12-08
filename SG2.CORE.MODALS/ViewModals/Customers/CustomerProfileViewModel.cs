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
        public int CustomerId { get; set; }
      

    }


    public class ProfilesSearchRequest
    {
        public String searchString { get; set; }
        public int SocialType { get; set; }

        public int Block { get; set; }

        public int Plan { get; set; }


    }
}
