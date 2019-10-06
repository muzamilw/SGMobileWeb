using SG2.CORE.BAL.Managers;
using SG2.CORE.MODAL.DTO.Customers;
using System.Collections.Generic;
using System.Web.Mvc;
using static SG2.CORE.COMMON.GlobalEnums;

namespace SG2.CORE.WEB.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base

        protected CustomerDTO CDT = null;
        protected IList<StatusDTO> ApplicationStatuses = null;
        protected readonly SessionManager _sessionManager;
        public BaseController()
        {            
            _sessionManager = new SessionManager();   
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            CDT = (CustomerDTO)_sessionManager.Get(SessionConstants.Customer);
           if (HttpContext.Application["ApplicationStatuses"] !=null)
                ApplicationStatuses = HttpContext.Application["ApplicationStatuses"] as List<StatusDTO>;
            if (CDT != null)
            {
                HttpContext.Items["isAuthentication"] = true;
                ViewBag.CustomerDTO = CDT;
                ViewBag.IsAuthentication = true;
            }

            base.OnActionExecuting(filterContext);
        }

    }
}