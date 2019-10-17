using SG2.CORE.DAL.DB;
using SG2.CORE.MODAL.DTO.Common;
using SG2.CORE.MODAL.DTO.Customers;

using SG2.CORE.MODAL.DTO.SystemSettings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SG2.CORE.DAL.Repositories
{

    public class CommonRepository
    {
        public static IList<StatusDTO> GetStatus()
        {
            using (var _db = new SocialGrowth2Connection())
            {
                var usrdata = _db.SG2_usp_Get_EnumerationValue().ToList();
                if (usrdata != null)
                {
                    List<StatusDTO> statusDTOs = new List<StatusDTO>();
                    foreach (var item in usrdata)
                    {
                        StatusDTO statusDTO = new StatusDTO();
                        statusDTO.StatusName = item.Enumeration;
                        statusDTO.StatusId = item.EnumerationValueId;
                        statusDTO.StatusValue = item.Name;
                        statusDTOs.Add(statusDTO);

                    }
                    return statusDTOs;

                }
                return null;
            }

        }

        public static List<SystemSettingsDTO> GetSystemConfigs()
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
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
                            systemSettingDTO.ConfigValue = item.ConfigValue1;
                            systemSettingDTO.ConfigValue2 = item.ConfigValue2;
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

        public static IList<TitleDTO> GetTitle()
        {
            try
            {
                using (var _db= new SocialGrowth2Connection())
                {
                    var Title = _db.Customer_Title.ToList();
                    if (Title != null)
                    {
                        List<TitleDTO> titleDTOs = new List<TitleDTO>();
                        foreach (var item in Title)
                        {
                            TitleDTO titleDTO = new TitleDTO();
                            titleDTO.TitleId = item.PkTitleId;
                            titleDTO.TitleName = item.TitleName;
                            titleDTOs.Add(titleDTO);

                        }
                        return titleDTOs;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

       

        public static IList<UserDTO> GetTeamMembers()
        {
            try
            {
                using (var _db=new SocialGrowth2Connection())
                {
                    var user = _db.SG2_usp_Get_AllUser();
                    if (user != null)
                    {
                        return user.Select(c => new UserDTO()
                        {
                            UserName=c.UserName,
                            UserId=c.UserId,
                            RoleName=c.RoleName

                        }).ToList();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public static IList<CityDTO> GetCities()
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var countires = _db.SystemCities.ToList();
                    if (countires != null)
                    {
                        return countires.Select(c => new CityDTO()
                        {
                            CountryId = c.CountryId,
                            Name = c.Name,
                            Code = c.Code,
                            StatusId = c.StatusId,
                            CityId = c.CityId,
                            StateId = c.StateId
                        }).ToList();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static IList<CityDTO> GetCitiesByCountryId(int countryId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var countires = _db.SystemCities.Where(x=>x.CountryId == countryId).ToList();
                    if (countires != null)
                    {
                        return countires.Select(c => new CityDTO()
                        {
                            CountryId = c.CountryId,
                            Name = c.Name,
                            Code = c.Code,
                            StatusId = c.StatusId,
                            CityId = c.CityId,
                            StateId = c.StateId
                        }).ToList();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

     

    }
}
