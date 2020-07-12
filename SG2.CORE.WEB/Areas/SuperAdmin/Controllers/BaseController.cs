using SG2.CORE.BAL.Managers;
using SG2.CORE.MODAL.DTO.Customers;
using SG2.CORE.MODAL.DTO.TeamMember;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using static SG2.CORE.COMMON.GlobalEnums;

namespace SG2.CORE.WEB.Areas.SuperAdmin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base

        protected SystemUserDTO CDT = null;
        protected IList<StatusDTO> ApplicationStatuses = null;
        protected readonly SessionManager _sessionManager;
        public BaseController()
        {
            _sessionManager = new SessionManager();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.BaseURL = "/sa";
            // ViewBag.SetMenuActiveClass = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            CDT = (SystemUserDTO)_sessionManager.Get(SessionConstants.SystemUser);
            if (HttpContext.Application["ApplicationStatuses"] != null)
                ApplicationStatuses = HttpContext.Application["ApplicationStatuses"] as List<StatusDTO>;
            if (CDT != null)
            {
                HttpContext.Items["isAuthentication"] = true;
                ViewBag.SystemUserDTO = CDT;
                ViewBag.IsAuthentication = true;
                if (CDT.UserRoleName == "Manager")
                {
                    var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                    if (controllerName == "SystemSettings" || controllerName == "TeamMember")
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            controller = "Permission",
                            action = "Index"
                        }));
                    }

                }
                else if (CDT.UserRoleName == "Operations")
                {
                    var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                    if (controllerName == "SystemSettings" || controllerName == "TeamMember" || controllerName == "ActionBoard" || controllerName == "Dashboard")
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            controller = "Permission",
                            action = "Index"
                        }));
                    }

                }
                else if (CDT.UserRoleName == "Sales")
                {
                    var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                    if (controllerName == "SystemSettings" || controllerName == "TeamMember" || controllerName == "ProxyIPs" || controllerName == "LikeAccts" || controllerName == "JVBoxes")
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            controller = "Permission",
                            action = "Index"
                        }));
                    }

                }
            }



            base.OnActionExecuting(filterContext);
        }

    }
}