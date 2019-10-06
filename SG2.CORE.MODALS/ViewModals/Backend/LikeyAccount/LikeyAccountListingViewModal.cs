using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend.LikeyAccount
{
    public class LikeyAccountListingViewModal
    {

        public int LikeyAccountId { get; set; }
        public string InstaUserName { get; set; }
        public string Status { get; set; }
        public Nullable<int> TotalRecord { get; set; }
        public int PageNumber { get; set; }
    }
}
