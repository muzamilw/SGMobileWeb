using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.Proxy
{
    public class ProxyMappingDTO
    {
        public int ProxyMappingId { get; set; }
        public int ProxyId { get; set; }
        public int CustomerId { get; set; }

        public string ProxyIPNumber { get; set; }
        public string ProxyPort { get; set; }
        public string ProxyIPName { get; set; }

    }
}
