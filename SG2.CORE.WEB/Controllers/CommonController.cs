using SG2.CORE.BAL.Managers;
using SG2.CORE.MODAL.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SG2.CORE.WEB.Controllers
{
    public class CommonController : Controller
    {
        protected readonly CustomerManager _customerManager;

        
        public CommonController()
        {
            _customerManager = new CustomerManager();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCityByCountryId(short countryId)
        {
            var Cities = CommonManager.GetCities().Where(m => m.CountryId == countryId).ToList();

            return Json(Cities, JsonRequestBehavior.AllowGet);
        }

        public IList<CitiesAndCountriesDTO> GetCityAndCountryData(int? CityId)
        {
            //var CACData = null;// CommonManager.GetCityAndCountryData(CityId);
            return null;
        }

    }
}