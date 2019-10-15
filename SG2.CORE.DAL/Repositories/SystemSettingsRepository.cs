using SG2.CORE.DAL.DB;
using SG2.CORE.MODAL.DTO.SystemSettings;
using SG2.CORE.MODAL.ViewModals.Backend.SystemSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.DAL.Repositories
{
    public class SystemSettingsRepository
    {

        public SystemSettingsDTO AddSystemSetting(SystemSettingsDTO entity)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    _db.SG2_usp_SystemConfig_Save(entity.ConfigId, entity.ConfigValue, entity.ConfigValue2, entity.ModifiedOn, entity.ModifiedBy);
                    return entity;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SystemSettingsDTO UpdateSystemSetting(SystemSettingsDTO entity)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    _db.SG2_usp_SystemConfig_Save(entity.ConfigId, entity.ConfigValue, entity.ConfigValue2, entity.ModifiedOn, entity.ModifiedBy);
                    return entity;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IList<SystemSettingsDTO> GetSystemConfigs()
        {
            try
            {
                using (var _db=new SocialGrowth2Connection())
                {
                    var systemConfigdata = _db.SG2_usp_SystemConfig_GetAll(null, 1, "100", null).ToList();
                    if (systemConfigdata != null)
                    {
                        List<SystemSettingsDTO> systemSettingsDTOs = new List<SystemSettingsDTO>();
                        foreach (var item in systemConfigdata)
                        {
                            SystemSettingsDTO systemSettingDTO = new SystemSettingsDTO();
                            systemSettingDTO.ConfigId = item.ConfigId;
                            systemSettingDTO.ConfigKey = item.ConfigKey;

                            systemSettingsDTOs.Add(systemSettingDTO);
                        }
                        return systemSettingsDTOs;

                    }
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        public SystemSettingsDTO GetSystemSettingsById(short ConfigId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var sys = _db.SG2_usp_SystemConfig_GetById(ConfigId).FirstOrDefault();
                    if (sys != null)
                    {
                        SystemSettingsDTO systemSettings = new SystemSettingsDTO()
                        {
                            ConfigId = sys.ConfigId,
                            ConfigKey = sys.ConfigKey,
                            ConfigValue = sys.ConfigValue,
                            ConfigValue2 = sys.ConfigValue2,
                            CreatedOn = sys.CreatedOn,
                            CreatedBy = sys.CreatedBy,
                            ModifiedOn = sys.ModifiedOn,
                            ModifiedBy = sys.CreatedBy,

                        };
                        return systemSettings;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<SystemSettingsListingViewModal> GetSystemSettingsData(string SearchCriteria, int PageNumber, string PageSize, int? StatusId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var systemConfigdata = _db.SG2_usp_SystemConfig_GetAll(SearchCriteria, PageNumber, PageSize, StatusId).ToList();
                    if (systemConfigdata != null)
                    {
                        List<SystemSettingsListingViewModal> systemSettingsListingViewModalList = new List<SystemSettingsListingViewModal>();

                        foreach (var item in systemConfigdata)
                        {
                            SystemSettingsListingViewModal systemSettingsListingViewModal = new SystemSettingsListingViewModal();
                            systemSettingsListingViewModal.ConfigId = item.ConfigId;
                            systemSettingsListingViewModal.ConfigKey = item.ConfigKey;
                            systemSettingsListingViewModal.TotalRecord = item.TotalRecord;
                            systemSettingsListingViewModalList.Add(systemSettingsListingViewModal);
                        }
                        return systemSettingsListingViewModalList;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
