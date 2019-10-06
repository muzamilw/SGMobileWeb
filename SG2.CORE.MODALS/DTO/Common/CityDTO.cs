using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.Common
{
    public  class CityDTO
    {
        public int CityId { get; set; }
        public short CountryId { get; set; }
        public Nullable<short> StateId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Nullable<short> StatusId { get; set; }
    }
}
