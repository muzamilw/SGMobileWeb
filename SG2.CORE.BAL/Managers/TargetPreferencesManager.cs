using SG2.CORE.DAL.Repositories;
using SG2.CORE.MODAL.DTO.TargetPreferences;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SG2.CORE.BAL.Managers
{
    public class TargetPreferencesManager
    {
        private readonly TargetPreferencesRepository _targetPreferencesRepository;

        public TargetPreferencesManager()
        {
            _targetPreferencesRepository = new TargetPreferencesRepository();
        }
        public TargetPreferencesDTO SaveTargetPreferences(TargetPreferencesDTO model)
        {
            try
            {

                return _targetPreferencesRepository.SaveTargetPreferences(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveSocialProfileData(string InstaUserName, string InstaPasseord, int cityId, int countryId, int SocialProfileId, int JVStatus=0, string verificationCode="")
        {
            try
            {
                return _targetPreferencesRepository.SaveSocialProfileData(InstaUserName, InstaPasseord, cityId, countryId, SocialProfileId, JVStatus, verificationCode);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool SaveJVInfo(int cityId, int countryId, int SocialProfileId, int JVStatus = 0, int ProxyId=0)
        {
            try
            {
                return _targetPreferencesRepository.SaveJVInfo(cityId, countryId, SocialProfileId, JVStatus, ProxyId);
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
                return _targetPreferencesRepository.IsSocialUserNameExist(InstaUser, SocialProfileId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TargetPreferencesDTO> GetPendingQueueTargetInformation(int QueueStatus)
        {
            try
            {
                return  _targetPreferencesRepository.GetPendingQueueTargetInformation(QueueStatus);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool SaveUpdateUserDataIndividually(string value, string fieldName, int socialProfileId, int TargetingInformationId)
        {
            try
            {
                return _targetPreferencesRepository.SaveUpdateUserDataIndividually(value, fieldName, socialProfileId, TargetingInformationId);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


    }
}
