using SG2.CORE.DAL.DB;
using SG2.CORE.MODAL.DTO.LikeyAccount;
using SG2.CORE.MODAL.ViewModals.Backend.LikeyAccount;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SG2.CORE.DAL.Repositories
{
    public class LikeyAccountRepository
    {

        public LikeyAccountDTO AddLikeyAccount(LikeyAccountDTO entity)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {

                    _db.SG2_usp_LikeyAccount_Save(entity.LikeyAccountId, entity.InstaUserName, entity.InstaPassword, entity.Country, entity.City, entity.Gender, entity.HashTag, entity.StatusId);
                    return entity;
                }
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
                using (var _db = new SocialGrowth2Entities())
                {
                    _db.SG2_usp_LikeyAccount_Save(entity.LikeyAccountId, entity.InstaUserName, entity.InstaPassword, entity.Country, entity.City, entity.Gender, entity.HashTag, entity.StatusId);
                    return entity;
                }

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
                using (var _db = new SocialGrowth2Entities())
                {

                    if (LKaccId == 0) return !_db.SG2_LikeyAccount.Any(u => u.InstaUserName.Equals(InstaUserName));
                    var user = _db.SG2_LikeyAccount.Find(LKaccId);
                    if (user.InstaUserName.Equals(InstaUserName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                    return !_db.SG2_LikeyAccount.Any(r => r.InstaUserName.Equals(InstaUserName) && r.LikeyAccountId != LKaccId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LikeyAccountDTO GetLikeyAccountById(int LikeyAccountId)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var pro = _db.SG2_usp_LikeyAccount_GetById(LikeyAccountId).FirstOrDefault();
                    if (pro != null)
                    {
                        LikeyAccountDTO likey = new LikeyAccountDTO()
                        {
                            LikeyAccountId = pro.LikeyAccountId,
                            InstaUserName = pro.InstaUserName,
                            InstaPassword = pro.InstaPassword,
                            Country = pro.Country,
                            City = pro.City,
                            Gender = Convert.ToSByte(pro.Gender),
                            HashTag = pro.HashTag,
                            StatusId = pro.StatusId

                        };
                        return likey;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteLikeyAccount(int LikeyAccountId)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var LKaccount=_db.SG2_usp_LikeyAccount_Delete(LikeyAccountId);
                    if (LKaccount == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IList<LikeyAccountListingViewModal> GetLikeyAccountData(string SearchCriteria, int PageNumber, string PageSize, int? StatusId)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    var likeyAccountdata = _db.SG2_usp_LikeyAccount_GetAll(SearchCriteria, PageNumber, PageSize, StatusId).ToList();
                    if (likeyAccountdata != null)
                    {
                        List<LikeyAccountListingViewModal> likeyAccountListingViewModalsList = new List<LikeyAccountListingViewModal>();

                        foreach (var item in likeyAccountdata)
                        {
                            LikeyAccountListingViewModal likeyAccountListingViewModal = new LikeyAccountListingViewModal();
                            likeyAccountListingViewModal.LikeyAccountId = item.LikeyAccountId;
                            likeyAccountListingViewModal.InstaUserName = item.InstaUserName;
                            likeyAccountListingViewModal.Status = item.StatusName;
                            likeyAccountListingViewModal.TotalRecord = item.TotalRecord;
                            likeyAccountListingViewModalsList.Add(likeyAccountListingViewModal);
                        }
                        return likeyAccountListingViewModalsList;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool TargetInformationSetLikes(int JVNoOfLikes, int JVLikeyStatus,int profileid)
        {
            try
            {
                using (var _db = new SocialGrowth2Entities())
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        
    }
}
