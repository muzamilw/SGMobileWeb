using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend.ActionBoard
{
    public class ActionBoardListingViewModel
    {
        public string UserName { get; set; }

        public string CustomerId { get; set; }

        public string ProfileId { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string InstaUsrName { get; set; }

        public string Comment { get; set; }

        public bool IsArchived { get; set; }

        public short EnumerationValueId { get; set; }
    }
}
