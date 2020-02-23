using SG2.CORE.MODAL.MobileViewModels;
using System;
using System.Web.Http;
using System.Collections.Generic;
using System.Linq;
using SG2.CORE.BAL.Managers;
using System.Web;
using System.Net;
using AutoMapper;
using SG2.CORE.MODAL;
using static SG2.CORE.COMMON.GlobalEnums;
using AutoMapper;

namespace SG2.CORE.WEB.APIController
{
    [RoutePrefix("api/mobile")]
    public class MobileController : ApiController
    {

        protected readonly CustomerManager _customerManager;
        protected readonly StatisticsManager _statsManager;

        public MobileController()
        {
            _customerManager = new CustomerManager();
            _statsManager = new StatisticsManager();
        }

        [Route("Login")]
        [HttpPost]
        public IHttpActionResult Login(MobileLoginRequest model)
        {
            if (ModelState.IsValid)
            {

                var res = _customerManager.PerformMobileLogin(model);
                if (res.LoginSuccessful)
                {

                    var resp = new
                    {
                        MobileLoginJsonRootObject = new MobileLoginResponse
                        {
                            StatusCode = 1,
                            StatusMessage = "Success",
                            SocialProfileId = res.SocialProfileId,
                            SocialPasswordNeeded = res.PasswordNeeded,
                            BlockCode = res.BlockCode,
                            BlockDateTimeUTC = res.BlockDateTimeUTC,
                            InitialStatsReceived = res.InitialStatsReceived

                        }

                    };

                    Customer cust = null;
                    if (res.IsBroker)
                    {
                        cust = _customerManager.GetCustomerByCustomerId(res.CustomerId);
                        resp.MobileLoginJsonRootObject.IsBroker = res.IsBroker;
                        resp.MobileLoginJsonRootObject.BrokerAppName = cust.BrokerAppName;
                        resp.MobileLoginJsonRootObject.BrokerAspectColor = cust.BrokerAspectColor;
                        resp.MobileLoginJsonRootObject.BrokerFeedbackPage = cust.BrokerFeedbackPage;
                        resp.MobileLoginJsonRootObject.BrokerHomePage = cust.BrokerHomePage;
                        if (!string.IsNullOrEmpty(cust.BrokerLogo))
                        {
                            resp.MobileLoginJsonRootObject.BrokerLogo = $"{HttpContext.Current.Request.Url.Scheme}{System.Uri.SchemeDelimiter}{HttpContext.Current.Request.Url.Authority}" +"/AgencyLogos/"+ cust.BrokerLogo;
                        }
                         
                        resp.MobileLoginJsonRootObject.BrokerPaymentPlanID = cust.BrokerPaymentPlanID;
                        resp.MobileLoginJsonRootObject.BrokerPrivacyPolicy = cust.BrokerPrivacyPolicy;
                        resp.MobileLoginJsonRootObject.BrokerStrapLine = cust.BrokerStrapLine;
                        resp.MobileLoginJsonRootObject.BrokerTermsOfUse = cust.BrokerTermsOfUse;
                        resp.MobileLoginJsonRootObject.BrokerTrainingLink = cust.BrokerTrainingLink;
                        resp.MobileLoginJsonRootObject.BrokerTrustPilotCode = cust.BrokerTrustPilotCode;
                    }

                            
                    return Ok(resp);
                }
                else
                {
                    return Ok(new
                    {
                        MobileLoginJsonRootObject = new MobileLoginResponse
                        {
                            StatusCode = res.ErrorCode,
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
        [HttpPost]
        public IHttpActionResult GetManifest(MobileManifestRequest model)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<SocialProfile, MobileSocialProfile>()
            );

            var config2 = new MapperConfiguration(cfg => cfg.CreateMap<SocialProfile_Instagram_TargetingInformation, MobileSocialProfile_Instagram_TargetingInformation>()
           );


            var config3 = new MapperConfiguration(cfg => cfg.CreateMap<SocialProfile_FollowedAccounts, MobileSocialProfile_FollowedAccounts>()
           );

            var config4 = new MapperConfiguration(cfg => cfg.CreateMap<PaymentPlan, MobilePaymentPlan>()
           );

            var mapper = new Mapper(config);
            var mapper2 = new Mapper(config2);
            var mapper3 = new Mapper(config3);
            var mapper4 = new Mapper(config4);

            if (ModelState.IsValid)
            {

              
                DateTime commentCutOffDate = DateTime.Today.AddDays(-1);

                var profile = _customerManager.GetSocialProfileById(model.SocialProfileId);
                if (!String.IsNullOrEmpty( model.SocialPassword))
                {
                    _customerManager.UpdateBasicSocialProfileSocialPassword(model.SocialPassword, model.SocialProfileId);
                }

                //resetting the changed flag to false since we are sending the new manifest.
                _customerManager.ResetSocialProfileManifestChangeFlag(false, model.SocialProfileId);

                 var stats = this._statsManager.GetStatistics(profile.SocialProfile.SocialProfileId);

                var whitelist = profile.SocialProfile_Instagram_TargetingInformation.WhistListManualUsers;
                var whilelistArray = new List<string>();
                if (!string.IsNullOrEmpty( whitelist) )
                    whilelistArray = whitelist.Split(',').ToList();

                var manifest = new MobileManifestResponse
                {
                    CustomerId = profile.SocialProfile.CustomerId.Value,
                    StatusCode = 1,
                    StatusMessage = "",
                    Profile = mapper.Map<MobileSocialProfile>(profile.SocialProfile),
                    CurrentPlan = mapper4.Map<MobilePaymentPlan>(  profile.CurrentPaymentPlan),
                    TargetInformation = mapper2.Map<MobileSocialProfile_Instagram_TargetingInformation>(profile.SocialProfile_Instagram_TargetingInformation),
                    // filter the unfollow list by the white list of users which is manually entered.
                    FollowersToUnFollow = mapper3.Map<List<MobileSocialProfile_FollowedAccounts>>(profile.SocialProfile_FollowedAccounts.Where(g => g.FollowedDateTime < commentCutOffDate && !whilelistArray.Contains(g.FollowedSocialUsername)).ToList()),
                    //randomize the followers to comment list and only send 50
                    FollowersToComment = mapper3.Map<List<MobileSocialProfile_FollowedAccounts>>(profile.SocialProfile_FollowedAccounts.Where(g => g.FollowedDateTime >= commentCutOffDate).OrderBy(x => Guid.NewGuid()).Take(50).ToList()),
                    FollowList = _customerManager.GetFollowList(model.SocialProfileId).Select( g=> new MobileSocialProfile_FollowedAccounts { FollowedSocialUsername = g.SocialUsername, FollowedDateTime = DateTime.Now }).Take(30).ToList(),
                    LikeList = _customerManager.GetFollowList(model.SocialProfileId).Select(g => new MobileSocialProfile_FollowedAccounts { FollowedSocialUsername = g.SocialUsername }).Take(30).ToList()

                //20 count Follow list is all paid instagram profile usernames which are already not in follower list.  and follow exchange checkbox true
                //10 count Like list is all paid instagram profile usernames which are already not in follower list.  and like exchange checkbox true
                };

                manifest.Profile.StatsFollowersIncrease = stats.FollowersTotal.Value - stats.FollowersInitial.Value;
                manifest.Profile.StatsFollowingsIncrease = stats.FollowingsTotal.Value - stats.FollowingsInitial.Value;

                var daysSinceRegistration = DateTime.Today - profile.SocialProfile.CreatedOn;
                //  
                ////manifest.TargetInformation.FollMaxPerDayLim = (profile.SocialProfile_Instagram_TargetingInformation.FollDailyIncreaseLim.Value * daysSinceRegistration.Days) + profile.SocialProfile_Instagram_TargetingInformation.FollNewPerDayLim.Value - manifest.FollowList.Count;
                ////if (manifest.TargetInformation.FollMaxPerDayLim > profile.SocialProfile_Instagram_TargetingInformation.FollMaxPerDayLim)
                ////    manifest.TargetInformation.FollMaxPerDayLim = profile.SocialProfile_Instagram_TargetingInformation.FollMaxPerDayLim.Value;

                ////manifest.TargetInformation.UnFollMaxPerDayLim = (profile.SocialProfile_Instagram_TargetingInformation.UnFollDailyIncreaseLim.Value * daysSinceRegistration.Days) + profile.SocialProfile_Instagram_TargetingInformation.UnFollNewPerDayLim.Value;
                ////if (manifest.TargetInformation.UnFollMaxPerDayLim > profile.SocialProfile_Instagram_TargetingInformation.UnFollMaxPerDayLim)
                ////    manifest.TargetInformation.UnFollMaxPerDayLim = profile.SocialProfile_Instagram_TargetingInformation.UnFollMaxPerDayLim.Value;

                ////manifest.TargetInformation.LikeMaxPerDayLim = (profile.SocialProfile_Instagram_TargetingInformation.LikeDailyIncreaseLim.Value * daysSinceRegistration.Days )+ profile.SocialProfile_Instagram_TargetingInformation.LikePerDayLim.Value - manifest.LikeList.Count;
                ////if (manifest.TargetInformation.LikeMaxPerDayLim > profile.SocialProfile_Instagram_TargetingInformation.LikeMaxPerDayLim)
                ////    manifest.TargetInformation.LikeMaxPerDayLim = profile.SocialProfile_Instagram_TargetingInformation.LikeMaxPerDayLim.Value;

                ////manifest.TargetInformation.ViewStoriesMaxPerDayLim = (profile.SocialProfile_Instagram_TargetingInformation.ViewStoriesDailyIncreaseLim.Value * daysSinceRegistration.Days )+ profile.SocialProfile_Instagram_TargetingInformation.ViewStoriesPerDayLim.Value;
                ////if (manifest.TargetInformation.ViewStoriesMaxPerDayLim > profile.SocialProfile_Instagram_TargetingInformation.ViewStoriesMaxPerDayLim)
                ////    manifest.TargetInformation.ViewStoriesMaxPerDayLim = profile.SocialProfile_Instagram_TargetingInformation.ViewStoriesMaxPerDayLim.Value;

                ////manifest.TargetInformation.CommentMaxPerDayLim = (profile.SocialProfile_Instagram_TargetingInformation.CommentDailyIncreaseLim.Value * daysSinceRegistration.Days )+ profile.SocialProfile_Instagram_TargetingInformation.CommentPerDayLim.Value;
                ////if (manifest.TargetInformation.CommentMaxPerDayLim > profile.SocialProfile_Instagram_TargetingInformation.CommentMaxPerDayLim)
                ////    manifest.TargetInformation.CommentMaxPerDayLim = profile.SocialProfile_Instagram_TargetingInformation.CommentMaxPerDayLim.Value;

              


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


        // Follow  with username    actionid = 60
        // UnFollow  with username  actionid = 61
        // Like with username       actionid = 62
        // Comment with username    actionid = 63
        // StoryView with username  actionid = 64
        


        //[Route("AppAction")]
        //[HttpPost]
        //public IHttpActionResult AppAction(MobileActionRequest model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (_customerManager.SaveMobileAppAction(model))
        //                return Ok(new MobileActionResponse { StatusCode=1, StatusMessage="Success" });
        //            else
        //                return Content(HttpStatusCode.BadRequest, "action could not be saved");

        //        }
        //        catch (Exception e)
        //        {
        //            return Content(HttpStatusCode.BadRequest, e.ToString());
        //        }
        //    }
        //    else
        //    {
        //        return Content(HttpStatusCode.BadRequest, "Input params missing");
        //    }
        //}

        // Follow  with username    actionid = 60
        // UnFollow  with username  actionid = 61
        // Like with username       actionid = 62
        // Comment with username    actionid = 63
        // StoryView with username  actionid = 64
        // FollowerCount with username  and count in msg actionid = 65

        [Route("AppActionBulk")]
        [HttpPost]
        public IHttpActionResult AppActionBulk(List<MobileActionRequest> model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    int successCount = 0;

                    foreach (var item in model)
                    {
                        if (_customerManager.SaveMobileAppAction(item))
                            successCount++;
                    }

                    int[] BlockedStatuses = new int[] { 66, 67, 68, 69, 70, 71,72 };

                    var BlockedActions = model.Where(g => BlockedStatuses.Contains(g.ActionId)).ToList();
                    if (BlockedActions != null &&  BlockedActions.Count>0)
                    {

                        _customerManager.UpdateBasicSocialProfileBlock( (BlockStatus )BlockedActions.First().ActionId, BlockedActions.First().SocialProfileId);
                    }

                    //add the newly followed accounts 
                    _customerManager.AddRemoveFollowAccounts(model.Where(g => g.ActionId == 60 || g.ActionId == 61).ToList());

                    
                    var FollowingCount = model.Where(g => g.ActionId == 60).Count();

                    var UnFollowCount = model.Where(g => g.ActionId == 61).Count();
                    //follow
                    var FollowingCountNet = FollowingCount;// - UnFollowCount;

                    var LikeCount = model.Where(g => g.ActionId == 62).Count();

                    var CommentCount = model.Where(g => g.ActionId == 63).Count();

                    var StoryCount = model.Where(g => g.ActionId == 64).Count();

                    var FollowingEvent = model.Where(g => g.ActionId == 65).FirstOrDefault();
                    var FollowCount = 0;
                    if ( FollowingEvent != null)
                    {
                        FollowCount = Convert.ToInt32(FollowingEvent.Message);
                        //take diff with previous no and then update.
                    }

                    DateTime statsDate = DateTime.Now;

                    if (!string.IsNullOrEmpty(model.First().ActionDateTime))
                    {
                        statsDate = Convert.ToDateTime(model.First().ActionDateTime);
                    }

                    //omny update stats if anything changes.
                    if (FollowingCount > 0  || FollowingCountNet  > 0|| LikeCount > 0 || CommentCount > 0 || StoryCount > 0 || FollowCount > 0  || UnFollowCount > 0)
                        _statsManager.UpdateStatistics(model.First().SocialProfileId, FollowingCountNet, LikeCount, CommentCount, StoryCount, FollowCount, statsDate, UnFollowCount);

                    if (successCount == model.Count)
                    {

                        var flag = _customerManager.GetSocialProfileManifestChangeFlag(model.First().SocialProfileId);
                        return Ok(new MobileActionResponse { StatusCode = 1, StatusMessage = "{\"Result\":\"Success\", \"ManifestUpdatedSinceLastGet\":\"" + flag.ToString() + "\" }" });
                    }
                    else
                        return Content(HttpStatusCode.BadRequest, "One or more actions could not be saved");
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


        [Route("InitialStats")]
        [HttpPost]
        public IHttpActionResult InitialStats(MobileIniitalStatsRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_statsManager.SaveInitialStatistics(model))
                        return Ok(new MobileActionResponse { StatusCode = 1, StatusMessage = "Success" });
                    else
                        return Content(HttpStatusCode.BadRequest, "Stats could not be saved");

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
    

    [Route("ClearData")]
    [HttpGet]
    public IHttpActionResult ClearData(int SocialProfileId)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (_statsManager.ClearStatsActions(SocialProfileId))
                    return Ok(new MobileActionResponse { StatusCode = 1, StatusMessage = "Success, data cleared" });
                else
                    return Content(HttpStatusCode.BadRequest, "Data could not be cleared");

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

public class NullToZeroIntTypeConverter : ITypeConverter<int?, int?>
    {
        //public int Convert(ResolutionContext ctx)
        //{
        //    if ((int)ctx.SourceValue == 0)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        return (int)ctx.SourceValue;
        //    }
        //}

        public int? Convert(int? source, int? destination, ResolutionContext context)
        {
            if (source.HasValue)
                return source.Value;
            else
                return 0;
        }
    }
}


  
           



    

