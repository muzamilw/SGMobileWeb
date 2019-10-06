using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.VPSSupplier
{
   public class VPSSupplierListingDTO
    {
        public int ProxyId { get; set; }
        public string ProxyIPNumber { get; set; }
        public int? NoOfFreeSlots { get; set; }
        public string JVBox { get; set; }
        public string Region { get; set; }
        public Nullable<int> TotalRecord { get; set; }
        public int PageNumber { get; set; }
        public string ProxyStatus { get; set; }
        public int ? VPSSId { get; set; }
        public string VPSSName { get; set; }

    }
}
