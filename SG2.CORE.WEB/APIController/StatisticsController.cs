using SG2.CORE.BAL.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;


namespace SG2.CORE.WEB.APIController
{
    public class StatisticsController : ApiController
    {

        protected readonly StatisticsManager _statisticsManager;

        public StatisticsController()
        {
            _statisticsManager = new StatisticsManager();
         
        }

        [HttpPost]
        public void SaveStatistics(string data)
        {
            _statisticsManager.SaveStatistics(data);
        }

        [HttpGet]
        public string TestApi()
        {
            return "I am here";
        }


    }
}
