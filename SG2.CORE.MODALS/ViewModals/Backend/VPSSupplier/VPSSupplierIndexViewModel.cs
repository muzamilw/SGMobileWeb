using SG2.CORE.MODAL.DTO.Customers;
using SG2.CORE.MODAL.ViewModals.Backend.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend.VPSSupplier
{
  public  class VPSSupplierIndexViewModel
    {
        public IEnumerable<VPSSupplierListingViewModel> VPSSupplierListingViewModel { get; set; }

        public string SearchCriteria { get; set; }
        public IList<StatusDTO> ApplicationStatuses { get; set; }
        public int TotalRecord { get; set; }
        public int? StatusId { get; set; }
        public int PageNumber { get; set; }
    }
}
