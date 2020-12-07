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

        public bool UpdateSocialProfilePhonePackageDetails(DateTime PurchaseDate, string PhonePackagePurchaseSessionID, int SocialProfileId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    var profile = _db.SocialProfiles.Where(g => g.SocialProfileId == SocialProfileId).SingleOrDefault();
                    
                    profile.PhonePackagePurchaseSessionID = PhonePackagePurchaseSessionID;
                    profile.PhonePackagePurchaseDate = PurchaseDate;
                    profile.PhonePackagePurchased = true;


                    _db.SaveChanges();

                    return true;
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool UpdateSocialProfileSocialPassword(string SocialPassword, int SocialProfileId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    var profile = _db.SocialProfiles.Where(g => g.SocialProfileId == SocialProfileId).SingleOrDefault();
                    
                    profile.SocialPassword = SocialPassword;
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

        public bool ResetSocialProfileManifestChangeFlag(bool Flag, int SocialProfileId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    _db.Database.ExecuteSqlCommand("update SocialProfile set ManifestUpdatedSinceLastGet=0 where SocialProfileId=" + SocialProfileId.ToString(), 1);

                    return true;
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool GetSocialProfileManifestChangeFlag(int SocialProfileId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    return _db.Database.SqlQuery<bool>("select ManifestUpdatedSinceLastGet from SocialProfile where SocialProfileId=" + SocialProfileId.ToString(), 1).SingleOrDefault();

                  
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool UpdateSocialProfileBlocks(BlockStatus blockStatus, int SocialProfileId, DateTime currentDate)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    var profile = _db.SocialProfiles.Where(g => g.SocialProfileId == SocialProfileId).SingleOrDefault();

                    profile.BockedSinceDateTime = currentDate;
                    if (blockStatus == BlockStatus.Clear)
                    {
                        profile.BlockedStatus = 0;
                    }
                    else
                    {
                        profile.BlockedStatus = (int)blockStatus;
                    }
                    
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


        public bool UpdateTargetProfileBlackLists(SocialProfileDTO request)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    var target = _db.SocialProfile_Instagram_TargetingInformation.Where(g => g.TargetingInformationId == request.SocialProfile_Instagram_TargetingInformation.TargetingInformationId).SingleOrDefault();

                    if (target != null)
                    {
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
        public bool UpdateTargetProfileWhiteList(SocialProfileDTO request)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    var target = _db.SocialProfile_Instagram_TargetingInformation.Where(g => g.TargetingInformationId == request.SocialProfile_Instagram_TargetingInformation.TargetingInformationId).SingleOrDefault();

                    if (target != null)
                    {
                        target.WhistListManualUsers = request.SocialProfile_Instagram_TargetingInformation.WhistListManualUsers;
                       
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

        public bool UpdateTargetProfile(SocialProfileDTO request, bool SaveFullTargetProfile = true)
		{
			try
			{
				using (var _db = new SocialGrowth2Connection())
				{

					var profile = _db.SocialProfile_Instagram_TargetingInformation.Where(g => g.TargetingInformationId == request.SocialProfile_Instagram_TargetingInformation.TargetingInformationId).SingleOrDefault();
                   

                    profile.HashTagsToEngage = request.SocialProfile_Instagram_TargetingInformation.HashTagsToEngage;
                    profile.LocationsToEngage = request.SocialProfile_Instagram_TargetingInformation.LocationsToEngage;
                    profile.DirectCompetitors = request.SocialProfile_Instagram_TargetingInformation.DirectCompetitors;
                    profile.GenderEngagmentPref = request.SocialProfile_Instagram_TargetingInformation.GenderEngagmentPref;
                    profile.IncludeBusinessAccounts = request.SocialProfile_Instagram_TargetingInformation.IncludeBusinessAccounts;

                    profile.AfterFollLikeuserPosts = request.SocialProfile_Instagram_TargetingInformation.AfterFollLikeuserPosts;
                    profile.AfterFollCommentUserPosts = request.SocialProfile_Instagram_TargetingInformation.AfterFollCommentUserPosts;
                    profile.AfterFollViewUserStory = request.SocialProfile_Instagram_TargetingInformation.AfterFollViewUserStory;
                    profile.AfterFollCommentUserStory = request.SocialProfile_Instagram_TargetingInformation.AfterFollCommentUserStory;
                    profile.UnFollFollowersAfterMinDays = request.SocialProfile_Instagram_TargetingInformation.UnFollFollowersAfterMinDays;
                    profile.FollowOn = request.SocialProfile_Instagram_TargetingInformation.FollowOn;
                    profile.ExecutionIntervals = request.SocialProfile_Instagram_TargetingInformation.ExecutionIntervals;

                    profile.MonOper = request.SocialProfile_Instagram_TargetingInformation.MonOper;
                    profile.TueOper = request.SocialProfile_Instagram_TargetingInformation.TueOper;
                    profile.WedOper = request.SocialProfile_Instagram_TargetingInformation.WedOper;
                    profile.ThuOper = request.SocialProfile_Instagram_TargetingInformation.ThuOper;
                    profile.FriOper = request.SocialProfile_Instagram_TargetingInformation.FriOper;
                    profile.SatOper = request.SocialProfile_Instagram_TargetingInformation.SatOper;
                    profile.SunOper = request.SocialProfile_Instagram_TargetingInformation.SunOper;

                    if (SaveFullTargetProfile)
                    {
                        

                        profile.FollUserProfileImage = request.SocialProfile_Instagram_TargetingInformation.FollUserProfileImage;
                        profile.FollUserMinPosts = request.SocialProfile_Instagram_TargetingInformation.FollUserMinPosts;
                        profile.FollUserMinPostsVal = request.SocialProfile_Instagram_TargetingInformation.FollUserMinPostsVal;

                        profile.FollUserPostsLastXDays = request.SocialProfile_Instagram_TargetingInformation.FollUserPostsLastXDays;
                        profile.FollUserPostsLastXDaysVal = request.SocialProfile_Instagram_TargetingInformation.FollUserPostsLastXDaysVal;

                        profile.FollDoNotFollowUsernamewithdigits = request.SocialProfile_Instagram_TargetingInformation.FollDoNotFollowUsernamewithdigits;
                        profile.FollDoNotFollowUsernamewithdigitsVal = request.SocialProfile_Instagram_TargetingInformation.FollDoNotFollowUsernamewithdigitsVal;

                        
                        

                        profile.AfterFollMuteUser = request.SocialProfile_Instagram_TargetingInformation.AfterFollMuteUser;
                        profile.FollUserLangs = request.SocialProfile_Instagram_TargetingInformation.FollUserLangs;
                        profile.FollUserLangsList = request.SocialProfile_Instagram_TargetingInformation.FollUserLangsList;
                       
                        

                        profile.UnFollowOn = request.SocialProfile_Instagram_TargetingInformation.UnFollowOn;
                        
                        profile.UnFollFollowersAfterMinDaysVal = request.SocialProfile_Instagram_TargetingInformation.UnFollFollowersAfterMinDaysVal;
                        profile.UnFollDoNotUnFollowLikersOfPosts = request.SocialProfile_Instagram_TargetingInformation.UnFollDoNotUnFollowLikersOfPosts;
                        profile.UnFollDoNotUnFollowLikersOfPostsCount = request.SocialProfile_Instagram_TargetingInformation.UnFollDoNotUnFollowLikersOfPostsCount;
                        profile.UnFollDoNotUnFollowCommThatCommented = request.SocialProfile_Instagram_TargetingInformation.UnFollDoNotUnFollowCommThatCommented;
                        profile.UnFollDoNotUnFollowCommThatCommentedCount = request.SocialProfile_Instagram_TargetingInformation.UnFollDoNotUnFollowCommThatCommentedCount;
                        profile.UnFollUseWhiteList = request.SocialProfile_Instagram_TargetingInformation.UnFollUseWhiteList;

                        

                        
                    }



                        profile.UpdatedOn = DateTime.Now;

                        //this was getting lost.
                        //profile.IsSystem = isSystem;

                        _db.SaveChanges();
                    //}

                   
                    //var sProfile = _db.SocialProfiles.Where(g => g.SocialProfileId == request.SocialProfile_Instagram_TargetingInformation.SocialProfileId).SingleOrDefault();
                    //if (sProfile != null)
                    //{

                    if (request.SocialProfile_Instagram_TargetingInformation.SocialProfileId.HasValue)
                        _db.Database.ExecuteSqlCommand("update SocialProfile set ManifestUpdatedSinceLastGet = 1 where SocialProfileId = " + request.SocialProfile_Instagram_TargetingInformation.SocialProfileId.ToString(), 1);
                     




                    return true;
				}

			}
			catch (Exception)
			{

				throw;
			}

		}


        public bool AddRemoveFollowAccounts(List<MobileActionRequest> list)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    foreach (var item in list)
                    {
                        if (item.ActionId == 60) 
                        {
                            _db.SocialProfile_FollowedAccounts.Add(new SocialProfile_FollowedAccounts { FollowedDateTime = String.IsNullOrEmpty( item.ActionDateTime) ? DateTime.UtcNow : Convert.ToDateTime (item.ActionDateTime), FollowedSocialUsername = item.TargetSocialUserName, SocialProfileId = item.SocialProfileId, StatusId = 1 });
                        }
                        else if (item.ActionId == 73)
                        {
                            _db.SocialProfile_FollowedAccounts.Add(new SocialProfile_FollowedAccounts { FollowedDateTime = String.IsNullOrEmpty(item.ActionDateTime) ? DateTime.UtcNow : Convert.ToDateTime(item.ActionDateTime), FollowedSocialUsername = item.TargetSocialUserName, SocialProfileId = item.SocialProfileId, StatusId = 3 });
                        }
                        else
                        {
                            var delrec = _db.SocialProfile_FollowedAccounts.Where(g => g.FollowedSocialUsername == item.TargetSocialUserName && g.SocialProfileId == item.SocialProfileId).ToList();
                            //if (delrec != null)
                            //    _db.SocialProfile_FollowedAccounts.Remove(delrec);
                            if ( delrec != null && delrec.Count() > 0)
                            {
                                foreach (var citem in delrec)
                                {
                                    citem.StatusId = 2;
                                }
                            }
                            

                        }
                    }

                    _db.SaveChanges();

                    return true;
                }

                }
            catch (Exception e)
            {
                return true;
                
            }
        }

        /// <summary>
        ///            //90  14  BorastCastMessage BorastCastMessage   False   1   True
        ///            //91  14  SequenceMessage BorastCastMessage   False   1   True
        ///            //92  14  MessageSeen BorastCastMessage   False   1   True
        ///            //93  14  ResetBroadcast ResetBroadcast  False   1   True
        ///            //94  14  ResetSequence ResetSequence   False   1   True
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool AddUpdateResetMeseages(List<MobileActionRequest> list)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    foreach (var item in list)
                    {
                        if (item.ActionId == 90 || item.ActionId == 91)
                        {
                            var msg = item.Message.Split(':');

                            _db.SocialProfile_Messages.Add(new SocialProfile_Messages { SentDateTime= String.IsNullOrEmpty(item.ActionDateTime) ? DateTime.UtcNow : Convert.ToDateTime(item.ActionDateTime), SentSocialUsername = item.TargetSocialUserName, SocialProfileId = item.SocialProfileId, StatusId = 1, MessageBody = msg[1], MessageType =  Convert.ToInt32(msg[0])});
                        }
                        else if (item.ActionId == 92)
                        {
                            var mtype = Convert.ToInt32(item.Message);
                            var latestmsg = _db.SocialProfile_Messages.Where(g => g.SocialProfileId.Value == item.SocialProfileId && g.SentSocialUsername == item.TargetSocialUserName && g.MessageType == mtype).OrderByDescending(g => g.SentDateTime).FirstOrDefault();
                            if (latestmsg!= null)
                                latestmsg.StatusId = 2;


                        }
                        else
                        {
                            _db.Database.ExecuteSqlCommand("delete from SocialProfile_Messages where socialprofileid = " + item.SocialProfileId.ToString() + " and MessageType=" + item.Message);

                        }
                    }

                    _db.SaveChanges();

                    return true;
                }

            }
            catch (Exception e)
            {
                return true;

            }
        }


        public bool RemoveBagTags(List<MobileActionRequest> list)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    foreach (var item in list)
                    {
                        var target = _db.SocialProfile_Instagram_TargetingInformation.Where(g => g.SocialProfileId == item.SocialProfileId).SingleOrDefault();
                        if ( target != null)
                        {
                            var tags = target.HashTagsToEngage.Split(',');

                            string tag = item.TargetSocialUserName.Trim().ToLower();

                            var newtags = tags.Where(g => !g.Contains(tag)).ToList();

                            target.HashTagsToEngage =  string.Join(",", newtags);  

                        }

                    }

                    _db.SaveChanges();

                    return true;
                }

            }
            catch (Exception e)
            {
                return true;

            }
        }

        public bool UpdateSocialProfileWarmupInformation(UpdateProfileWarmupRequest model)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    var profile = _db.SocialProfiles.Where(g => g.SocialProfileId == model.SocialProfileId).SingleOrDefault();

                    if (model.IgAccountStartDate != null)
                        profile.IgAccountStartDate = Convert.ToDateTime(model.IgAccountStartDate);

                    if (model.WarmupCalculated != null)
                        profile.WarmupCalculated = Convert.ToBoolean(model.WarmupCalculated);

                    if (model.WarmupCompleted != null)
                        profile.WarmupCompleted = Convert.ToBoolean(model.WarmupCompleted);

                    if (model.WarmupCompletedDateTime != null)
                        profile.WarmupCompletedDateTime = Convert.ToDateTime(model.WarmupCompletedDateTime);

                    if (model.BotRunningDays != null)
                        profile.BotRunningDays = Convert.ToInt32(model.BotRunningDays);

                    _db.SaveChanges();

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
