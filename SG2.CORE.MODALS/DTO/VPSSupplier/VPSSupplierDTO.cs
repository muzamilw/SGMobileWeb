using SG2.CORE.MODAL.ViewModals.Backend.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.VPSSupplier
{
   public class VPSSupplierDTO
    {
        public int VPSSId { get; set; }
        public string IPManageBy { get; set; }
        public string SupportPhone { get; set; }
        public string SupportEmail { get; set; }
        public string IssuingISPName { get; set; }
        public string IssuingISPPhone { get; set; }
        public string IssuingISPWebsite { get; set; }
        public string IssuingISPAccount { get; set; }
        public string IssuingISPPassword { get; set; }
        public string IssuingISPMemo { get; set; }
        public int? StatusId { get; set; }

        public IList<VPSSupplierListingDTO> VPSSList { get; set; }
    }
}
