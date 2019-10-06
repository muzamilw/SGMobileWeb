using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.Common
{
    public  class CountryDTO
    {
        public short CountryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string PhoneCode { get; set; }
        public Nullable<short> StatusId { get; set; }
    }

}
