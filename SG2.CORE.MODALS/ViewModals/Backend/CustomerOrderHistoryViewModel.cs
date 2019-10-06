using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend
{
   public class CustomerOrderHistoryViewModel
    {
        public string Name{ get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }

        public decimal? Price { get; set; }

        public string Status { get; set; }
        public string Id { get; set; }

        public string SPId { get; set; }
        public string SPName { get; set; }
        public string  Email { get; set; }
        public string  JVStatus { get; set; }
        public string  SPStatus { get; set; }
        public string  SusrName { get; set; }
        public string  UserName { get; set; }
        public int? TotalRecords { get; set; }
    }
}
