using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using SG2.CORE.BAL.Managers;
using SG2.CORE.MODAL.DTO.Customers;
using static SG2.CORE.COMMON.GlobalEnums;

namespace SG2.Core.Web
{
    public class AuthorizeCustomer : AuthorizeAttribute
    {

        private readonly SessionManager _sessionManager;

        public AuthorizeCustomer()
        {
            _sessionManager = new SessionManager();
        }
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            if (!httpContext.Items.Contains("isAuthentication"))
            {
                httpContext.Items.Add("isAuthentication", false);
            }
            
            var user = (CustomerDTO) _sessionManager.Get(SessionConstants.Customer);
            if (user != null)
            {
                httpContext.Items["isAuthentication"] = true;
                return true;
            }
            return false;  
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Items.Contains("isAuthentication"))
            {
                var routeValues = new RouteValueDictionary(new
                {
                    controller = "Home",
                    action = "Index",
                });
                filterContext.Result = new RedirectToRouteResult(routeValues);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (_sessionManager.Get(SessionConstants.Customer) == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                new {
                    controller = "Account",
                    action = "Login",
                    namespaces = new[] { "SG2.CORE.WEB.Controller.Controllers" }
                }));
            }
        }


    }

}