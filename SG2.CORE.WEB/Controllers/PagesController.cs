using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SG2.CORE.WEB.Controllers
{
    public class PagesController : BaseController
    {
        // GET: Pages
        public ActionResult TermsConditions()
        {
            //return this.RedirectToActionPermanent("signup", "account");
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            //return this.RedirectToActionPermanent("signup", "account");
            return View();
        }

        public ActionResult CookiesPolicy()
        {
            //return this.RedirectToActionPermanent("signup", "account");
            return View();
        }

    }
}