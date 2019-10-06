using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Customers
{
    public class CustomerPayAndConfirmViewModel
    {
        public List<CustomerPaymentCardsViewModel> CardDetails { get; set; }
        public CustomerPaymentPlansViewModel PaymentPlan { get; set; }

        public string PlanId { get; set; }

    }
}
