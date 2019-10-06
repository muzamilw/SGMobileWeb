using SG2.CORE.DAL.Repositories;
using SG2.CORE.MODAL.DTO.VPSSupplier;
using SG2.CORE.MODAL.ViewModals.Backend.VPSSupplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.BAL.Managers
{
    public class VPSSupplierManager
    {
        private readonly VPSSupplierRepository _vPSSupplierRepository;
        public VPSSupplierManager()
        {
            _vPSSupplierRepository = new VPSSupplierRepository();
        }

        public bool ImportCSV(List<VPSSCSVRecordViewModel> vPSSCSVRecordViewModels)
        {
            var impCSV = _vPSSupplierRepository.ImportCSV(vPSSCSVRecordViewModels);

            return impCSV;
        }

        public bool DeleteSuppliers(int VPSId)
        {
            try
            {
                var ProxyIp = _vPSSupplierRepository.DeleteSuppliers(VPSId);
                return ProxyIp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public VPSSupplierDTO AddUpdateVPSSupplier(VPSSupplierDTO model)
        {
            try
            {
                var VPSS = _vPSSupplierRepository.AddUpdateVPSSupplier(model);
                return VPSS;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public VPSSupplierDTO GetVPSSupplier(int id)
        {

            var VPSS = _vPSSupplierRepository.GetSupplierDTO(id);
            return VPSS;

        }

        public IList<VPSSupplierListingViewModel> GetSupplierData(string SearchCriteria, string PageSize, int PageNumber, int? statusId)
        {
            var Model = _vPSSupplierRepository.GetSupplierData(SearchCriteria, PageNumber, PageSize, statusId);

            return Model;
        }

        public List<GeoPointsDTO> GetGeoPoints()
        {
            var Model = _vPSSupplierRepository.GetGeoPoints();

            return Model;
        }

        public string  SaveGeoPoints(List<GeoPointsDTO> geoPointsDTOs)
        {
            var Model = _vPSSupplierRepository.SaveGeoPoints(geoPointsDTOs);

            return Model;
        }
        

        public bool IsSupplierExist(string IssuingISPName, int VPSSId)
        {
            try
            {
                return _vPSSupplierRepository.IsSupplierExist(IssuingISPName, VPSSId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        

    }
}
