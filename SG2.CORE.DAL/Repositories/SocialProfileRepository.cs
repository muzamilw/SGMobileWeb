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
					if (profile != null)
					{
						var parentProperties = request.SocialProfile_Instagram_TargetingInformation.GetType().GetProperties();
						var childProperties = profile.GetType().GetProperties();

						foreach (var parentProperty in parentProperties)
						{
							foreach (var childProperty in childProperties)
							{
								if (parentProperty.Name != "CreatedBy" && parentProperty.Name != "CreatedOn")
								{
									if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType)
									{
										childProperty.SetValue(profile, parentProperty.GetValue(request.SocialProfile_Instagram_TargetingInformation));
										break;
									}
								}
							}
						}
						profile.UpdatedOn = DateTime.Now;

						_db.SaveChanges();
					}

				

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
