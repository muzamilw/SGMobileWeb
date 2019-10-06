using SG2.CORE.MODAL.ViewModals.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Customers
{
    public class CustomerPlanDetailViewModel
    {
        public List<CustomerPaymentPlansViewModel> PaymentPlans { get; set; }
        public List<CustomerPaymentCardsViewModel> PaymentCards { get; set; }

        public IList<CustomerOrderHistoryViewModel> OrderHistoryViewModels { get; set; }

    }
}
