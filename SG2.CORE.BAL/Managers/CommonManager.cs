using SG2.CORE.DAL.Repositories;
using SG2.CORE.MODAL.DTO.Common;
using SG2.CORE.MODAL.DTO.Customers;
using SG2.CORE.MODAL.DTO.Proxy;
using SG2.CORE.MODAL.DTO.SystemSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace SG2.CORE.BAL.Managers
{
    public class CommonManager
    {
        private readonly CommonRepository _commonRepository;
        protected readonly List<SystemSettingsDTO> SystemConfigs;

        public CommonManager()
        {
            _commonRepository = new CommonRepository();
        }
        public static IList<StatusDTO> GetStatus()
        {
            var Model = CommonRepository.GetStatus();
            return Model;
        }


        public static IList<CountryDTO> GetCountries()
        {
            var Model = CommonRepository.GetCountries();
            return Model;
        }

        public static IList<CitiesAndCountriesDTO> GetCityAndCountryData(int? CityId)
        {
            var Model = CommonRepository.GetCityAndCountryData(CityId);
            return Model;

        }
        public static List<SystemSettingsDTO> GetSystemConfigs()
        {
            var Model = CommonRepository.GetSystemConfigs();
            return Model;
        }

        public static IList<CityDTO> GetCities()
        {
            var Model = CommonRepository.GetCities();
            return Model;
        }

        public static IList<VPSSDTO> GetVPSDTO()
        {
            var Model = CommonRepository.GetVPSDTO();
            return Model;
        }
        public static IList<CityDTO> GetCitiesByCountryId(int countryid)
        {
            var Model = CommonRepository.GetCitiesByCountryId(countryid);
            return Model;
        }

        public static IList<UserDTO> GetTeamMembers()
        {
            var Model = CommonRepository.GetTeamMembers();
            return Model;

        }
        public static IList<TitleDTO> GetTitle()
        {
            var Model = CommonRepository.GetTitle();
            return Model;
        }

        public ProxyMappingDTO AssignedNearestProxyIP(int customer, string address, int socialProfileId, string _googeApiKey)
        {
            ProxyMappingDTO proxyMappingDTO = new ProxyMappingDTO();
            float Longitude = 0;
            float Latitude = 0;
            string geoCoordinate = GetLatitudeAndLongitude(address, _googeApiKey);
            if (geoCoordinate != string.Empty)
            {
                var cords = geoCoordinate.Split(',');
                Latitude =(float)Convert.ToDecimal(cords[0]);
                Longitude = (float)Convert.ToDecimal(cords[1]);

            }
            proxyMappingDTO = CommonRepository.AssignedNearestProxyIP(customer, Latitude, Longitude, socialProfileId);
            return proxyMappingDTO;

        }
        
        public string GetLatitudeAndLongitude(string address, string _googleApiKey)
        {
            string urlAddress = "https://maps.googleapis.com/maps/api/geocode/xml?address=" + address + "&key=" + _googleApiKey;
            string returnValue = "";
            try
            {

                XmlDocument objXmlDocument = new XmlDocument();
                objXmlDocument.Load(urlAddress);
                XmlNodeList objXmlNodeList = objXmlDocument.SelectNodes("/GeocodeResponse/result/geometry/location");
                foreach (XmlNode objXmlNode in objXmlNodeList)
                {
                    // GET LATITUDE 
                    returnValue = objXmlNode.ChildNodes.Item(0).InnerText;
                    // GET LONGITUDE 
                    returnValue += "," + objXmlNode.ChildNodes.Item(1).InnerText;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

    }
}
