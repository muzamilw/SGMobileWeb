using SG2.CORE.DAL.Repositories;
using SG2.CORE.MODAL.DTO.SystemSettings;
using SG2.CORE.MODAL.ViewModals.Backend.SystemSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.BAL.Managers
{
    public class SystemSettingsManager
    {
        private readonly SystemSettingsRepository _systemSettingsRepository;
        private readonly SessionManager _sessionManager;

        public SystemSettingsManager()
        {
            _systemSettingsRepository = new SystemSettingsRepository();
            _sessionManager = new SessionManager();
        }


        public SystemSettingsDTO AddLikeyAccount(SystemSettingsDTO entity)
        {
            try
            {
                _systemSettingsRepository.AddSystemSetting(entity);
                return entity;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SystemSettingsDTO UpdateLikeyAccount(SystemSettingsDTO entity)
        {
            try
            {
                _systemSettingsRepository.UpdateSystemSetting(entity);
                return entity;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SystemSettingsDTO GetSystemSettingsId(short ConfigId)
        {
            try
            {
                SystemSettingsDTO jVBoxDTO = new SystemSettingsDTO();
                jVBoxDTO = _systemSettingsRepository.GetSystemSettingsById(ConfigId);
                return jVBoxDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<SystemSettingsListingViewModal> GetSystemSettingsData(string SearchCriteria, string PageSize, int PageNumber, int? StatusId)
        {
            var Model = _systemSettingsRepository.GetSystemSettingsData(SearchCriteria, PageNumber, PageSize, StatusId);

            return Model;
        }
    }
}
