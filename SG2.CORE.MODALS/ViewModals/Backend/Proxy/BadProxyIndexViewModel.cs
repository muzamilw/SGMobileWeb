using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend.Proxy
{
    public class BadProxyIndexViewModel
    {
        public IEnumerable<BadProxyViewModel> BadProxyListing { get; set; }
        public int TotalRecord { get; set; }
        public int PageNumber { get; set; }
    }
}
