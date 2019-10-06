using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.LikeyAccount
{
    public class LikeyAccountDTO
    {
        public int LikeyAccountId { get; set; }
        public string InstaUserName { get; set; }
        public string InstaPassword { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public short Gender { get; set; }
        public string HashTag { get; set; }
        public int StatusId { get; set; }

    }
}
