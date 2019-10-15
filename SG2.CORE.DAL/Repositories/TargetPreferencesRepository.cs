using SG2.CORE.DAL.DB;
using SG2.CORE.MODAL.DTO.TargetPreferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.DAL.Repositories
{
    public class TargetPreferencesRepository
    {

        public TargetPreferencesDTO SaveTargetPreferences(TargetPreferencesDTO entity)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var result = _db.SG2_usp_Customer_SavePreference(entity.SocialProfileId,
                                                                      entity.Preference1,
                                                                      entity.Preference2,
                                                                      entity.Preference3,
                                                                      entity.Preference4,
                                                                      entity.Preference5,
                                                                      entity.Preference6,
                                                                      entity.Preference7,
                                                                      entity.Preference8,
                                                                      entity.Preference9,
                                                                      entity.Country,
                                                                      entity.InstaUser,
                                                                      entity.InstaPassword,
                                                                      Convert.ToInt32(entity.City),
                                                                      entity.ProfileName,
                                                                      entity.Id,
                                                                      entity.QueueStatusId,
                                                                      entity.Preference10,
                                                                      entity.SocialAccAs);

                    if (result != null)
                    {
                        entity.SocialProfileId = (int)result.Select(x => x.SocialProfileId).FirstOrDefault();
                        return entity;
                    }
                    return null;
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public bool SaveSocialProfileData(string InstaUserName, string InstaPasseord, int cityId, int countryId, int SocialProfileId, int JVStatus = 0, string verificationCode = "")
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var user = _db.SG2_SocialProfile.FirstOrDefault(x => x.SocialProfileId == SocialProfileId);
                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(InstaUserName))
                        {
                            user.SocialUsername = InstaUserName;
                        }
                             
                        if (!string.IsNullOrEmpty(InstaPasseord))
                        {
                            user.SocialPassword = InstaPasseord;
                        }
                            
                        if (cityId != 0)
                            user.SocialPrefferedCity = cityId;

                        if (countryId != 0)
                            user.SocialPrefferedCountry = countryId;

                        if (JVStatus != 0)
                        {
                            user.JVBoxStatusId = JVStatus;
                        }

                        if (!string.IsNullOrEmpty(verificationCode))
                        {
                            user.verificationCode = verificationCode;
                        }

                        _db.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool SaveJVInfo(int cityId, int countryId, int SocialProfileId, int JVStatus = 0,int ProxyId=0)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var user = _db.SG2_SocialProfile.FirstOrDefault(x => x.SocialProfileId == SocialProfileId);
                    if (user != null)
                    {
                        user.SocialPrefferedCity = cityId;
                        user.SocialPrefferedCountry = countryId;
                        if (JVStatus != 0)
                        {
                            user.JVBoxStatusId = JVStatus;
                        }
                        if (ProxyId != 0)
                        {
                           
                        }
                        _db.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        

        public bool IsSocialUserNameExist(string InstaUser, int SocialProfileId = 0)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    if (SocialProfileId == 0) return !_db.SG2_SocialProfile.Any(u => u.SocialUsername.Equals(InstaUser));
                    var user = _db.SG2_SocialProfile.Find(SocialProfileId);
                    if (user.SocialUsername.Equals(InstaUser, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                    return !_db.SG2_SocialProfile.Any(r => r.SocialUsername.Equals(InstaUser) && r.SocialProfileId != SocialProfileId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TargetPreferencesDTO> GetPendingQueueTargetInformation(int QueueStatusId)
        {
            try
            {
                List<TargetPreferencesDTO> profiles = null;
                using (var _db = new SocialGrowth2Connection())
                {
                    var profs =  _db.SG2_usp_Customer_GetPendingSocialProfiles(QueueStatusId).ToList();
                    if (profs != null)
                    {
                        profiles = profs.Select(prof => new TargetPreferencesDTO()
                        {
                            Preference1 = prof.Preference1,
                            Preference10 = prof.Preference10,
                            Preference2 = prof.Preference2,
                            Preference3 = prof.Preference3,
                            Preference4 = prof.Preference4,
                            Preference5 = prof.Preference5,
                            Preference6 = prof.Preference6,
                            Preference7 = prof.Preference7,
                            Preference8 = prof.Preference8,
                            Preference9 = prof.Preference9,
                            SocialProfileId=prof.SocialProfileId,
                            ProfileName=prof.SocialProfileName,
                            InstaUser=prof.SocialUsername,
                            JvBoxExchangeName=prof.ExchangeName,
                            JVServerId = prof.JVServerId??0
                        }).ToList();
                    }
                }
                return profiles;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
