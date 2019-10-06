using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend.JVBox
{
    public class JVBoxListingViewModal
    {
        public int JVBoxId { get; set; }
        public string BoxName { get; set; }
        public int LiveUser { get; set; }
        public string Status { get; set; }
        public Nullable<int> TotalRecord { get; set; }
        public int PageNumber { get; set; }

        public int? JVBoxMaxLimit { get; set; }

        public int? ServerRunningStatusId { get; set; }
    }
}
