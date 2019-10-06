using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend.Proxy
{
   public class BadProxyViewModel
    {
        public int? ProxyId { get; set; }
        public int profileId { get; set; }
        public string Profiles { get; set; }
        public string ProxyIP { get; set; }
        public Nullable<int> TotalRecord { get; set; }

    }
}
