using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Customers
{
   public class CustomerPaymentCardsViewModel
    {
        public string CardId {get; set; }
        public string Brand {get; set; }
        public string Funding {get; set; }
        public string Last4 {get; set; }
        public long ExpMonth {get; set; }
        public long ExpYear {get; set; }
        public string Cvc {get; set; }
        public string Description {get; set; }
        public string SourceToken {get; set; }
        public string CardNumber {get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PaymentMethodId { get; set; }  

    }

}
