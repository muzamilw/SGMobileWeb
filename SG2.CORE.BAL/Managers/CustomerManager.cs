using SG2.CORE.DAL.Repositories;
using System;
using static SG2.CORE.COMMON.GlobalEnums;
using SG2.CORE.MODAL.DTO.Customers;
using SG2.CORE.MODAL.ViewModals.Backend;
using System.Collections.Generic;
using SG2.CORE.DAL.DB;
using SG2.CORE.MODAL.ViewModals.Backend.ActionBoard;
using System.Threading.Tasks;
using SG2.CORE.MODAL.DTO.TargetPreferences;
using SG2.CORE.MODAL.DTO.Common;
using SG2.CORE.MODAL.DTO.JVBox;

namespace SG2.CORE.BAL.Managers
{
    public class CustomerManager
    {
        private readonly CustomerRepository _customerRepository;

        private readonly SessionManager _sessionManager;

        public CustomerManager()
        {
            _customerRepository = new CustomerRepository();
            _sessionManager = new SessionManager();
        }

        public bool IsEmailExist(string email, int id = 0)
        {
            try
            {
                return _customerRepository.IsEmailnameExist(email, id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CustomerDTO GetCustomerByEmail(string email)
        {
            try
            {
                return _customerRepository.GetCustomerByEmail(email);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool DeleteCustomerAll(int customerId, int SocialProfileId)
        {
            try
            {
                return _customerRepository.DeleteCustomerAll(customerId, SocialProfileId);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool DeleteCustomer(int customerId, int SocialProfileId)
        {
            try
            {
                return _customerRepository.DeleteCustomerAll(customerId, SocialProfileId);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool LoginUser(string username, string password, ref string errorMessage)
        {
            try
            {
                //todo: remove status
                var cust = _customerRepository.GetLogin(username, password, 1);
                if (cust != null)
                {
                    //todo: status
                    if (cust.StatusId == 7)
                    {
                        errorMessage = "Email verification required.";
                        return false;
                    }
                    else if (cust.StatusId == 6)
                    {
                        errorMessage = "Your account is inactive. Please contact adminstrator.";
                        return false;
                    }
                    else if (cust.StatusId == 4)
                    {
                        errorMessage = "Your account has been deleted.";
                        return false;
                    }
                    else if (cust.StatusId == 5 || cust.StatusId == 1)
                    {
                        _sessionManager.Set(SessionConstants.Customer, cust);
                        return true;
                    }

                }
                errorMessage = "Invalid email or password.";
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<CustomerListingViewModel> GetUserData(string SearchCriteria, string PageSize, int PageNumber, int? StatusId, string ProductId, string JVStatus, int? Subscription)
        {
            var Model = _customerRepository.GetUserData(SearchCriteria, PageNumber, PageSize, StatusId, ProductId, JVStatus, Subscription);

            return Model;
        }

        public IList<CustomerOrderHistoryViewModel> GetCustomerOrderHistory(string PageSize, int PageNumber, int id, int SPId)
        {
            var Model = _customerRepository.GetCustomerOrderHistory(PageSize, PageNumber, id, SPId);
            return Model;

        }

        public IList<CustomerDTO> GetCustomers()
        {
            var Model = _customerRepository.GetCustomers();

            return Model;

        }

        public IList<ProductDTO> GetProductIds()
        {
            var Model = _customerRepository.GetProductIds();
            return Model;
        }

        public bool ScheduleCall(int customerId, DateTime schedule, string notes)
        {
            try
            {
                //return true;
                return _customerRepository.ScheduleCall(customerId, schedule, notes);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool SaveUpdateUserDataIndividually(string value, string fieldName, int customerId, int socialProfileId)
        {
            try
            {
                return _customerRepository.SaveUpdateUserDataIndividually(value, fieldName, customerId, socialProfileId);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public string GetUserComment(int CustomerId)
        {
            try
            {
                return _customerRepository.GetUserComment(CustomerId);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public CustomerDetailViewModel GetSpecificUserData(int CustomerId, int SocialProfileId)
        {
            var Model = _customerRepository.GetCustomerDetail(CustomerId, SocialProfileId);
            return Model;

        }

        public CustomerTargetPreferencesViewModel GetSpecificUserTargettingInformation(int CustomerId, int SocialPId)
        {
            var Model = _customerRepository.GetSpecificUserTargettingInformation(CustomerId, SocialPId);
            return Model;
        }

        public bool SetSocialProfileArchive(int id, int archive)
        {
            try
            {
                var Model = _customerRepository.SetSocialProfileArchive(id, archive);
                return Model;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ActionBoardListingViewModel> GetActionBoardData(int? JVBoxStatusId)
        {
            var Model = _customerRepository.GetActionBoardData(JVBoxStatusId);
            return Model;
        }

        public List<ActionBoardJVSData> GetJVStatuses()
        {
            var Model = _customerRepository.GetJVStatuses();

            return Model;
        }

        //public bool UpdateSuccessfulLogin(CustomerIndexViewModel model)
        //{
        //    var cust = _custRepository.UpdateSuccessfulLogin(new Customer()
        //    {
        //        Id = model.Id,
        //        loginIPAddress = model.loginIPAddress,
        //        LoginDevice = model.LoginDevice,
        //    });

        //    return cust == null;
        //}

        //public bool UpdateFailedLogin(CustomerIndexViewModel model)
        //{
        //    var cust = _custRepository.UpdateFailedLogin(new Customer()
        //    {
        //        Email = model.Email,
        //        CustomerStatus = model.CustomerStatus,
        //        loginIPAddress = model.loginIPAddress,
        //        LoginDevice = model.LoginDevice,
        //    });
        //    return cust == null;
        //}

        //public bool VerifyTokenInfo(string email, string token, short statusId)
        //{
        //    var cust = _custRepository.VerifyTokenInfo(email, token, statusId);
        //    return cust == null;
        //}

        //public bool UpdateToken(string email, string token)
        //{
        //    var cust = _custRepository.UpdateToken(email, token);
        //    return cust == null;
        //}

        public CustomerDTO SignUpCustomer(CustomerDTO model, out string dbError)
        {
            try
            {

                CustomerDTO customerDTO = _customerRepository.SignUp(model, out dbError);
                return customerDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CustomerDTO UpdateCustomerProfile(CustomerDTO model)
        {
            try
            {
                var Customer = _customerRepository.UpdateCustomerProfile(model);

                if (Customer != null)
                {

                    _sessionManager.Set(SessionConstants.Customer, Customer);

                }

                return Customer;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public CustomerDTO UpdateCustomerEmailSubscription(CustomerDTO model)
        {
            try
            {
                var Customer = _customerRepository.UpdateCustomerEmailSubscription(model);
                return Customer;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public JVBoxDTO AssignJVBoxToCustomer(int customerId, int socialProfileId)
        {
            try
            {
                var AssignJVBox = _customerRepository.AssignJVBox(customerId, socialProfileId);
                return AssignJVBox;
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
                var Customer = _customerRepository.UpdateCustomerPassword(password, customerId);
                return Customer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ActivateCustomerPassword(short statusId, int customerId)
        {
            try
            {
                var Customer = _customerRepository.ActivateCustomerAccount(statusId, customerId);
                return Customer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateStripeCustomerId(int CustomerId, string StripCustomerId)
        {
            try
            {
                return await _customerRepository.UpdateStripeCustomerId(CustomerId, StripCustomerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProxyIPDTO> GetProxyIPs(int CountryId, int CityId)
        {
            try
            {
                return _customerRepository.GetProxyIPs(CountryId, CityId);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<JVBoxDTO> GetMPBoxes()
        {
            try
            {
                return _customerRepository.GetMPBoxes();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool UpdateJVStatus(int profileId, int? jvStatus = null)
        {
            try
            {
                return _customerRepository.UpdateJVStatus(profileId, jvStatus);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool UpdateMPBox(int profileId, int? MPBox)
        {
            try
            {
                return _customerRepository.UpdateMPBox(profileId, MPBox);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string GetProxyIp(int profileId)
        {
            try
            {
                return _customerRepository.GetProxyIp(profileId);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool UpdateProxyIp(int profileId, int? proxyIp = null)
        {
            try
            {
                return _customerRepository.UpdateProxyIp(profileId, proxyIp);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public CustomerDTO GetCustomerByCustomerId(int CustomerId)
        {
            try
            {
                return _customerRepository.GetCustomerByCustomerId(CustomerId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SubscriptionDTO> InsertSubscription(SubscriptionDTO sG2_Subscription)
        {
            try
            {
                var sb = await _customerRepository.InsertSubscription(sG2_Subscription);
                return sb;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSubscription(SubscriptionDTO sG2_Subscription)
        {
            try
            {
                _customerRepository.UpdateSubscription(sG2_Subscription);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSubscriptionStatus(int subscriptionId, int statusId)
        {
            try
            {
                _customerRepository.UpdateSubscriptionStatus(subscriptionId, statusId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SubscriptionDTO GetSubscription(int SocialProfileId)
        {
            try
            {
                SubscriptionDTO subscriptionDTO = _customerRepository.GetSubscription(SocialProfileId);

                return subscriptionDTO;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public CustomerDTO SignupCustomerProfileAndPreference(CustomerAndPreferenceDTO model)
        {
            try
            {
                var Customer = _customerRepository.SignupCustomerProfileAndPreference(model);

                if (Customer != null)
                {
                    _sessionManager.Set(SessionConstants.Customer, Customer);
                }

                return Customer;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public int? GetTargetedCityIdByCustomerId(int customerId, int socialProfileId)
        {
            try
            {
                return _customerRepository.GetTargetedCityIdByCustomerId(customerId, socialProfileId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomerSocialProfileDTO> GetSocialProfilesByCustomerid(int customerId)
        {
            try
            {
                return _customerRepository.GetSocialProfilesByCustomerid(customerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public  List<TargetPreferencesDTO> SetPendingTargetPreferenceIntoQueue()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //}

        public CustomerTargetProfileDTO GetSocialProfilesById(int profileId)
        {
            try
            {
                return _customerRepository.GetSocialProfilesById(profileId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public SubscriptionDTO GetLastCancelledSubscription(int profileId, DateTime dateTime)
        {
            try
            {

                return _customerRepository.GetLastCancelledSubscription(profileId, dateTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
          public async Task<bool> UpdateSPNoOfAttempt(int profileId, short NoOfAttempt, DateTime JVAttemptsBlockedTill)
        {
            try
            {
                return await _customerRepository.UpdateSPNoOfAttempt(profileId, NoOfAttempt, JVAttemptsBlockedTill);

                ;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> SetSocialProfileJVStatus(int profileId, int jvStatusId, string updatedBy)
        {
            try
            {
                return await _customerRepository.SetSocialProfileJVStatus(profileId, jvStatusId, updatedBy);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SetTargetPreferenceQueueStatus(int profileId, short queueStatus, string updatedBy)
        {
            try
            {
                return await _customerRepository.SetTargetPreferenceQueueStatus(profileId, queueStatus, updatedBy);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SetTargetPreferenceLikeyStatus(int profileId, short jvLikeyStatus, int JVNoOfLikes, string updatedBy)
        {
            try
            {
                return await _customerRepository.SetTargetPreferenceLikeyStatus(profileId, jvLikeyStatus, JVNoOfLikes, updatedBy);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateNoOfAttempts(int profileId, int noOfAttempts)
        {
            try
            {
                return _customerRepository.UpdateNoOfAttempts(profileId, noOfAttempts);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool BlockProfile24Hrs(int profileId, int noOfAttempts)
        {
            try
            {
                return _customerRepository.BlockProfile24Hrs(profileId, noOfAttempts);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
