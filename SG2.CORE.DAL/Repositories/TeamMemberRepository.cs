using SG2.CORE.DAL.DB;
using SG2.CORE.MODAL.DTO.TeamMember;
using SG2.CORE.MODAL.ViewModals.Backend.TeamMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SG2.CORE.MODAL;

namespace SG2.CORE.DAL.Repositories
{
    public class TeamMemberRepository
    {
        public SystemUserDTO AddTeamMember(SystemUserDTO entity)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var oUser = new SystemUser();
                    oUser.Title = entity.Title;
                    oUser.FirstName = entity.FirstName;
                    oUser.LastName = entity.LastName;
                    oUser.Email = entity.Email;
                    oUser.SystemRoleId = entity.SystemRoleId;
                    oUser.Password = entity.Password;
                    oUser.StatusId = entity.StatusId;
                    oUser.CreatedBy = entity.CreatedBy;
                    oUser.CreatedOn = DateTime.Now;
                    oUser.ModifiedBy = entity.ModifiedBy;
                    oUser.ModifiedOn = DateTime.Now;


                    _db.SystemUsers.Add(oUser);
                    _db.SaveChanges();
                    entity.SystemUserId = oUser.SystemUserId;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SystemUserDTO UpdateTeamMember(SystemUserDTO entity)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var oUser = _db.SystemUsers.Where(g => g.SystemUserId == entity.SystemUserId).SingleOrDefault();
                    oUser.Title = entity.Title;
                    oUser.FirstName = entity.FirstName;
                    oUser.LastName = entity.LastName;
                    oUser.Email = entity.Email;
                    oUser.SystemRoleId = entity.SystemRoleId;
                    oUser.Password = entity.Password;
                    oUser.StatusId = entity.StatusId;
                    oUser.CreatedBy = entity.CreatedBy;
                    oUser.CreatedOn = DateTime.Now;
                    oUser.ModifiedBy = entity.ModifiedBy;
                    oUser.ModifiedOn = DateTime.Now;


                   
                    _db.SaveChanges();
                    return entity;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SystemUserDTO GetTeamMemberById(int SystemUserId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var user = _db.SystemUsers.Where(g => g.SystemUserId == SystemUserId).SingleOrDefault();
                    if (user != null)
                    {
                        SystemUserDTO systemSettings = new SystemUserDTO()
                        {
                            SystemUserId = user.SystemUserId,
                            Title = user.Title,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Password = user.Password,
                            StatusId = user.StatusId,
                            SystemRoleId = user.SystemRoleId,
                            HostUser = user.HostUser,
                            CreatedOn = user.CreatedOn,
                            CreatedBy = user.CreatedBy,
                            ModifiedOn = user.ModifiedOn,
                            ModifiedBy = user.CreatedBy,

                        };
                        return systemSettings;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<TeamMemberListingViewModal> GetTeamMemberData(string SearchCriteria, int PageNumber, string PageSize, int? StatusId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var teamMemberdata = _db.SG2_usp_SystemUser_GetAll(SearchCriteria, PageNumber, PageSize, StatusId).ToList();
                    if (teamMemberdata != null)
                    {
                        List<TeamMemberListingViewModal> teamMemberListingViewModalList = new List<TeamMemberListingViewModal>();
                        foreach (var item in teamMemberdata)
                        {
                            TeamMemberListingViewModal teamMemberListingViewModal = new TeamMemberListingViewModal();
                            teamMemberListingViewModal.SystemUserId = item.SystemUserId;
                            teamMemberListingViewModal.FullName = item.FullName;
                            teamMemberListingViewModal.RoleName = item.RoleName;
                            teamMemberListingViewModal.Email = item.Email;
                            teamMemberListingViewModal.Status = item.StatusName;
                            teamMemberListingViewModal.PageNumber = PageNumber;
                            teamMemberListingViewModal.TotalRecord = item.TotalRecord;
                            teamMemberListingViewModalList.Add(teamMemberListingViewModal);
                        }
                        return teamMemberListingViewModalList;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IList<SystemRoleDTO> GetAllRoleList()
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var roleList = _db.SystemRoles.Where(g => g.StatusId == 19 && g.RoleId != 1).ToList();
                    if (roleList != null)
                    {
                        List<SystemRoleDTO> SystemRoleList = new List<SystemRoleDTO>();
                        foreach (var item in roleList)
                        {
                            SystemRoleDTO systemRole = new SystemRoleDTO();
                            systemRole.SystemRoleId = item.RoleId;
                            systemRole.RoleName = item.Name;
                            SystemRoleList.Add(systemRole);
                        }
                        return SystemRoleList;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsEmailExist(string email, int id = 0)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    if (id == 0) return !_db.SystemUsers.Any(u => u.Email.Equals(email));
                    var user = _db.SystemUsers.Find(id);
                    if (user.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                    return !_db.SystemUsers.Any(r => r.Email.Equals(email) && r.SystemUserId != id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SystemUserDTO GetLogin(string Email, string Password)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var user = _db.SG2_usp_SystemUser_Login(Email, Password).FirstOrDefault();
                    if (user != null)
                    {
                        SystemUserDTO systemUser = new SystemUserDTO()
                        {
                            SystemUserId = user.SystemUserId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Password = user.Password,
                            StatusId = user.StatusId,
                            SystemRoleId = user.SystemRoleId,
                            HostUser = user.HostUser,
                            CreatedOn = user.CreatedOn,
                            CreatedBy = user.CreatedBy,
                            ModifiedOn = user.ModifiedOn,
                            ModifiedBy = user.CreatedBy,
                            Title      = user.Title,
                            UserRoleName  = user.RoleName,
                            UserName = user.FirstName.FirstOrDefault().ToString() + user.LastName.FirstOrDefault().ToString()
                        };
                        return systemUser;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateCustomerPassword(string password, int systemUserId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var usr = _db.SystemUsers.FirstOrDefault(x => x.SystemUserId == systemUserId);
                    if (usr != null)
                    {
                        usr.Password = password;
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
    }
}
