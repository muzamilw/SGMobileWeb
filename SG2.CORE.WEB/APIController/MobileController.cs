using SG2.CORE.MODAL.MobileViewModels;
using System;
using System.Web.Http;
using System.Collections.Generic;
using Action = SG2.CORE.MODAL.MobileViewModels.Action;
using SG2.CORE.BAL.Managers;
using System.Web;

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
        public IHttpActionResult Login(MobileLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string errorMesage = "";

                var res = _customerManager.LoginUser(model.Email, model.Password, ref errorMesage);
                if (res.Item1)
                {
                    return Ok(new
                    {
                        MobileLoginJsonRootObject = new MobileLoginJsonRootObject
                        {
                            StatusCode = 1,
                            StatusMessage = "Success",
                            CustomerId = res.Item2,
                            CustomerEmail = res.Item3

                        }

                    });
                }
                else
                {
                    return Ok(new
                    {
                        MobileLoginJsonRootObject = new MobileLoginJsonRootObject
                        {
                            StatusCode = 4,
                            StatusMessage = "Invalid username or password"
                        }

                    });
                }

            }
            else
            {
                return Ok(new
                {
                    MobileLoginJsonRootObject = new MobileLoginJsonRootObject
                    {
                        StatusCode = 4,
                        StatusMessage = "Invalid username or password"
                    }

                });
            }

            //List<Platform> Platforms = new List<Platform>();
            //Platform Profile1 = new Platform();
            //Profile1.PlatfromId = 1;
            //Profile1.Name = "Instagram";
            //Profile1.PlatformStatus = "Enabled";
            //Profile1.ProfileUserNames = new List<string> { "n_sardar000", "waheedsardar321" };
            //Platforms.Add(Profile1);

            //if (model.Status == null)
            //{
            //    return Ok(new
            //    {
            //        MobileLoginJsonRootObject = new MobileLoginJsonRootObject
            //        {
            //            CustomerEmail = model.Email,
            //            CustomerId = 1234,
            //            DeviceEMEI = "ANVF34S-DF",
            //            DeviceId = "AS22",
            //            Platforms = Platforms,
            //            StatusCode = 1,
            //            StatusMessage = "Success"
            //        }

            //    });

            //}
            //else
            //if (model.Status != null && model.Status == 2)
            //{
            //    return Ok(new
            //    {
            //        MobileLoginJsonRootObject = new MobileLoginJsonRootObject
            //        {
            //            StatusCode = 2,
            //            StatusMessage = "Invalid email and PIN"
            //        }

            //    });

            //}
            //else
            //if (model.Status != null && model.Status == 3)
            //{
            //    return Ok(new
            //    {
            //        MobileLoginJsonRootObject = new MobileLoginJsonRootObject
            //        {
            //            StatusCode = 3,
            //            StatusMessage = "Device Id not matched"
            //        }

            //    });

            //}
            //else
            //if (model.Status != null && model.Status == 4)
            //{
            //    return Ok(new
            //    {
            //        MobileLoginJsonRootObject = new MobileLoginJsonRootObject
            //        {
            //            StatusCode = 4,
            //            StatusMessage = "Subscription expired"
            //        }

            //    });

            //}
            //else
            //{
            //    return Ok(new
            //    {
            //        MobileLoginJsonRootObject = new MobileLoginJsonRootObject
            //        {
            //            StatusCode =0 ,
            //            StatusMessage = "UnKnown Status"
            //        }

            //    });

            //} 
        }
        [Route("GetManifestFile")]
        [HttpPost]
        public IHttpActionResult GetManifestFile(MobileLoginViewModel Model)
        {
          

            return Ok(new
            {
                MobileJsonRootObject = this.GetMobileManifestData()
            });
        
            
        }


    private MobileManifest GetMobileManifestData()
    {
        var platforms = new List<Platform>();
        var profiles = new List<Profile>();
        var actions = new List<MODAL.MobileViewModels.Action>();
        var action1 = new Action()
        {
            Type = "Follow",
            Count = 5,
            WaitEachAction = "18-sec"
        };
        var action2 = new Action()
        {
            Type = "Sleep",
            Count = 1,
            WaitEachAction = "30-mint"
        };
        var action3 = new Action()
        {
            Type = "Follow",
            Count = 8,
            WaitEachAction = "18-sec"
        };
        var action4 = new Action()
        {
            Type = "Sleep",
            Count = 1,
            WaitEachAction = "10-mint"
        };
        var action5 = new Action()
        {
            Type = "Unfollow",
            Count = 5,
            WaitEachAction = "40-sec"
        };
        var action6 = new Action()
        {
            Type = "Like",
            Count = 40,
            WaitEachAction = "30-Sec"
        };
        var action7 = new Action()
        {
            Type = "StoryView",
            Count = 3,
            WaitEachAction = "60-Sec"
        };
        var action8 = new Action()
        {
            Type = "Sleep",
            Count = 1,
            WaitEachAction = "50-mint"
        };
        var action9 = new Action()
        {
            Type = "Engagement",
            Count = 10,
            WaitEachAction = "90-se "
        };

        actions.Add(action1);
        actions.Add(action2);
        actions.Add(action3);
        actions.Add(action4);
        actions.Add(action5);
        actions.Add(action6);
        actions.Add(action7);
        actions.Add(action8);
        actions.Add(action9);

        Profile profile = new Profile()
        {
            ProfileId = 123456,
            Username = "hassanjamilbwp",
            Password = "",
            ProfileStatusId = 1,
            FollowModuleEnabled = true,
            UnfollowModuleEnabled = true,
            LikeModuleEnabled = true,
            CommentsModuleEnabled = true,
            StoryViewModuleEnabled = true,
            EngageMouduleEnabled = true,
            DirectMessagesModuleEnabled = true,
            UsernameHashtagsBioNameNotContains = "",
            FollowModule = new FollowModule()
            {
                Enabled = true,
                Settings = new Settings
                {
                    UserHasProfileImage = true,
                    UserHasMinimumNoOfPost = 10,
                    UserPostedWithLastXDays = 90,
                    UsersBlackList = null,
                    CheckPostForBlackListWords = true,
                    FollowPrivateAccount = false,
                    SkipBusinessAcounts = true,
                    DoNotFollowAccountHavingDigits = 5,
                    FollowAccountGender = new List<string> { "Male", "Female" },
                    FollowOnlyTheseLanguages = new List<string> { "EN", "MX" },
                    LikeUserLatesPosts = true,
                    CommentsUserLatesPosts = false,
                    ViewUserLatestStory = true,
                    MuteUserAfterFollow = true,
                },
                FollowData = new FollowData
                {
                    HashTags = new List<string> { "#pakistan", "#Lahore", "#Rawalpindi" },
                    GeoLocations = new List<string> { "Pakistan Lahore", "Pakistan Islamabad" },
                    CompetitorAccounts = new List<string> { "@hassanjamilbwp", "@oops_horizon" }
                }
            },
            UnFollowModule = new UnFollowModule()
            {
                Enabled = true,
                Settings = new UnFollowSettings
                {
                    UnflollowAfterNoOfDays = 15,
                    DoNotUnfollowLikersThatLikesNoOfPosts = 5,
                    DoNotUnfollowLikersThatCommentsNoOfPosts = 5,
                    WhiteList = new List<string> { "@username", "@username2" }
                }
            },
            StoryViewModule = new StoryViewModule()
            {
                Enabled = true,
                Settings = new StoryViewModuleSettings
                {
                    PostedNotMoreThanMinutesAgo = 180,
                    LikeUserRecentPosts = 3,
                    ReplyToUserStoryAfterView = true,
                    SendDirectMessageAfterLike = false
                }
            },
            CommentsModule = new CommentsModule()
            {
                Enabled = true,
                Settings = new CommentsModuleSettings()
                {
                    CommentsMostRecentPosts = 3,
                    CommentsOnly = new List<string> { "Images", "Videos" },
                    PostedWithinLastXDays = 3,
                    FilterPostsByNumberOfLikes = 5,
                }
            },
            DirectMessageModule = null,
            EngagementModule = new EngagementModule
            {
                Enabled = true,
                Settings = new EngagementModuleSettings
                {
                    EnableLikeCommentsAfterPostIsLike = true,
                    EngageEveryDayWithFollowingNo = 40,
                    EngageEveryDayWithUnFollowingNo = 40,
                    LikeMostRecentPost = true,
                    SendDirectMessageAfterLike = false,
                    ViewUserStoriesAfterLike = true
                },
                DirectMessageData = new EngagementModuleData
                {
                    Message = "Hello.....,"
                },

            },
            Actions = actions
        };

        profiles.Add(profile);
        Platform platform = new Platform()
        {
            PlatfromId = 1,
            Name = "Instagram",
            PlatformStatus = "Enabled",
            Profiles = profiles
        };

        platforms.Add(platform);

        return new MobileManifest
        {
            CustomerId = 123456,
            CustomerEmail = "hassanjamil.bwp@gmail.com",
            DocumentDateUTC = DateTime.Now.ToFileTimeUtc(),
            DeviceId = "",
            LicenseExpiredDateUTC = DateTime.Now.AddDays(10).ToFileTimeUtc(),
            StatusCode = 1,
            StatusMessage = "",
            Platforms = platforms
        };


    }



}
}
