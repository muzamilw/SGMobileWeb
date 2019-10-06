using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.klaviyo
{

    public class KlaviyoListSubscription
    {
        public string api_key { get; set; }

        public List<KlaviyoProfile> profiles { get; set; }
    }

}
