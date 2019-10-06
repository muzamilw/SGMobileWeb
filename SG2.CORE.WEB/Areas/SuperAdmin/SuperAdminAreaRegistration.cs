using System.Web.Mvc;

namespace SG2.CORE.WEB.Areas.SuperAdmin
{
    public class SuperAdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SuperAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SuperAdmin_default",
                "sadmin/{controller}/{action}/{id}",
                new { action = "Index",Controller="DashBoard", id = UrlParameter.Optional }
            );
        }
    }
}