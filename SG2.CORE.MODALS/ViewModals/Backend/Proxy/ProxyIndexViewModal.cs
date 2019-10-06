using SG2.CORE.MODAL.DTO.Common;
using SG2.CORE.MODAL.DTO.Customers;
using System.Collections.Generic;

namespace SG2.CORE.MODAL.ViewModals.Backend.Proxy
{
    public class ProxyIndexViewModal
    {

        public IEnumerable<ProxyListingViewModal> ProxyListing { get; set; }
        public string SearchCriteria { get; set; }
        public IList<StatusDTO> ApplicationStatuses { get; set; }
        public IList<VPSSDTO> vPSSDTOs { get; set; }
        public int TotalRecord { get; set; }
        public int? StatusId { get; set; }

        public int? VPSSId { get; set; }
        public int PageNumber { get; set; }
    }
}
