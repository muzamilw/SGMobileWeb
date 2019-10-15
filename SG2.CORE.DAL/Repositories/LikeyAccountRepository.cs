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
                using (var _db = new SocialGrowth2Connection())
                {
                    var oLikeyAccount = new LikeyAccount();
                    oLikeyAccount.LikeyAccountId = entity.LikeyAccountId;
                    oLikeyAccount.InstaUserName = entity.InstaUserName;
                    oLikeyAccount.InstaPassword = entity.InstaPassword;
                    oLikeyAccount.Country = entity.Country;
                    oLikeyAccount.City = entity.City;
                    oLikeyAccount.Gender = entity.Gender;
                    oLikeyAccount.HashTag = entity.HashTag;
                    oLikeyAccount.StatusId = entity.StatusId;


                    _db.LikeyAccounts.Add(oLikeyAccount);
                    _db.SaveChanges();

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
                using (var _db = new SocialGrowth2Connection())
                {
                    var oLikeyAccount = _db.LikeyAccounts.Where(g => g.LikeyAccountId == entity.LikeyAccountId).SingleOrDefault();

                    oLikeyAccount.LikeyAccountId = entity.LikeyAccountId;
                    oLikeyAccount.InstaUserName = entity.InstaUserName;
                    oLikeyAccount.InstaPassword = entity.InstaPassword;
                    oLikeyAccount.Country = entity.Country;
                    oLikeyAccount.City = entity.City;
                    oLikeyAccount.Gender = entity.Gender;
                    oLikeyAccount.HashTag = entity.HashTag;
                    oLikeyAccount.StatusId = entity.StatusId;


                    _db.SaveChanges();
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
                using (var _db = new SocialGrowth2Connection())
                {

                    if (LKaccId == 0) return !_db.LikeyAccounts.Any(u => u.InstaUserName.Equals(InstaUserName));
                    var user = _db.LikeyAccounts.Find(LKaccId);
                    if (user.InstaUserName.Equals(InstaUserName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                    return !_db.LikeyAccounts.Any(r => r.InstaUserName.Equals(InstaUserName) && r.LikeyAccountId != LKaccId);
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
                using (var _db = new SocialGrowth2Connection())
                {
                    var pro = _db.LikeyAccounts.Where(g => g.LikeyAccountId == LikeyAccountId).FirstOrDefault();
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
                using (var _db = new SocialGrowth2Connection())
                {
                    var LKaccount=_db.LikeyAccounts.Where( g=> g.LikeyAccountId == LikeyAccountId).FirstOrDefault();
                    if (LKaccount != null)
                    {
                        LKaccount.StatusId = 18;
                        _db.SaveChanges();
                        return true;
                    }
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
                using (var _db = new SocialGrowth2Connection())
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
                using (var _db = new SocialGrowth2Connection())
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
