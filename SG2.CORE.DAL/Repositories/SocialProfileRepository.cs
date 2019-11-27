using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Web;
using AutoMapper;
using SG2.CORE.COMMON;
using SG2.CORE.DAL.DB;
using SG2.CORE.MODAL.DTO.Common;
using SG2.CORE.MODAL.DTO.Customers;
using SG2.CORE.MODAL;
using SG2.CORE.MODAL.ViewModals.Backend;
using SG2.CORE.MODAL.ViewModals.Backend.ActionBoard;
using static SG2.CORE.COMMON.GlobalEnums;
using SG2.CORE.MODAL.MobileViewModels;

namespace SG2.CORE.DAL.Repositories
{
	public class SocialProfileRepository
	{
		public bool UpdateSocialProfile(string DeviceBinLocation, string SocialProfileName, int SocialProfileId)
		{
			try
			{
				using (var _db = new SocialGrowth2Connection())
				{

					var profile = _db.SocialProfiles.Where(g => g.SocialProfileId == SocialProfileId).SingleOrDefault();
					if (DeviceBinLocation != null) {
						profile.DeviceBinLocation = DeviceBinLocation;

					}
					profile.SocialProfileName = SocialProfileName;
                    profile.SocialUsername = SocialProfileName;
                    profile.UpdatedBy = "User";
                    profile.UpdatedOn = DateTime.Now;

					_db.SaveChanges();

					return true;
				}

			}
			catch (Exception)
			{

				throw;
			}

		}


        public bool UpdateTargetProfileLists(SocialProfileDTO request)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    var target = _db.SocialProfile_Instagram_TargetingInformation.Where(g => g.TargetingInformationId == request.SocialProfile_Instagram_TargetingInformation.TargetingInformationId).SingleOrDefault();

                    if ( target != null)
                    {
                        target.WhistListManualUsers = request.SocialProfile_Instagram_TargetingInformation.WhistListManualUsers;
                        target.WhilstListImportedUsers = request.SocialProfile_Instagram_TargetingInformation.WhilstListImportedUsers;
                        target.BlackListUsers = request.SocialProfile_Instagram_TargetingInformation.BlackListUsers;
                        target.BlackListLocations = request.SocialProfile_Instagram_TargetingInformation.BlackListLocations;
                        target.BlackListHashtags = request.SocialProfile_Instagram_TargetingInformation.BlackListHashtags;
                        target.BlackListWordsManual = request.SocialProfile_Instagram_TargetingInformation.BlackListWordsManual;


                        target.UpdatedBy = "User";
                        target.UpdatedOn = DateTime.Now;
                        _db.SaveChanges();
                    }

                    return true;
                }

            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool UpdateTargetProfile(SocialProfileDTO request)
		{
			try
			{
				using (var _db = new SocialGrowth2Connection())
				{

					var profile = _db.SocialProfile_Instagram_TargetingInformation.Where(g => g.TargetingInformationId == request.SocialProfile_Instagram_TargetingInformation.TargetingInformationId).SingleOrDefault();
                    //var isSystem = profile.IsSystem;
                    //               if (profile != null)
                    //{
                    //	var parentProperties = request.SocialProfile_Instagram_TargetingInformation.GetType().GetProperties();
                    //	var childProperties = profile.GetType().GetProperties();

                    //	foreach (var parentProperty in parentProperties)
                    //	{
                    //		foreach (var childProperty in childProperties)
                    //		{
                    //			if (parentProperty.Name != "CreatedBy" && parentProperty.Name != "CreatedOn"  && parentProperty.Name != "WhistListManualUsers" && parentProperty.Name != "WhilstListImportedUsers" && parentProperty.Name != "BlackListUsers" && parentProperty.Name != "BlackListLocations" && parentProperty.Name != "BlackListHashtags" && parentProperty.Name != "BlackListWordsManual")
                    //                           {
                    //				if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType)
                    //				{
                    //					childProperty.SetValue(profile, parentProperty.GetValue(request.SocialProfile_Instagram_TargetingInformation));
                    //					break;
                    //				}
                    //			}
                    //		}
                    //	}

                    profile.HashTagsToEngage = request.SocialProfile_Instagram_TargetingInformation.HashTagsToEngage;
                    profile.LocationsToEngage = request.SocialProfile_Instagram_TargetingInformation.LocationsToEngage;
                    profile.DirectCompetitors = request.SocialProfile_Instagram_TargetingInformation.DirectCompetitors;
                    profile.GenderEngagmentPref = request.SocialProfile_Instagram_TargetingInformation.GenderEngagmentPref;
                    profile.IncludeBusinessAccounts = request.SocialProfile_Instagram_TargetingInformation.IncludeBusinessAccounts;

                    profile.FollDailyIncreaseLim = request.SocialProfile_Instagram_TargetingInformation.FollDailyIncreaseLim;
                    profile.FollMaxPerDayLim = request.SocialProfile_Instagram_TargetingInformation.FollMaxPerDayLim;
                    profile.FollNewPerDayLim = request.SocialProfile_Instagram_TargetingInformation.FollNewPerDayLim;

                    profile.UnFollDailyIncreaseLim = request.SocialProfile_Instagram_TargetingInformation.UnFollDailyIncreaseLim;
                    profile.UnFollMaxPerDayLim = request.SocialProfile_Instagram_TargetingInformation.UnFollMaxPerDayLim;
                    profile.UnFollNewPerDayLim = request.SocialProfile_Instagram_TargetingInformation.UnFollNewPerDayLim;

                    profile.LikeDailyIncreaseLim = request.SocialProfile_Instagram_TargetingInformation.LikeDailyIncreaseLim;
                    profile.LikeMaxPerDayLim = request.SocialProfile_Instagram_TargetingInformation.LikeMaxPerDayLim;
                    profile.LikePerDayLim = request.SocialProfile_Instagram_TargetingInformation.LikePerDayLim;

                    profile.ViewStoriesDailyIncreaseLim = request.SocialProfile_Instagram_TargetingInformation.ViewStoriesDailyIncreaseLim;
                    profile.ViewStoriesMaxPerDayLim = request.SocialProfile_Instagram_TargetingInformation.ViewStoriesMaxPerDayLim;
                    profile.ViewStoriesPerDayLim = request.SocialProfile_Instagram_TargetingInformation.ViewStoriesPerDayLim;

                    profile.CommentDailyIncreaseLim = request.SocialProfile_Instagram_TargetingInformation.CommentDailyIncreaseLim;
                    profile.CommentMaxPerDayLim = request.SocialProfile_Instagram_TargetingInformation.CommentMaxPerDayLim;
                    profile.CommentPerDayLim = request.SocialProfile_Instagram_TargetingInformation.CommentPerDayLim;

                    profile.FollowOn = request.SocialProfile_Instagram_TargetingInformation.FollowOn;

                    profile.FollUserProfileImage = request.SocialProfile_Instagram_TargetingInformation.FollUserProfileImage;
                    profile.FollUserMinPosts = request.SocialProfile_Instagram_TargetingInformation.FollUserMinPosts;
                    profile.FollUserMinPostsVal = request.SocialProfile_Instagram_TargetingInformation.FollUserMinPostsVal;

                    profile.FollUserPostsLastXDays = request.SocialProfile_Instagram_TargetingInformation.FollUserPostsLastXDays;
                    profile.FollUserPostsLastXDaysVal = request.SocialProfile_Instagram_TargetingInformation.FollUserPostsLastXDaysVal;

                    profile.FollDoNotFollowUsernamewithdigits = request.SocialProfile_Instagram_TargetingInformation.FollDoNotFollowUsernamewithdigits;
                    profile.FollDoNotFollowUsernamewithdigitsVal = request.SocialProfile_Instagram_TargetingInformation.FollDoNotFollowUsernamewithdigitsVal;

                    profile.AfterFollLikeuserPosts = request.SocialProfile_Instagram_TargetingInformation.AfterFollLikeuserPosts;
                    profile.AfterFollCommentUserPosts = request.SocialProfile_Instagram_TargetingInformation.AfterFollCommentUserPosts;
                    profile.FollEngageViewUserStoryAfterLike = request.SocialProfile_Instagram_TargetingInformation.FollEngageViewUserStoryAfterLike;
                    profile.AfterFollMuteUser = request.SocialProfile_Instagram_TargetingInformation.AfterFollMuteUser;
                    profile.FollUserLangs = request.SocialProfile_Instagram_TargetingInformation.FollUserLangs;
                    profile.FollUserLangsList = request.SocialProfile_Instagram_TargetingInformation.FollUserLangsList;
                    profile.AfterFollViewUserStory = request.SocialProfile_Instagram_TargetingInformation.AfterFollViewUserStory;

                    profile.UnFollowOn = request.SocialProfile_Instagram_TargetingInformation.UnFollowOn;
                    profile.UnFollFollowersAfterMinDays = request.SocialProfile_Instagram_TargetingInformation.UnFollFollowersAfterMinDays;
                    profile.UnFollFollowersAfterMinDaysVal = request.SocialProfile_Instagram_TargetingInformation.UnFollFollowersAfterMinDaysVal;
                    profile.UnFollDoNotUnFollowLikersOfPosts = request.SocialProfile_Instagram_TargetingInformation.UnFollDoNotUnFollowLikersOfPosts;
                    profile.UnFollDoNotUnFollowLikersOfPostsCount = request.SocialProfile_Instagram_TargetingInformation.UnFollDoNotUnFollowLikersOfPostsCount;
                    profile.UnFollDoNotUnFollowCommThatCommented = request.SocialProfile_Instagram_TargetingInformation.UnFollDoNotUnFollowCommThatCommented;
                    profile.UnFollDoNotUnFollowCommThatCommentedCount = request.SocialProfile_Instagram_TargetingInformation.UnFollDoNotUnFollowCommThatCommentedCount;
                    profile.UnFollUseWhiteList = request.SocialProfile_Instagram_TargetingInformation.UnFollUseWhiteList;

                    profile.ContactMembersOn = request.SocialProfile_Instagram_TargetingInformation.ContactMembersOn;
                    profile.CommUsrRecentPosts = request.SocialProfile_Instagram_TargetingInformation.CommUsrRecentPosts;
                    profile.CommUsrRecentPostsVal = request.SocialProfile_Instagram_TargetingInformation.CommUsrRecentPostsVal;
                    profile.CommUsrRecentPostsTypes = request.SocialProfile_Instagram_TargetingInformation.CommUsrRecentPostsTypes;
                    profile.CommUserPostedWithinXDays = request.SocialProfile_Instagram_TargetingInformation.CommUserPostedWithinXDays;
                    profile.CommUserPostedWithinXDaysVal = request.SocialProfile_Instagram_TargetingInformation.CommUserPostedWithinXDaysVal;
                    profile.CommFltrPostsByLike = request.SocialProfile_Instagram_TargetingInformation.CommFltrPostsByLike;
                    profile.CommFltrPostsByLikeCount = request.SocialProfile_Instagram_TargetingInformation.CommFltrPostsByLikeCount;
                    profile.CommDelCommAfterXDays = request.SocialProfile_Instagram_TargetingInformation.CommDelCommAfterXDays;
                    profile.CommDelCommAfterXDaysCount = request.SocialProfile_Instagram_TargetingInformation.CommDelCommAfterXDaysCount;

                    profile.CommLine1 = request.SocialProfile_Instagram_TargetingInformation.CommLine1;
                    profile.CommLine2 = request.SocialProfile_Instagram_TargetingInformation.CommLine2;
                    profile.CommLine3 = request.SocialProfile_Instagram_TargetingInformation.CommLine3;
                    profile.CommLine4 = request.SocialProfile_Instagram_TargetingInformation.CommLine4;
                    profile.CommLine5 = request.SocialProfile_Instagram_TargetingInformation.CommLine5;


                    profile.MonOper = request.SocialProfile_Instagram_TargetingInformation.MonOper;
                    profile.TueOper = request.SocialProfile_Instagram_TargetingInformation.TueOper;
                    profile.WedOper = request.SocialProfile_Instagram_TargetingInformation.WedOper;
                    profile.ThuOper = request.SocialProfile_Instagram_TargetingInformation.ThuOper;
                    profile.FriOper = request.SocialProfile_Instagram_TargetingInformation.FriOper;
                    profile.SatOper = request.SocialProfile_Instagram_TargetingInformation.SatOper;
                    profile.SunOper = request.SocialProfile_Instagram_TargetingInformation.SunOper;

                    profile.ExecutionIntervals = request.SocialProfile_Instagram_TargetingInformation.ExecutionIntervals;




                        profile.UpdatedOn = DateTime.Now;

                        //this was getting lost.
                        //profile.IsSystem = isSystem;

                        _db.SaveChanges();
					//}

				

					return true;
				}

			}
			catch (Exception)
			{

				throw;
			}

		}
	}
}
