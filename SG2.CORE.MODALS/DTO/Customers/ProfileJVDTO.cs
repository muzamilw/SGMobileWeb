using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.Customers
{
    public class ProfileJVDTO
    {
        public int JVAttempts { get; set; }
        public DateTime? JVAttemptsBlockedTill { get; set; }
        public int JVAttemptStatus { get; set; }
        public int ProfileId { get; set; }
    }
}
