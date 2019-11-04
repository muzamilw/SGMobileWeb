using SG2.CORE.MODAL.MobileViewModels;
using System;
using System.Web.Http;
using System.Collections.Generic;

using SG2.CORE.BAL.Managers;
using System.Web;
using System.Net;
using AutoMapper;
using SG2.CORE.MODAL;
using static SG2.CORE.COMMON.GlobalEnums;

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

        [Route("GetManifest")]
        public IHttpActionResult GetManifest(MobileManifestRequest model)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<SocialProfile, MobileSocialProfile>()
            );

            var config2 = new MapperConfiguration(cfg => cfg.CreateMap<SocialProfile_Instagram_TargetingInformation, MobileSocialProfile_Instagram_TargetingInformation>()
           );


            var config3 = new MapperConfiguration(cfg => cfg.CreateMap<List<SocialProfile_FollowedAccounts>, List<MobileSocialProfile_FollowedAccounts>>()
           );
            var mapper = new Mapper(config);
            var mapper2 = new Mapper(config2);
            var mapper3 = new Mapper(config3);

            if (ModelState.IsValid)
            {

                var profile = _customerManager.GetSocialProfileById(model.SocialProfileId);

                var manifest = new MobileManifestResponse
                {
                    CustomerId = profile.SocialProfile.CustomerId.Value,
                    StatusCode = 1,
                    StatusMessage = "",
                    Profile = mapper.Map<MobileSocialProfile>(profile.SocialProfile),
                    TargetInformation = mapper2.Map<MobileSocialProfile_Instagram_TargetingInformation>(profile.SocialProfile_Instagram_TargetingInformation),
                    ExistingFollowers = mapper3.Map<List<MobileSocialProfile_FollowedAccounts>>(profile.SocialProfile_FollowedAccounts)

                };

                manifest.Profile.Status = ((GeneralStatus)profile.SocialProfile.StatusId).ToString();

                manifest.Profile.SocialProfileType = ((SocialMedia)profile.SocialProfile.SocialProfileTypeId).ToString();

                return Ok(new
                {
                    MobileJsonRootObject = manifest
                });
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, "Input params missing");
            }


        }



        [Route("AppAction")]
        [HttpPost]
        public IHttpActionResult AppAction(MobileActionRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_customerManager.SaveMobileAppAction(model))
                        return Ok();
                    else
                        return Content(HttpStatusCode.BadRequest, "action could not be saved");

                }
                catch (Exception e)
                {
                    return Content(HttpStatusCode.BadRequest, e.ToString());
                }
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, "Input params missing");
            }
        }
    }
}


  
           



    

