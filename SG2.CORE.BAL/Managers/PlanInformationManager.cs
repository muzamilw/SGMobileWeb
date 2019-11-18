using SG2.CORE.DAL.Repositories;
using SG2.CORE.MODAL.DTO.PlanInformation;
using SG2.CORE.MODAL.ViewModals.Backend.PlanInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.BAL.Managers
{
   public class PlanInformationManager
    {
        private readonly PlanInformationRepository _planInformationRepository;

        public PlanInformationManager()
        {
            _planInformationRepository = new PlanInformationRepository();

        }

        public IList<PlanInformationListingViewModel> GetPlanInformationData(string SearchCriteria, string PageSize, int PageNumber, int? StatusId)
        {
            var Model = _planInformationRepository.GetPlanInformationData(SearchCriteria, PageNumber, PageSize, StatusId);

            return Model;
        }

        public PlanInformationDTO SavePlan(PlanInformationDTO entity)
        {
            try
            {
                _planInformationRepository.SavePlan(entity);
                return entity;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       public IList<PlanInformationDTO> GetAllSocialGrowthPlans()
        {
            try
            {
                var PlanInfor = _planInformationRepository.GetallIntagramPlans();
                return PlanInfor;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public PlanInformationDTO GetPlanInformationById(int planInformationId)
        {
            try
            {
                PlanInformationDTO PlInfoDTO = new PlanInformationDTO();
                PlInfoDTO = _planInformationRepository.GetPlanInformationById(planInformationId);
                return PlInfoDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DeletePlanInformation(int PlanInformationId)
        {
            try
            {
                var likeyAccount = _planInformationRepository.DeletePlanInformation(PlanInformationId);
                return likeyAccount;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool IsPlanNameExists(string PlanName, int PlanId = 0)
        {
            try
            {
                return _planInformationRepository.IsPlanNameExists(PlanName, PlanId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        

    }
}
