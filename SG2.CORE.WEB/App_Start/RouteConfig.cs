using System.Web.Mvc;
using System.Web.Routing;

namespace SG2.CORE.WEB.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
             name: "Special",
             url: "{action}",
             defaults: new { controller = "Home", action = "Index" }
         );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SG2.CORE.WEB.Controllers" }
            );
        }
    }
}
