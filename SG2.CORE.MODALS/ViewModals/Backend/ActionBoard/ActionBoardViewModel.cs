using SG2.CORE.MODAL.DTO.Common;

using SG2.CORE.MODAL.ViewModals.Backend.ActionBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Backend.ActionBoard
{
    public class ActionBoardViewModel
    {
        public List<ActionBoardJVSData> JVStatuses { get; set; }
       
        public IList<CountryDTO> Countries { get; set; }
        public List<ProxyIPDTO> ProxyIPs { get; set; }
        public IList<CityDTO> Cities { get; set; }

    }
}
