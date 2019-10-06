using SG2.CORE.MODAL.DTO.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend.JVBox
{
    public class JVBoxIndexViewModal
    {
        public IEnumerable<JVBoxListingViewModal> JVBoxListing { get; set; }
        public string SearchCriteria { get; set; }
        public IList<StatusDTO> ApplicationStatuses { get; set; }
        public int TotalRecord { get; set; }
        public int? StatusId { get; set; }
        public int PageNumber { get; set; }
    }
}
