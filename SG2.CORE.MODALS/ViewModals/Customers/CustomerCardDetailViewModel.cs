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

    public class NewSubscriptionRequestModel
    {
        public string email { get; set; }
        public string paymentmethod { get; set; }
        public int selectedPlanId { get; set; }
        public int customerid { get; set; }
        public int socialProfileId { get; set; }
     
    }
}
