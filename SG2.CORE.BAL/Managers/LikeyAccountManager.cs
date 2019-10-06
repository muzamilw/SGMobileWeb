using SG2.CORE.DAL.Repositories;
using SG2.CORE.MODAL.DTO.LikeyAccount;
using SG2.CORE.MODAL.ViewModals.Backend.LikeyAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.BAL.Managers
{
    public class LikeyAccountManager
    {

        private readonly LikeyAccountRepository _likeyAccountRepository;
        private readonly SessionManager _sessionManager;

        public LikeyAccountManager()
        {
            _likeyAccountRepository = new LikeyAccountRepository();
            _sessionManager = new SessionManager();
        }


        public LikeyAccountDTO AddLikeyAccount(LikeyAccountDTO entity)
        {
            try
            {
                _likeyAccountRepository.AddLikeyAccount(entity);
                return entity;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LikeyAccountDTO UpdateLikeyAccount(LikeyAccountDTO entity)
        {
            try
            {
                _likeyAccountRepository.UpdateLikeyAccount(entity);
                return entity;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LikeyAccountDTO GetJLikeyAccountById(int JVBoxId)
        {
            try
            {
                LikeyAccountDTO jVBoxDTO = new LikeyAccountDTO();
                jVBoxDTO = _likeyAccountRepository.GetLikeyAccountById(JVBoxId);
                return jVBoxDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

         public bool IsInstaUserNameExists(string InstaUserName, int LKaccId = 0)
        {
            try
            {
                return _likeyAccountRepository.IsInstaUserNameExists(InstaUserName, LKaccId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool DeleteLikeyAccount(int JVBoxId)
        {
            try
            {
                var likeyAccount=_likeyAccountRepository.DeleteLikeyAccount(JVBoxId);
                return likeyAccount;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<LikeyAccountListingViewModal> GetLikeyAccountData(string SearchCriteria, string PageSize, int PageNumber ,int? StatusId)
        {
            var Model = _likeyAccountRepository.GetLikeyAccountData(SearchCriteria, PageNumber, PageSize, StatusId);

            return Model;
        }
    }
}
