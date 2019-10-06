using SG2.CORE.BAL.Managers;
using SG2.CORE.MODAL.DTO.TeamMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using static SG2.CORE.COMMON.GlobalEnums;

namespace SG2.CORE.WEB.App_Start
{
    public class SuperAdminAuthorizeFilters : AuthorizeAttribute
    {
        private readonly SessionManager _sessionManager;

        public SuperAdminAuthorizeFilters()
        {
            _sessionManager = new SessionManager();
        }
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            if (!httpContext.Items.Contains("isAuthentication"))
            {
                httpContext.Items.Add("isAuthentication", false);
            }

            var user = (SystemUserDTO)_sessionManager.Get(SessionConstants.SystemUser);
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
                    controller = "Account",
                    action = "login",
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
            if (_sessionManager.Get(SessionConstants.SystemUser) == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                new
                {
                    controller = "Account",
                    action = "Login",
                    namespaces = new[] { "SG2.CORE.WEB.Areas.SuperAdmin.Controllers" }
                }));
            }
        }


    }
}