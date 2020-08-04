using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SG2.CORE.BAL.Managers;
using SG2.CORE.MODAL.DTO.Customers;
using SG2.CORE.WEB.App_Start;

namespace SG2.CORE.WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {

        private IList<StatusDTO> _Statuses;
        internal IList<StatusDTO> Statuses // the global controlled variable
        {
            get
            {
                if (_Statuses ==null)
                {                    
                    _Statuses = CommonManager.GetStatus();
                }
                return _Statuses;
            }
        }

        protected void Application_Start()
        {            
            Application["ApplicationStatuses"] = Statuses;// the global variable's bed

            try
            {
                string domain = WebConfigurationManager.AppSettings["WebsiteUrl"];

                JobScheduler sch = new JobScheduler();
                sch.Start();

                // run code every 60mins and upload file on server
                //var pendingTargetInfoQueueTimer = new System.Timers.Timer { Interval = (60 * (60 * 1000)) };
                //string pendingTargetInfoQueueurl = string.Format("{0}api/queuestatus/SetPendingTargetPreferenceIntoQueue", domain);
                //pendingTargetInfoQueueTimer.Elapsed += (sender, eventArgs) =>
                //{
                //    using (var s = new HttpClient())
                //    {
                //        var result = s.GetAsync(pendingTargetInfoQueueurl).Result;
                //    }
                //};
                //pendingTargetInfoQueueTimer.Start();

            }
            catch (Exception ex)
            {
                throw;
            }

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

       

    }
}
