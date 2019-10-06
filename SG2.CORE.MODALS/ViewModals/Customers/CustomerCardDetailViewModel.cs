using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Customers
{
    public class CustomerCardDetailViewModel
    {
        [DataType(DataType.CreditCard)]
        public string CardMumber { get; set; }
        public string CVC { get; set; }
        public short Year { get; set; }
        public short Month { get; set; }
        public string StripePlanId { get; set; }
        public string stripeToken { get; set; }
        public int SocialProfileId { get; set; }
    }
}
