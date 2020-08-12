using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Home
{
    public class IndexViewModel
    {
        public SignupWizardViewModel SignupWizardViewModel { get; set; }
        public string StripeApikey { get; set; }
    }


    public class contactFormViewModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string message { get; set; }
    }
}
