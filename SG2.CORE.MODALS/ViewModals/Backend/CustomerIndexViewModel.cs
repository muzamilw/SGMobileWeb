using SG2.CORE.MODAL.DTO.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend
{
    public class CustomerIndexViewModel
    {
        public IEnumerable<CustomerListingViewModel> CustomerListing { get; set; }

        public string SearchCriteria { get; set; }

        public IList<StatusDTO> ApplicationStatuses{get; set;}

        public IList<ProductDTO> ProductIds { get; set; }

        public int TotalRecord { get; set; }
        public int? StatusId { get; set; }
        public int PageNumber { get; set; }
        public string ProductId { get; set; }
        public string JVStatus { get; set; }

        public int? Subscription { get; set; }

        public int? profileType { get; set; }



    }
}
