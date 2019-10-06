using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend.SystemSettings
{
    public class SystemSettingsListingViewModal
    {
        public short ConfigId { get; set; }
        public string ConfigKey { get; set; }
        public Nullable<int> TotalRecord { get; set; }
        public int PageNumber { get; set; }
    }
}
