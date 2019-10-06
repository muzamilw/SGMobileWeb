using SG2.CORE.BAL.Managers;
using SG2.CORE.COMMON;
using SG2.CORE.MODAL.DTO.TeamMember;
using SG2.CORE.MODAL.ViewModals.Backend.TeamMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SG2.CORE.WEB.Areas.SuperAdmin.Controllers
{
    public class AccountController : Controller
    {
        protected readonly TeamMemberManager _teamMemberManager;
        protected readonly SessionManager _sessionmanager;
       
        public AccountController()
        {
            _teamMemberManager = new TeamMemberManager();
            _sessionmanager = new SessionManager();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(SuperAdminViewModal model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ErrorMessagesDTO errorMessages = new ErrorMessagesDTO();
                    errorMessages = _teamMemberManager.LoginUser(model.Email, model.Password);
                    if (!errorMessages.IsError)
                    {
                        HttpContext.Items["isAuthentication"] = true;                      
                        return RedirectToAction("Index", "DashBoard");
                    }
                    else
                    {
                        ModelState.AddModelError("InvalidCredentials", errorMessages.ErrorMessage);
                        HttpContext.Items["isAuthentication"] = false;
                        return View(model);
                    }
                   
                     
                }
                else
                {
                    string messages = string.Join(", ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    //TempData["Success"] = "False";
                    //TempData["Message"] = messages;
                    return View(model);
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public ActionResult Logout()
        {
            _sessionmanager.Clear();
            HttpContext.Items["isAuthentication"] = false;
            HttpContext.Items["isAuthentication"] = null;
            return RedirectToAction("Index", "DashBoard");
        }

    }
}