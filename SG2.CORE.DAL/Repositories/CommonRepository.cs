using SG2.CORE.DAL.DB;
using SG2.CORE.MODAL.DTO.Common;
using SG2.CORE.MODAL.DTO.Customers;
using SG2.CORE.MODAL.DTO.Proxy;
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
            using (var _db = new SocialGrowth2Entities())
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
                using (var _db = new SocialGrowth2Entities())
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
                using (var _db= new SocialGrowth2Entities())
                {
                    var Title = _db.SG2_usp_Get_Title().ToList();
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

        public static IList<CountryDTO> GetCountries()
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var countires = _db.SG2_SystemCountry.ToList();
                    if (countires != null)
                    {
                        return countires.Select(c => new CountryDTO()
                        {
                            CountryId = c.CountryId,
                            Name = c.Name,
                            Code = c.Code,
                            PhoneCode = c.PhoneCode,
                            StatusId = c.StatusId
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

        public static IList<CitiesAndCountriesDTO> GetCityAndCountryData(int? CityId )
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var data = _db.SG2_usp_Get_ProxyCitiesAndCountries(CityId).ToList();
                    return data.Select(c => new CitiesAndCountriesDTO()
                    {
                        CityId=Convert.ToInt32(c.CityId),
                        CountryId=Convert.ToInt32(c.CountryId),
                        CountryName=c.CountryName,
                        CityName=c.CityName,
                        CountyCityName=c.FullCityCountryName

                    }).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public static IList<VPSSDTO> GetVPSDTO()
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var VPS = _db.SG2_usp_Get_VPSSupplier().ToList();
                    if (VPS != null)
                    {
                        return VPS.Select(c => new VPSSDTO()
                        {
                            IssuingISPName = c.IssuingISPName,
                            VPSSId = c.VPSSId
                        }).ToList();
                    }

                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static IList<UserDTO> GetTeamMembers()
        {
            try
            {
                using (var _db=new SocialGrowth2Entities())
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
                using (var _db = new SocialGrowth2Entities())
                {
                    var countires = _db.SG2_SystemCity.ToList();
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
                using (var _db = new SocialGrowth2Entities())
                {
                    var countires = _db.SG2_SystemCity.Where(x=>x.CountryId == countryId).ToList();
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

        public static ProxyMappingDTO AssignedNearestProxyIP(int customerId, float latitude, float longitude,int socialProfileId)
        {
            using (var _db = new SocialGrowth2Entities())
            {
                var proxydata = _db.SG2_usp_Customer_AssignedNearestProxyIP(customerId, latitude, longitude, socialProfileId).FirstOrDefault();
                ProxyMappingDTO proxyDTO = new ProxyMappingDTO();
                if (proxydata != null)
                {
                    proxyDTO.CustomerId = proxydata.SocialProfileId;//TODO add SocialProfileid
                    proxyDTO.ProxyId = proxydata.ProxyId;
                    proxyDTO.ProxyMappingId = proxydata.ProxyMappingId;
                    proxyDTO.ProxyIPName = proxydata.ProxyIPName;
                    proxyDTO.ProxyPort = proxydata.ProxyPort;
                    proxyDTO.ProxyIPNumber = proxydata.ProxyIPNumber;
                    return proxyDTO;
                }
                return null;
            }
        }


    }
}
