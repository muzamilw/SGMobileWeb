using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.Proxy
{
    public class ProxyDTO
    {

        public int ProxyId { get; set; }

        public string ProxyIPNumber { get; set; }

		 public string ProxyIPName { get; set; }


        public string BaseCity { get; set; }

        public string BaseCountry { get; set; }

        public string GeoPoints { get; set; }
        public int? IssuingISPNameId { get; set; }

        public string AssignedCustomerID1 { get; set; }

        public string AssignedCustomerID2 { get; set; }

        public string AssignedCustomerID3 { get; set; }
        public string AssignedCustomer1City { get; set; }
        public string AssignedCustomer2City { get; set; }
        public string AssignedCustomer3City { get; set; }

        public int? StatusId { get; set; }

        public string ProxyPort { get; set; }
    }
}
