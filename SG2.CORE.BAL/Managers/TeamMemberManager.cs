using SG2.CORE.COMMON;
using SG2.CORE.DAL.Repositories;
using SG2.CORE.MODAL.DTO.TeamMember;
using SG2.CORE.MODAL.ViewModals.Backend.TeamMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SG2.CORE.COMMON.GlobalEnums;

namespace SG2.CORE.BAL.Managers
{
    public class TeamMemberManager
    {
        private readonly TeamMemberRepository _teamMemberRepository;
        private readonly SessionManager _sessionManager;

        public TeamMemberManager()
        {
            _teamMemberRepository = new TeamMemberRepository();
            _sessionManager = new SessionManager();
        }


        public SystemUserDTO AddSystemUser(SystemUserDTO entity)
        {
            try
            {
                _teamMemberRepository.AddTeamMember(entity);
                return entity;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SystemUserDTO UpdatSystemUser(SystemUserDTO entity)
        {
            try
            {
                _teamMemberRepository.UpdateTeamMember(entity);
                return entity;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SystemUserDTO GetSystemUserId(int SystemUserId)
        {
            try
            {
                SystemUserDTO systemUserDTO = new SystemUserDTO();
                systemUserDTO = _teamMemberRepository.GetTeamMemberById(SystemUserId);
                return systemUserDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<TeamMemberListingViewModal> GetTeamMemberData(string SearchCriteria, string PageSize, int PageNumber, int? StatusId)
        {
            var Model = _teamMemberRepository.GetTeamMemberData(SearchCriteria, PageNumber, PageSize, StatusId);
            return Model;
        }


        public IList<SystemRoleDTO> GetAllRoleList()
        {
            var Model = _teamMemberRepository.GetAllRoleList();
            return Model;
        }

        public bool IsEmailExist(string email, int id = 0)
        {
            try
            {
                return _teamMemberRepository.IsEmailExist(email, id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ErrorMessagesDTO LoginUser(string username, string password)
        {
            try
            {
                ErrorMessagesDTO errorMessagesDTO = new ErrorMessagesDTO();
                var user = _teamMemberRepository.GetLogin(username, password);
                if (user != null)
                {
                    if (user.StatusId == (short)GlobalEnums.SystemUserStatus.Active)
                    {
                        _sessionManager.Set(SessionConstants.SystemUser, user);
                        errorMessagesDTO.IsError = false;

                    }
                    else if (user.StatusId == (short)GlobalEnums.SystemUserStatus.Invited)
                    {
                        errorMessagesDTO.IsError = true;
                        errorMessagesDTO.ErrorMessage = "Your email address is not verified.";
                    }
                    else if (user.StatusId == (short)GlobalEnums.SystemUserStatus.Suspend)
                    {
                        errorMessagesDTO.IsError = true;
                        errorMessagesDTO.ErrorMessage = "This account has been suspened. Please contact your administrator.";
                    }
                    else if (user.StatusId == (short)GlobalEnums.SystemUserStatus.Delete)
                    {
                        errorMessagesDTO.IsError = true;
                        errorMessagesDTO.ErrorMessage = "This account has been deleted. Please contact your administrator.";
                    }
                    else
                    {

                        errorMessagesDTO.IsError = true;
                        errorMessagesDTO.ErrorMessage = "Please enter a valid email address or password.";
                    }

                    return errorMessagesDTO;
                }
                errorMessagesDTO.IsError = true;
                errorMessagesDTO.ErrorMessage = "Please enter a valid email address or password.";
                return errorMessagesDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateCustomerPassword(string password, int customerId)
        {
            try
            {
                var Customer = _teamMemberRepository.UpdateCustomerPassword(password, customerId);
                return Customer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
