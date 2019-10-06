using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend.VPSSupplier
{
   public class VPSSupplierListingViewModel
    {

        public int? VPSSId { get; set; }

        public int NoOfAssignedIPs { get; set; }
        public string IssuingISPName { get; set; }
        public string Status { get; set; }
        public int? PageSize { get; set; }
    }
}
