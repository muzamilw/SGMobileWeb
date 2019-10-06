using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.MobileViewModels
{
   
    public class MobileLoginJsonRootObject
    {
        public int CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string DeviceId { get; set; }
        public string DeviceEMEI { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public List<Platform> Platforms { get; set; }
    }
}
