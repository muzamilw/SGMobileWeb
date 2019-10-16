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
                    var config = new SystemConfig();
                    config.ConfigValue = entity.ConfigValue;
                    config.ConfigValue2 = entity.ConfigValue2;
                    config.ModifiedOn = DateTime.Now;
                    config.ModifiedBy = entity.ModifiedBy;
                    _db.SystemConfigs.Add(config);

                    _db.SaveChanges();
                    entity.ConfigId = config.ConfigId;
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
                    var config = _db.SystemConfigs.Where(g => g.ConfigId == entity.ConfigId).SingleOrDefault();
                    config.ConfigValue = entity.ConfigValue;
                    config.ConfigValue2 = entity.ConfigValue2;
                    config.ModifiedOn = DateTime.Now;
                    config.ModifiedBy = entity.ModifiedBy;

                    _db.SaveChanges();
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
                    var systemConfigdata = _db.SystemConfigs.ToList();
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
                    var sys = _db.SystemConfigs.Where(g => g.ConfigId == ConfigId).FirstOrDefault();
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
                    var systemConfigdata = _db.SystemConfigs.ToList(); //_db.SG2_usp_SystemConfig_GetAll(SearchCriteria, PageNumber, PageSize, StatusId).ToList();
                    if (systemConfigdata != null)
                    {
                        List<SystemSettingsListingViewModal> systemSettingsListingViewModalList = new List<SystemSettingsListingViewModal>();

                        foreach (var item in systemConfigdata)
                        {
                            SystemSettingsListingViewModal systemSettingsListingViewModal = new SystemSettingsListingViewModal();
                            systemSettingsListingViewModal.ConfigId = item.ConfigId;
                            systemSettingsListingViewModal.ConfigKey = item.ConfigKey;
                            systemSettingsListingViewModal.TotalRecord = systemConfigdata.Count();
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
