using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Customers
{
    public class CustomerPaymentPlansViewModel
    {

        public string PlanId { get; set; }
        public bool PlanStatus { get; set; }
        public string AggregateUsage { get; set; }
        public long Amount { get; set; }

        public string BillingScheme { get; set; }

        public string Created { get; set; }

        public string Currency { get; set; }

        public string Interval { get; set; }

        public long IntervalCount { get; set; }

        public bool LiveMode { get; set; }

        public string MetaData { get; set; }

        public string PlanName { get; set; }
        public string ProductCode { get; set; }

        public string Tiers { get; set; }
        public string TiersMode { get; set; }

        public string Transform_usage { get; set; }

        public int TrialPeriodDays { get; set; }

        public int UsageType { get; set; }

        public bool currentPlan { get; set; }
    }
}
