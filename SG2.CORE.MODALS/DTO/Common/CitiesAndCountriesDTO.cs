using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.DTO.Common
{
   public class CitiesAndCountriesDTO
    {
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public string CityName { get; set; }
        public string CountyCityName { get; set; }
    }
}
