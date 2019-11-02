using SG2.CORE.MODAL.MobileViewModels;
using System;
using System.Web.Http;
using System.Collections.Generic;

using SG2.CORE.BAL.Managers;
using System.Web;
using System.Net;

namespace SG2.CORE.WEB.APIController
{
    [RoutePrefix("api/mobile")]
    public class MobileController : ApiController
    {

        protected readonly CustomerManager _customerManager;

        public MobileController()
        {
            _customerManager = new CustomerManager();
        }

        [Route("Login")]
        [HttpPost]
        public IHttpActionResult Login(MobileLoginRequest model)
        {
            if (ModelState.IsValid)
            {

                var res = _customerManager.PerformMobileLogin(model.Email, model.Pin, model.IMEI, model.ForceSwitchDevice);
                if (res.LoginSuccessful)
                {
                    return Ok(new
                    {
                        MobileLoginJsonRootObject = new MobileLoginResponse
                        {
                            StatusCode = 1,
                            StatusMessage = "Success",
                            SocialProfileId = res.SocialProfileId,
                            SocialPasswordNeeded = res.PasswordNeeded

                        }

                    });
                }
                else
                {
                    return Ok(new
                    {
                        MobileLoginJsonRootObject = new MobileLoginResponse
                        {
                            StatusCode = 2,
                            StatusMessage = res.Message
                        }

                    });
                }

            }
            else
            {
                return Content(HttpStatusCode.BadRequest, "Input params missing");
            }

        }

        [Route("GetManifestFile")]
        [HttpPost]
        public IHttpActionResult GetManifestFile(MobileManifestRequest model)
        {
            var profile = _customerManager.GetSocialProfileById(model.SocialProfileId);

            var manifest = new MobileManifestResponse
            {
                CustomerId = 123456,
                CustomerEmail = "hassanjamil.bwp@gmail.com",
                LicenseExpiredDateUTC = DateTime.Now.AddDays(10).ToFileTimeUtc(),
                StatusCode = 1,
                StatusMessage = "",
               
            };

            return Ok(new
            {
                MobileJsonRootObject = manifest
            });


        }





    }
}
