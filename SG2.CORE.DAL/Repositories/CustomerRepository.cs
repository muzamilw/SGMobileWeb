using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Text.RegularExpressions;
using SG2.CORE.MODAL.ViewModals.Customers;

namespace SG2.CORE.DAL.Repositories
{
    public class CustomerRepository
    {

        public bool IsEmailnameExist(string email, int id = 0)
        {

           
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    

                    if (id == 0) return !_db.Customers.Any(u => u.EmailAddress.Equals(email));
                    var user = _db.Customers.Find(id);
                    if (user.EmailAddress.Equals(email, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                    return !_db.Customers.Any(r => r.EmailAddress.Equals(email) && r.CustomerId != id);
                }
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
                using (var _db = new SocialGrowth2Connection())
                {
                    var user = _db.Customers.FirstOrDefault(x => x.EmailAddress == email);

                    if (user != null)
                    {
                        if (user.EmailAddress.Equals(email, StringComparison.CurrentCultureIgnoreCase))
                        {
                            CustomerDTO customerDTO = new CustomerDTO();
                            customerDTO.CustomerId = user.CustomerId;
                            customerDTO.FirstName = user.FirstName;
                            customerDTO.SurName = user.SurName;
                            customerDTO.EmailAddress = user.EmailAddress;
                            return customerDTO;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public CustomerDTO SignUp(CustomerDTO entity, out string dbError)
        {
            try
            {
                dbError = "";
                using (var _db = new SocialGrowth2Connection())
                {
                    if (_db.Customers.Any(x => x.EmailAddress == entity.EmailAddress))
                    {
                        dbError = "Email already exists. Please try another email.";
                        return null;
                    }
                    CustomerDTO cusDTO = null;
                    var usr = _db.SG2_usp_Customer_SignUp(entity.FirstName,
                                                            entity.SurName,
                                                            entity.EmailAddress,
                                                            entity.Password,
                                                            entity.CreatedBy,
                                                            entity.GUID,
                                                            entity.LastLoginIP,
                                                            entity.StatusId).FirstOrDefault();
                    if (usr != null)
                    {
                        cusDTO = new CustomerDTO();
                        cusDTO.CustomerId = usr.CustomerId;
                    }
                    return cusDTO;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CustomerDTO UpdateCustomerProfile(CustomerDTO entity)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var customer = _db.Customers.Where(g => g.CustomerId == entity.CustomerId).SingleOrDefault();
                    if (customer != null)
                    {
                        customer.FirstName = entity.FirstName;
                        customer.SurName = entity.SurName;
                        _db.SaveChanges();


                        //update contact

                        var contact = _db.Customer_ContactDetail.Where(g => g.CustomerId == entity.CustomerId).SingleOrDefault();
                        if ( contact != null)
                        {
                            contact.PhoneNumber = entity.PhoneNumber;
                            contact.PhoneCode = entity.PhoneCode;
                            _db.SaveChanges();
                        }

                    }
                    return entity;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CustomerDTO UpdateCustomerEmailSubscription(CustomerDTO entity)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var usr = _db.Customers.FirstOrDefault(x => x.CustomerId == entity.CustomerId);
				
					if (usr != null)
                    {
                        usr.IsOptedEducationalEmailSeries = entity.IsOptedEducationalEmailSeries;
                        usr.IsOptedMarketingEmail = entity.IsOptedMarketingEmail;
                        usr.UpdatedOn = System.DateTime.Now;
                        usr.UpdatedBy = entity.UpdatedBy;
                        _db.SaveChanges();
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
        
   
        public string GetUserComment(int CustomerId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var result = _db.Customers.SqlQuery("SELECT * from Customer where CustomerId=@CustomerId "
                                                   , new SqlParameter("@CustomerId", CustomerId)
                                               ).FirstOrDefault();
                    if (result != null)
                    {
                        return result.Comment;
                    }
                    else
                        return null;
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task<bool> SetSocialProfileJVStatus(int profileId, int jvStatusId, string updatedBy)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var socialProfile = _db.SocialProfiles.FirstOrDefault(m => m.SocialProfileId == profileId);

                    //int? JVBoxStatus = socialProfile.JVBoxStatusId;
                    //int? JVBoxId = socialProfile.JVBoxId;

                    if (socialProfile != null)
                    {
                        if (jvStatusId == 0)
                            socialProfile.TrelloStatusId = null;
                        else
                            socialProfile.TrelloStatusId = jvStatusId;
                        socialProfile.UpdatedBy = updatedBy;
                        socialProfile.UpdatedOn = DateTime.Now;
                        await _db.SaveChangesAsync();
                        return true;
                    }

                    //if (JVBoxStatus != null)
                    //{
                    //    var SPH = _db.Set<SG2_SocialProfile_StatusHistory>();
                    //    SPH.Add(new SG2_SocialProfile_StatusHistory { JVBoxStatusId = JVBoxStatus, JVBoxId = JVBoxId, CreatedDate = DateTime.Now, SocialProfileId = profileId });

                    //    await _db.SaveChangesAsync();
                    //    return true;
                    //}
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool SaveUpdateUserDataIndividually(string value, string fieldName, int customerId, int socialProfileId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    if ((fieldName == "InstaUsrName") || (fieldName == "InstaPassword"))
                    {
                        var user = _db.SocialProfiles.FirstOrDefault(x => x.SocialProfileId == socialProfileId);

                        if (user != null)
                        {
                            if (fieldName == "InstaUsrName")
                            {
                                user.SocialUsername = value;
                                _db.SaveChanges();
                                return true;
                            }
                            else if (fieldName == "InstaPassword")
                            {
                                user.SocialPassword = value;
                                _db.SaveChanges();
                                return true;
                            }
                        }
                    }
                    else if ((fieldName == "firstName") || (fieldName == "LastName") || (fieldName == "Source") || (fieldName == "Comment") || (fieldName == "Title") || (fieldName == "ResTeamMember") || (fieldName == "AvaToEveryOne") || (fieldName == "CustomerStatus"))
                    {
                        var user = _db.Customers.FirstOrDefault(x => x.CustomerId == customerId);

                        if (user != null)
                        {
                            if (fieldName == "firstName")
                            {
                                user.FirstName = value;
                                _db.SaveChanges();
                                return true;
                            }
                            else if (fieldName == "LastName")
                            {
                                user.SurName = value;
                                _db.SaveChanges();
                                return true;
                            }
                            else if (fieldName == "Source")
                            {
                                user.Source = value;
                                _db.SaveChanges();
                                return true;

                            }
                            else if (fieldName == "Comment")
                            {
                                user.Comment = user.Comment + "\r\n" + value;
                                _db.SaveChanges();
                                return true;

                            }
                            else if (fieldName == "Title")
                            {
                                user.Title = value;
                                _db.SaveChanges();
                                return true;
                            }
                            else if (fieldName == "ResTeamMember")
                            {
                                user.ResponsibleTeamMemberId = Convert.ToInt32(value);
                                _db.SaveChanges();
                                return true;

                            }
                            else if (fieldName == "AvaToEveryOne")
                            {

                                user.AvailableToEveryOne = Convert.ToBoolean(Convert.ToInt32(value));
                                _db.SaveChanges();
                                return true;
                            }
                            else if (fieldName == "CustomerStatus")
                            {
                                user.StatusId = Convert.ToInt16(value);
                                _db.SaveChanges();
                                return true;
                            }
                        }

                    }
                    else if ((fieldName == "Tel") || (fieldName == "Town") || (fieldName == "Country") || (fieldName == "Mobile") || (fieldName == "PostalCode") || (fieldName == "AddressLine"))
                    {
                        var user = _db.Customer_ContactDetail.FirstOrDefault(x => x.CustomerId == customerId);
                        if (user != null)
                        {
                            if (fieldName == "Tel")
                            {
                                user.PhoneNumber = value;
                                _db.SaveChanges();
                                return true;

                            }
                            else if (fieldName == "Town")
                            {
                                user.City = value;
                                _db.SaveChanges();
                                return true;

                            }
                            else if (fieldName == "Country")
                            {
                                user.Country = value;
                                _db.SaveChanges();
                                return true;

                            }
                            else if (fieldName == "Mobile")
                            {
                                user.MobileNumber = value;
                                _db.SaveChanges();
                                return true;
                            }
                            else if (fieldName == "PostalCode")
                            {
                                user.PostalCode = value;
                                _db.SaveChanges();
                                return true;

                            }
                            else if (fieldName == "AddressLine")
                            {
                                user.AddressLine1 = value;
                                _db.SaveChanges();
                                return true;
                            }
                        }

                    }
                    return false;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

              

        public bool DeleteCustomer(int customerId, int SocialProfileId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    var profile = _db.SocialProfiles.Where(g => g.SocialProfileId == SocialProfileId && g.CustomerId == customerId).SingleOrDefault();
                    profile.StatusId = 18;

                    _db.SaveChanges();

                    return true;
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool DeleteCustomerAll(int customerId, int SocialProfileId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var actiondata = _db.SG2_Delete_Customer_All(customerId, SocialProfileId);

                    return true;



                }
               
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool DeleteProfile(int customerId, int SocialProfileId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var actiondata = _db.usp_Delete_Profile(SocialProfileId);

                    return true;



                }

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
                using (var _db = new SocialGrowth2Connection())
                {
                    var usr = _db.Customers.FirstOrDefault(x => x.CustomerId == customerId);
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

        public bool ActivateCustomerAccount(short statusId, int customerId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var usr = _db.Customers.FirstOrDefault(x => x.CustomerId == customerId);
                    if (usr != null)
                    {
                        usr.StatusId = statusId;
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


        public bool SetSocialProfileArchive(int id, int archive)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var usr = _db.SocialProfiles.FirstOrDefault(x => x.SocialProfileId == id);
                    if (usr != null)
                    {
                        if (archive == 1)
                            usr.IsArchived = true;
                        else
                            usr.IsArchived = false;
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
        
        //-- TODO: Change Method and move to exact repository
        public List<ActionBoardListingViewModel> GetActionBoardData(int? JVBoxStatusId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var actiondata = _db.SG2_usp_GetProfilebyJVStatusId(JVBoxStatusId).ToList();
                    if (actiondata != null)
                    {
                        List<ActionBoardListingViewModel> actionBoardViewModelList = new List<ActionBoardListingViewModel>();

                        foreach (var item in actiondata)
                        {
                            ActionBoardListingViewModel actionBoardViewModel = new ActionBoardListingViewModel();
                            actionBoardViewModel.InstaUsrName = item.InstaUsrName ?? "--";
                            actionBoardViewModel.Status = item.JVStatusName;
                            actionBoardViewModel.UserName = item.FullName ?? "--";
                            actionBoardViewModel.CustomerId = HttpUtility.UrlEncode(CryptoEngine.Encrypt(item.CustomerId.ToString()));
                            actionBoardViewModel.Email = item.Email;
                            actionBoardViewModel.ProfileId = HttpUtility.UrlEncode(CryptoEngine.Encrypt(item.SPId.ToString()));
                            actionBoardViewModel.EnumerationValueId = (short)item.JVStatusId;
                            actionBoardViewModel.Comment = item.Comment ?? "--";
                            actionBoardViewModel.IsArchived = item.IsArchived;
                            actionBoardViewModelList.Add(actionBoardViewModel);
                        }
                        return actionBoardViewModelList;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //-- TODO: Change Method and move to exact repository
        public CustomerTargetPreferencesViewModel GetSpecificUserTargettingInformation(int CustomerId, int SocialPId)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<SocialProfile_Instagram_TargetingInformation, CustomerTargetPreferencesViewModel>());
            var mapper = new Mapper(config);
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var actiondata = _db.SocialProfile_Instagram_TargetingInformation.Where(g => g.SocialProfileId == SocialPId).SingleOrDefault();

                    var profile = _db.SocialProfiles.Where(g => g.SocialProfileId == SocialPId).SingleOrDefault();
                    if (actiondata != null)
                    {
                        CustomerTargetPreferencesViewModel customerTargetPreferencesViewModel = mapper.Map<CustomerTargetPreferencesViewModel>(actiondata);

                        customerTargetPreferencesViewModel.Id = actiondata.SocialProfileId.ToString();
                        customerTargetPreferencesViewModel.SPId = actiondata.SocialProfileId.ToString();

                        var customer  = _db.Customers.Where(g => g.CustomerId == profile.CustomerId).SingleOrDefault();
                        customerTargetPreferencesViewModel.Notes = customer.Comment;

                        customerTargetPreferencesViewModel.InstaUser = profile.SocialUsername;
                        customerTargetPreferencesViewModel.Email = customer.EmailAddress;


                        
                        return customerTargetPreferencesViewModel;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CustomerDetailViewModel GetCustomerDetail(int CustomerId, int profileId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    //-- TODO: Change proper sp
                    var actiondata = _db.SG2_usp_Get_SpecificCustomerDetail(CustomerId, profileId).FirstOrDefault();
                    if (actiondata != null)
                    {
                        CustomerDetailViewModel customerDetailViewModel = new CustomerDetailViewModel();
                        customerDetailViewModel.Id = Convert.ToString(actiondata.Id);
                        customerDetailViewModel.FirstName = actiondata.FirstName;
                        customerDetailViewModel.InstaUsrName = actiondata.InstaUsrName;
                        //customerDetailViewModel.IPNo = Convert.ToString(actiondata.IPNO);
                        //customerDetailViewModel.JVBoxNo = actiondata.JVBoxNo;
                        //customerDetailViewModel.JVStatus = actiondata.JVStatus;
                        customerDetailViewModel.LastName = actiondata.LastName;
                        customerDetailViewModel.Mobile = actiondata.Mobile;
                        customerDetailViewModel.Name = actiondata.Name;
                        customerDetailViewModel.OptedEdEmailSeries = Convert.ToBoolean(actiondata.OptedEdEmailSeries);
                        customerDetailViewModel.OptedMarkEmail = Convert.ToBoolean(actiondata.OptedMarkEmail);
                        customerDetailViewModel.PostalCode = actiondata.PostalCode;
                        customerDetailViewModel.Register = actiondata.Register;
                        customerDetailViewModel.ResTeamMember = actiondata.ResTeamMember;
                        customerDetailViewModel.Source = actiondata.Source;
                        //customerDetailViewModel.Status = actiondata.Status;
                        customerDetailViewModel.Tel = actiondata.Tel;
                        customerDetailViewModel.Title = actiondata.Title;
                        customerDetailViewModel.City = actiondata.City;
                        customerDetailViewModel.Email = actiondata.Email;
                        customerDetailViewModel.Comment = actiondata.Comment;
                        customerDetailViewModel.UpdatedOn = actiondata.UpdatedOn.ToString("MMMM dd yyyy HH:mm:ss");
                        customerDetailViewModel.InstaPassword = actiondata.InstaPassword;
                        customerDetailViewModel.AvaToEveryOne = actiondata.AvaToEveryOne;
                        customerDetailViewModel.Country = actiondata.Country;
                        customerDetailViewModel.AddressLine = actiondata.AddressLine;
                        customerDetailViewModel.Tel = actiondata.Tel;
                        customerDetailViewModel.SocialProfileId = Convert.ToString(profileId);
                        customerDetailViewModel.SocialProfileName = actiondata.SocialProfileName;
                        customerDetailViewModel.CustomerStatus = actiondata.CustomerStatus.ToString();
                        customerDetailViewModel.IsArchived = actiondata.IsArchived;
                        //customerDetailViewModel.ProxyPort = actiondata.ProxyPort;
                        return customerDetailViewModel;
                    }
                    return null;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //-- TODO: Change Method and move to exact repository
        public IList<ProductDTO> GetProductIds()
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var PrdDdata = _db.PaymentPlans;
                    if (PrdDdata != null)
                    {
                        List<ProductDTO> productDTOsList = new List<ProductDTO>();

                        foreach (var item in PrdDdata)
                        {
                            ProductDTO productDTO = new ProductDTO();
                            productDTO.Name = item.PlanName;
                            productDTO.StripePlanId = item.StripePlanId;
                            productDTOsList.Add(productDTO);
                        }
                        return productDTOsList;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IList<CustomerDTO> GetCustomers()
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    //-- TODO: Change Proper SP
                    var Customers = _db.Customers.ToList();
                    if (Customers != null)
                    {
                        List<CustomerDTO> customerDTOs = new List<CustomerDTO>();
                        foreach (var item in Customers)
                        {
                            CustomerDTO customerDTO = new CustomerDTO();

                            customerDTO.CustomerId = item.CustomerId;
                            customerDTO.FirstName = item.FirstName + ' '  + item.SurName;
                            customerDTOs.Add(customerDTO);


                        }
                        return customerDTOs;

                    }
                    return null;

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<CustomerOrderHistoryViewModel> GetCustomerOrderHistory(string PageSize, int PageNumber, int id, int SPId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    //-- TODO: Change SP to proper name and
                    var usrHData = _db.SG2_usp_Get_CustomerOrderHistory(id, SPId, PageNumber, PageSize).ToList();
                    if (usrHData != null)
                    {
                        List<CustomerOrderHistoryViewModel> orderHistoryViewModels = new List<CustomerOrderHistoryViewModel>();
                        foreach (var item in usrHData)
                        {
                            CustomerOrderHistoryViewModel orderHistoryViewModel = new CustomerOrderHistoryViewModel();
                            orderHistoryViewModel.Id = Convert.ToString(id);
                            orderHistoryViewModel.SPId = Convert.ToString(SPId);
                            orderHistoryViewModel.Name = item.SubscrpName;
                            orderHistoryViewModel.StartDate = item.StartDate;
                            orderHistoryViewModel.EndDate = item.EndDate;
                            orderHistoryViewModel.Status = item.Status;
                            orderHistoryViewModel.TotalRecords = item.TotalRecord;
                            orderHistoryViewModel.Price = item.Price;
                            orderHistoryViewModel.SPName = item.SProfileName;
                            orderHistoryViewModel.SPStatus = item.SocialProfileStatus;
                            orderHistoryViewModel.SusrName = item.SProfileUsrName;
                            orderHistoryViewModel.UserName = item.UserName;
                            orderHistoryViewModel.Email = item.Email;
                            //orderHistoryViewModel.JVStatus = item.JVBoxStatus;
                            orderHistoryViewModels.Add(orderHistoryViewModel);
                        }
                        return orderHistoryViewModels;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IList<CustomerListingViewModel> GetUserData(string SearchCriteria, int PageNumber, string PageSize, int? Status, string ProductId, string JVStatus, int? Subscription, int? profileType, int? BlockId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    //-- TODO: Change proper sp
                    var usrdata = _db.SG2_usp_GetUserDetailsForbackOffice(SearchCriteria, PageNumber, PageSize, Status, (ProductId), JVStatus, Subscription,profileType, BlockId).ToList();
                    if (usrdata != null)
                    {
                        List<CustomerListingViewModel> customerListingViewModelsList = new List<CustomerListingViewModel>();
                        foreach (var item in usrdata)
                        {
                            CustomerListingViewModel customerListingViewModel = new CustomerListingViewModel();
                            //customerListingViewModel.Proxy = item.ProxyIPNumber;
                            customerListingViewModel.Products = item.Products;
                            customerListingViewModel.Status = item.Status;
                            customerListingViewModel.JVBoxStatus = item.JVBoxStatus;
                            customerListingViewModel.Name = item.UserName;
                            customerListingViewModel.InstaUsrName = item.InstaName;
                            customerListingViewModel.ID = Convert.ToString(item.CustomerId);
                            customerListingViewModel.BrokerAccount = item.IsBroker;
                            customerListingViewModel.TotalRecord = item.TotalRecord;
                            customerListingViewModel.SocialProfileId = Convert.ToString(item.SocialProfileId);
                            customerListingViewModel.SocialProfileName = item.SocialProfileName;
                            customerListingViewModel.CustomerEmail = item.CustomerEmail;
                            customerListingViewModel.FollowOn = item.FollowOn;
                            customerListingViewModel.UnFollFollowersAfterMinDays = item.UnFollFollowersAfterMinDays;
                            customerListingViewModel.AfterFollLikeuserPosts = item.AfterFollLikeuserPosts;
                            customerListingViewModel.AfterFollViewUserStory = item.AfterFollViewUserStory;
                            customerListingViewModel.BlockStatus = item.BlockedStatus;
                            customerListingViewModel.BrokerAppName = item.BrokerAppName;

                            customerListingViewModelsList.Add(customerListingViewModel);
                        }
                        return customerListingViewModelsList;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public (bool, string, int, bool, int ErrorCode, int CustomerId, bool IsBroker, int BlockCode, DateTime BlockDateTimeUTC, bool InitialStatsReceived) PerformMobileLogin(MobileLoginRequest model)
        {


            model.ForceSwitchDevice = true;//overrising the flag and always forcing login.
            using (var _db = new SocialGrowth2Connection())
            {
                var profile = _db.SocialProfiles.Where(g => g.SocialUsername == model.Email && g.PinCode == model.Pin).SingleOrDefault();
                if (profile != null)
                {
                    var Customer = _db.Customers.Where(g => g.CustomerId == profile.CustomerId).Single();

                    if (string.IsNullOrEmpty(profile.DeviceIMEI))
                    {
                        profile.DeviceIMEI = model.IMEI;
                        profile.DeviceStatus = (int)DeviceStatus.Connected;
                        profile.LastConnectedDateTime = DateTime.Now;
                        profile.AppTimeZoneOffSet = model.AppTimeZoneOffSet ?? "";
                        profile.AppVersion = model.AppVersion ?? "";
                        _db.SaveChanges();
                        return (true, "Login Sucessful", profile.SocialProfileId, string.IsNullOrEmpty( profile.SocialPassword),1, profile.CustomerId.Value, Customer.IsBroker.HasValue ? Customer.IsBroker.Value:false, profile.BlockedStatus??0, profile.BockedSinceDateTime.HasValue ? profile.BockedSinceDateTime.Value.ToUniversalTime():DateTime.MinValue , profile.InitialStatsReceived?? false);
                    }
                    else if (profile.DeviceIMEI == model.IMEI)
                    {
                        profile.DeviceIMEI = model.IMEI;
                        profile.DeviceStatus = (int)DeviceStatus.Connected;
                        profile.LastConnectedDateTime = DateTime.Now;
                        profile.AppTimeZoneOffSet = model.AppTimeZoneOffSet ?? "";
                        profile.AppVersion = model.AppVersion ?? "";
                        _db.SaveChanges();
                        return (true, "Login Sucessful", profile.SocialProfileId, string.IsNullOrEmpty(profile.SocialPassword),1, profile.CustomerId.Value, Customer.IsBroker.HasValue ? Customer.IsBroker.Value : false, profile.BlockedStatus ?? 0, profile.BockedSinceDateTime.HasValue ? profile.BockedSinceDateTime.Value.ToUniversalTime() : DateTime.MinValue, profile.InitialStatsReceived ?? false);
                    }
                    else if (profile.DeviceIMEI != model.IMEI && model.ForceSwitchDevice == false)
                    {
                        return (false, "Device IMEI does not match", profile.SocialProfileId,false,2,0,false,0,DateTime.MinValue,false);
                    }
                    else if (profile.DeviceIMEI != model.IMEI && model.ForceSwitchDevice == true)
                    {
                        profile.DeviceIMEI = model.IMEI;
                        profile.DeviceStatus = (int)DeviceStatus.Connected;
                        profile.LastConnectedDateTime = DateTime.Now;
                        _db.SaveChanges();
                        return (true, "Login Sucessful", profile.SocialProfileId, string.IsNullOrEmpty(profile.SocialPassword),1, profile.CustomerId.Value, Customer.IsBroker.HasValue ? Customer.IsBroker.Value : false, profile.BlockedStatus ?? 0, profile.BockedSinceDateTime.HasValue ? profile.BockedSinceDateTime.Value.ToUniversalTime() : DateTime.MinValue, profile.InitialStatsReceived ?? false);
                    }
                    else
                    {
                        return (false, "Device IMEI does not match", profile.SocialProfileId, false,2,0,false,0,DateTime.MinValue,false);
                    }


                }
                //else if(profile.BlockedStatus != 0)
                //{
                //    return (false, "Profile Blocked", 0, false, (int)profile.BlockedStatus, 0, false);
                //}
                else
                {
                    return (false, "Invalid username or Pin", 0,false,3,0,false,0,DateTime.MinValue,false);
                }

            }
        }


        public CustomerDTO GetLogin(string username, string password, short statusId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    //-- TODO: Change sp to propername
                    var usr = _db.SG2_usp_Login_Customers(username, password, username, username, statusId).FirstOrDefault();
                    if (usr != null)
                    {
                        CustomerDTO cst = new CustomerDTO()
                        {
                            GUID = usr.GUID,
                            CustomerId = usr.CustomerId,
                            FirstName = usr.FirstName,
                            EmailAddress = usr.EmailAddress,
                            SurName = usr.SurName,
                            PhoneCode = usr.PhoneCode,
                            PhoneNumber = usr.PhoneNumber,
                            Password = usr.Password,
                            StripeCustomerId = usr.StripeCustomerId,
                            IsOptedEducationalEmailSeries = Convert.ToBoolean(usr.IsOptedEducationalEmailSeries),
                            IsOptedMarketingEmail = Convert.ToBoolean(usr.IsOptedMarketingEmail),
                            StatusId = usr.StatusId,
                            StripeSubscriptionId = usr.StripeSubscriptionId,
                            DefaultSocialProfileId = usr.DefaultSocialProfileId.Value,
                            
                        };

                        var cust = _db.Customers.Where(g => g.CustomerId == usr.CustomerId).SingleOrDefault();
                        if (cust != null)
                        {
                            cst.IsBroker = cust.IsBroker;
                            cst.BrokerPaymentPlanID = cust.BrokerPaymentPlanID;
                        }
                        return cst;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ScheduleCall(int customerId, DateTime schedule, string notes)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    var contact = _db.Customer_ContactDetail.Where(g => g.CustomerId == customerId).FirstOrDefault();
                    
                    if (contact != null)
                    {
                        contact.Notes = notes;
                        contact.ScheduleCallDate = schedule;
                        _db.SaveChanges();
                    }
                    return false;
                }
            }

            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool UpdateCustomerBrokerProfile(CustomerBrokerProfileRequest model)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var cus = _db.Customers.FirstOrDefault(x => x.CustomerId == model.cid);
                    if (cus != null)
                    {
                        cus.BrokerAppName = model.BrokerAppName;
                        cus.BrokerAspectColor = model.BrokerAspectColor;
                        cus.BrokerFeedbackPage = model.BrokerFeedbackPage;
                        cus.BrokerHomePage = model.BrokerHomePage;
                        if (!string.IsNullOrEmpty( model.BrokerLogo))
                            cus.BrokerLogo = model.BrokerLogo;
                        cus.BrokerPrivacyPolicy = model.BrokerPrivacyPolicy;
                        cus.BrokerStrapLine = model.BrokerStrapLine;
                        cus.BrokerTermsOfUse = model.BrokerTermsOfUse;
                        cus.BrokerTrainingLink = model.BrokerTrainingLink;
                        cus.BrokerTrustPilotCode = model.BrokerTrustPilotCode;

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

        public bool UpdateCustomerStripeCustomerId(int CustomerId, string StripCustomerId, string StripeSubscriptionId, int PaymentPlanId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var cus = _db.Customers.FirstOrDefault(x => x.CustomerId == CustomerId);
                    if (cus != null)
                    {
                        cus.StripeCustomerId = StripCustomerId;
                        cus.StripeSubscriptionId = StripeSubscriptionId;
                        cus.BrokerPaymentPlanID = PaymentPlanId == 0 ? (int?)null : PaymentPlanId;
                        if (string.IsNullOrEmpty(StripCustomerId) || string.IsNullOrEmpty(StripeSubscriptionId))
                        {
                            cus.IsBroker = false;
                            cus.StripeSubscriptionId = null;
                            cus.StripeCustomerId = null;

                            _db.Customer_Payments.Add(new Customer_Payments { CustomerId = cus.CustomerId, PaymentPlanId = 1, StatusId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now, Name= "Subscrption set to Free", Description = "Subscrption set to Free", Price = 0 });
                        }
                        else
                            cus.IsBroker = true;
                        _db.SaveChanges();


                        //set all profiles to free if afilliate subscription is null 
                        if (string.IsNullOrEmpty(StripCustomerId) || string.IsNullOrEmpty(StripeSubscriptionId))
                        {
                            var profiles = _db.SocialProfiles.Where(g => g.CustomerId == CustomerId).ToList();
                            foreach (var profile in profiles)
                            {
                                profile.StripeSubscriptionId = null;
                                profile.PaymentPlanId = null;
                                profile.StatusId = 25;
                                profile.StripeSubscriptionId = null;
                                profile.StripeCustomerId = null;


                                _db.SocialProfile_Payments.Add(new SocialProfile_Payments { SocialProfileId = profile.SocialProfileId, PaymentPlanId = 1, StatusId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now, Name = "Subscrption set to Free", Description = "Subscrption set to Free", Price = 0 });

                            }
                            _db.SaveChanges();

                        }
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
        public bool UpdateSocialProfileStripeCustomerId(int SocialProfileId, string StripCustomerId, string StripeSubscriptionId, int PaymentPlanId, PlanSubscription status = PlanSubscription.ActivePlan)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var cus = _db.SocialProfiles.FirstOrDefault(x => x.SocialProfileId == SocialProfileId);
                    if (cus != null)
                    {
                        cus.StripeCustomerId = StripCustomerId;
                        cus.StripeSubscriptionId = StripeSubscriptionId;
                        cus.PaymentPlanId = PaymentPlanId;
                        cus.StatusId = (int)status;
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




        public CustomerDTO GetCustomerDTOByCustomerId(int CustomerId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var usr = _db.SG2_usp_Customers_Get(CustomerId).FirstOrDefault();
                    if (usr != null)
                    {
                        CustomerDTO cst = new CustomerDTO()
                        {
                            GUID = usr.GUID,
                            CustomerId = usr.CustomerId,
                            FirstName = usr.FirstName,
                            EmailAddress = usr.EmailAddress,
                            SurName = usr.SurName,
                            PhoneCode = usr.PhoneCode,
                            PhoneNumber = usr.PhoneNumber,
                            Password = usr.Password,
                            StripeCustomerId = usr.StripeCustomerId,
                            IsOptedEducationalEmailSeries = Convert.ToBoolean(usr.IsOptedEducationalEmailSeries),
                            IsOptedMarketingEmail = Convert.ToBoolean(usr.IsOptedMarketingEmail),
                            StatusId = usr.StatusId,
                            
                            //StripeSubscriptionId = usr.StripeSubscriptionId
                        };
                        return cst;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Customer GetCustomerByCustomerId(int CustomerId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    return _db.Customers.Where(g=> g.CustomerId == CustomerId).FirstOrDefault();
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CustomerSocialProfileDTO> GetSocialProfilesWithoutExistingFollowers(int socialProfileId)
        {
            try
            {
                List<CustomerSocialProfileDTO> profiles = null;
                using (var _db = new SocialGrowth2Connection())
                {
                    var profs = (from m in _db.SocialProfiles
                                 join r in _db.SocialProfile_FollowedAccounts on socialProfileId equals r.SocialProfileId
                                 where m.SocialUsername != r.FollowedSocialUsername && m.StatusId == 25 && m.SocialUsername != null
                                 select m)
                                 .OrderBy(x => Guid.NewGuid()).Take(20).ToList();

                    if (profs != null)
                    {
                        profiles = profs.Select(p => new CustomerSocialProfileDTO()
                        {
                            CustomerId = p.CustomerId,

                            ProfileName = p.SocialProfileName,
                            SocialPassword = p.SocialPassword,
                            SocialProfileId = p.SocialProfileId,
                            SocialProfileTypeId = p.SocialProfileTypeId,
                            SocialProfileTypeName = p.SocialProfileTypeId == 30 ? "Instagram" : "Other",
                            SocialUsername = p.SocialUsername,
                            StatusId = p.StatusId,
                       
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

        public async Task<SocialProfile_PaymentsDTO> InsertCustomerPaymentRecord(SocialProfile_PaymentsDTO model)
        {
            using (var _db = new SocialGrowth2Connection())
            {
                try
                {
                    Customer_Payments payrec = new Customer_Payments { Description = model.Description, StripeSubscriptionId = model.StripeSubscriptionId, CustomerId = model.SocialProfileId, EndDate = model.EndDate, Name = model.Name, PaymentDateTime = model.PaymentDateTime, PaymentPlanId = model.PaymentPlanId, Price = model.Price, StartDate = model.StartDate, StatusId = 1, StripeInvoiceId = model.StripeInvoiceId, StripePlanId = model.StripePlanId, SubscriptionType = "month" };
                    _db.Customer_Payments.Add(payrec);
                    _db.SaveChanges();

                    model.PaymentId = payrec.PaymentId;

                    return model;

                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
        }

        public async Task<SocialProfile_PaymentsDTO> InsertSocialProfilePaymentRecord(SocialProfile_PaymentsDTO model)
        {
            using (var _db = new SocialGrowth2Connection())
            {
                try
                {
                    SocialProfile_Payments payrec = new SocialProfile_Payments { Description  = model.Description, StripeSubscriptionId = model.StripeSubscriptionId, SocialProfileId = model.SocialProfileId , EndDate = model.EndDate, Name = model.Name, PaymentDateTime = model.PaymentDateTime, PaymentPlanId = model.PaymentPlanId, Price = model.Price, StartDate = model.StartDate, StatusId = 1, StripeInvoiceId = model.StripeInvoiceId, StripePlanId = model.StripePlanId, SubscriptionType = "month" };
                    _db.SocialProfile_Payments.Add(payrec);
                    _db.SaveChanges();

                    model.PaymentId = payrec.PaymentId;

                    return model;
                 
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
        }

        //TODO: Not usable
        public void UpdateSubscription(SocialProfile_PaymentsDTO subscriptionDTO)
        {
            using (var _db = new SocialGrowth2Connection())
            {

                SocialProfile_Payments subscription = new SocialProfile_Payments();

                var query =
               from sub in _db.SocialProfile_Payments
               where sub.PaymentId == subscriptionDTO.PaymentId
               select sub;

                subscription = query.FirstOrDefault();
                // Execute the query, and change the column values
                // you want to change.
                subscription.PaymentId = subscriptionDTO.PaymentId;
                subscription.Description = subscriptionDTO.Description;
                subscription.Name = subscriptionDTO.Description;
                subscription.Price = subscriptionDTO.Price;
                subscription.StripePlanId = subscriptionDTO.StripePlanId;
                subscription.SubscriptionType = subscriptionDTO.SubscriptionType;
                subscription.StartDate = subscriptionDTO.StartDate;
                subscription.EndDate = subscriptionDTO.EndDate;
                subscription.StatusId = subscriptionDTO.StatusId;

                // Submit the changes to the database.
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Provide for exceptions.
                }

            }
        }

        
        public void UpdateSubscriptionStatus(int subscriptionId, int statusId)
        {
            using (var _db = new SocialGrowth2Connection())
            {

                SocialProfile_Payments subscription = new SocialProfile_Payments();

                var query =
               from sub in _db.SocialProfile_Payments
               where sub.PaymentId == subscriptionId
                     && sub.StatusId == (int)GlobalEnums.PlanSubscription.Active
               select sub;

                subscription = query.FirstOrDefault();
                subscription.PaymentId = subscriptionId;
                subscription.StatusId = statusId;

                // Submit the changes to the database.
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Provide for exceptions.
                }

            }
        }

        public SocialProfile_PaymentsDTO GetSubscription(int SocialProfileId)
        {
            try
            {
                SocialProfile_Payments sub = new SocialProfile_Payments();
                using (var _db = new SocialGrowth2Connection())
                {

                    return (from m in _db.SocialProfile_Payments
                                                                      join r in _db.PaymentPlans on m.PaymentPlanId equals r.PaymentPlanId
                                                                      join ps in _db.EnumerationValues on m.StatusId equals ps.EnumerationValueId
                                                                      select new SocialProfile_PaymentsDTO { Description = m.Description, EndDate = m.EndDate, PaymentPlanId = m.PaymentPlanId, Name = m.Name, PaymentDateTime = m.PaymentDateTime, PaymentId = m.PaymentId, PaymentPlanName = r.PlanName, Price = m.Price, SocialProfileId = m.SocialProfileId, StartDate = m.StartDate, Status = ps.Name, StatusId = m.StatusId, StripeInvoiceId = m.StripeInvoiceId, StripePlanId = m.StripePlanId, StripeSubscriptionId = m.StripeSubscriptionId, SubscriptionType = m.SubscriptionType }).ToList()
                                                                      .Where( g=> g.SocialProfileId == SocialProfileId && g.StatusId == 25).SingleOrDefault();
                }


              

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        

        public CustomerDTO SignupCustomerProfileAndPreference(CustomerAndPreferenceDTO entity)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var usr = _db.SG2_usp_Customer_SignUpCustomerWithPreference(entity.FirstName, entity.Lastname, entity.EmailAddress, entity.Password, entity.GUID, entity.LastLoginIP, entity.Preference1, entity.Preference2, entity.Preference3, entity.Preference4, entity.Preference5, entity.Preference6, int.Parse(entity.PreferLocation), entity.StatusId).FirstOrDefault();
                    if (usr != null)
                    {
                        CustomerDTO cst = new CustomerDTO()
                        {
                            GUID = usr.GUID,
                            CustomerId = usr.CustomerId,
                            FirstName = usr.FirstName,
                            EmailAddress = usr.EmailAddress,
                            SurName = usr.SurName,
                            Password = usr.Password,
                            SocialProfileId = usr.SocialProfileId,
                            StripeCustomerId = Convert.ToString(usr.StripeCustomerId)
                        };
                        return cst;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public int? GetTargetedCityIdByCustomerId(int customerId, int socialProfileId)
        //{
        //    try
        //    {
        //        using (var _db = new SocialGrowth2Connection())
        //        {
        //            var cust = _db.SocialProfiles.FirstOrDefault(x => x.SocialProfileId == socialProfileId);//TODO add SocialProfileId
        //            if (cust != null)
        //            {
        //                return (cust.SocialPrefferedCity);
        //            }
        //            return 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        public List<CustomerSocialProfileDTO> GetSocialProfilesByCustomerid(int customerId, ProfilesSearchRequest model)
        {
            try
            {
                List<CustomerSocialProfileDTO> profiles = null;
                using (var _db = new SocialGrowth2Connection())
                {
                    var profs = _db.SG2_usp_Customer_GetSocialProfilesByCustomerId(customerId,model.searchString,model.Plan,model.SocialType,model.Block).ToList();
                    if (profs != null)
                    {
                        profiles = profs.Select(p => new CustomerSocialProfileDTO()
                        {
                            CustomerId = p.CustomerId,
                           
                            ProfileName = p.ProfileName,
                            SocialPassword = p.SocialPassword,
                            SocialProfileId = p.SocialProfileId,
                            SocialProfileTypeId = p.SocialProfileTypeId,
                            SocialProfileTypeName = p.SocialProfileTypeId == 30 ? "Instagram": "Other",
                            SocialUsername = p.SocialUsername,
                            StatusId = p.StatusId,
                            StatusName = p.StatusName,
                            StripeCustomerId = p.StripeCustomerId,
                            SubScriptionStatus = p.SubscriptionStatus,
                            SubscriptionName = p.SubscriptionName,
                            BlockedStatus =  p.BlockedStatus.HasValue == false || (p.BlockedStatus.HasValue && p.BlockedStatus.Value == 0) ? "Valid" : p.BlockedStatus.Value == 66 ? "Action Blocked": p.BlockedStatus.Value == 67 ?"Hard Blocked" : p.BlockedStatus.Value == 68?"Password Blocked":"Valid",
                             AfterFollLikeuserPosts = p.AfterFollLikeuserPosts,
                              AfterFollViewUserStory = p.AfterFollViewUserStory,
                              FollowOn = p.FollowOn,
                              UnFollFollowersAfterMinDays = p.UnFollFollowersAfterMinDays

                            
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

        public SocialProfile_PaymentsDTO GetLastCancelledSubscription(int profileId,  DateTime dateTime)
        {
            SocialProfile_Payments sub = new SocialProfile_Payments();
          
            using (var _db = new SocialGrowth2Connection())
            {

                return (from m in _db.SocialProfile_Payments
                        join r in _db.PaymentPlans on m.PaymentPlanId equals r.PaymentPlanId
                        join ps in _db.EnumerationValues on m.StatusId equals ps.EnumerationValueId
                        select new SocialProfile_PaymentsDTO { Description = m.Description, EndDate = m.EndDate, PaymentPlanId = m.PaymentPlanId, Name = m.Name, PaymentDateTime = m.PaymentDateTime, PaymentId = m.PaymentId, PaymentPlanName = r.PlanName, Price = m.Price, SocialProfileId = m.SocialProfileId, StartDate = m.StartDate, Status = ps.Name, StatusId = m.StatusId, StripeInvoiceId = m.StripeInvoiceId, StripePlanId = m.StripePlanId, StripeSubscriptionId = m.StripeSubscriptionId, SubscriptionType = m.SubscriptionType }).ToList()
                                                  .Where(g => g.SocialProfileId == profileId && g.StatusId == 26).SingleOrDefault();


               
               
               
            }
           
        }


        public bool SaveMobileAppActions(MobileActionRequest model)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var action = new SocialProfile_Actions();
                    if (!string.IsNullOrEmpty( model.ActionDateTime ))
                    {
                        action.ActionDateTime = Convert.ToDateTime(model.ActionDateTime);
                    }
                    else
                        action.ActionDateTime = DateTime.UtcNow;

                    action.ActionID = model.ActionId;
                    action.Message = model.Message;
                    action.SocialProfileId = model.SocialProfileId;
                    action.TargetProfile = model.TargetSocialUserName;

                    _db.SocialProfile_Actions.Add(action);
                    _db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return true;
        }


        public List<SocialProfile_Actions> ReturnLastActions(int socialProfileId, int NoOfActions)

        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                   return  _db.SocialProfile_Actions.Where(g => g.SocialProfileId == socialProfileId && g.ActionID != 69).OrderByDescending(g => g.ActionDateTime).Take(NoOfActions).ToList();
                }
            }
            catch (Exception e)
            {

                throw e;
            }
           
        }

        public SocialProfile GetSocialProfileByStripeSubscriptionId(string StripeSubscriptionId)

        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    return _db.SocialProfiles.Where(g => g.StripeSubscriptionId == StripeSubscriptionId).SingleOrDefault();
                }
            }
            catch (Exception e)
            {

                throw e;
            }

        }


        public SocialProfileDTO GetSocialProfilesById(int profileId)
        {

            //var config = new MapperConfiguration(cfg => cfg.CreateMap<SG2_usp_Customer_GetSocialProfileById_Result, CustomerTargetProfileDTO>());
            //var mapper = new Mapper(config);

            


            try
            {
                SocialProfileDTO profile = new SocialProfileDTO();
                using (var _db = new SocialGrowth2Connection())
                {
                    //global profile loading
                    if (profileId == -999)
                    {
                        var gProfile = _db.SocialProfile_Instagram_TargetingInformation.Where(g => g.IsSystem == true && g.SocialProfileId == null).Single();
                        profileId = gProfile.SocialProfileId.Value;
                    }


                    _db.Configuration.LazyLoadingEnabled = false;
                    //var prof = _db.SG2_usp_Customer_GetSocialProfileById(profileId).FirstOrDefault();
                    //targetProfile = mapper.Map<SocialProfileDTO>(prof);
                    profile.SocialProfile = _db.SocialProfiles.Where(g => g.SocialProfileId == profileId).SingleOrDefault();
                    profile.SocialProfile_Instagram_TargetingInformation = _db.SocialProfile_Instagram_TargetingInformation.Where(g => g.SocialProfileId == profileId).SingleOrDefault();
                    profile.CurrentPaymentPlan = _db.PaymentPlans.Where(g => g.PaymentPlanId == profile.SocialProfile.PaymentPlanId).SingleOrDefault();
                    profile.LastSocialProfile_Payments = (from m in _db.SocialProfile_Payments
                                                          join r in _db.PaymentPlans on m.PaymentPlanId equals r.PaymentPlanId
                                                          join ps in _db.EnumerationValues on m.StatusId equals ps.EnumerationValueId
                                                          where m.SocialProfileId == profileId
                                                          select new SocialProfile_PaymentsDTO { Description = m.Description, EndDate = m.EndDate, PaymentPlanId = m.PaymentPlanId, Name = m.Name, PaymentDateTime = m.PaymentDateTime, PaymentId = m.PaymentId, PaymentPlanName = r.PlanName, Price = m.Price, SocialProfileId = m.SocialProfileId, StartDate = m.StartDate, Status = ps.Name, StatusId = m.StatusId, StripeInvoiceId = m.StripeInvoiceId, StripePlanId = m.StripePlanId, StripeSubscriptionId = m.StripeSubscriptionId, SubscriptionType = m.SubscriptionType }).ToList();
                                                          
                    profile.SocialProfile_FollowedAccounts = _db.SocialProfile_FollowedAccounts.Where(g => g.SocialProfileId == profileId).OrderByDescending(g => g.FollowedDateTime).ToList();
					profile.socialcustomer = _db.Customers.Where(g => g.CustomerId == profile.SocialProfile.CustomerId).SingleOrDefault();
                    
                }

                if (profile.SocialProfile_Instagram_TargetingInformation.ExecutionIntervals != null)
                   profile.SocialProfile_Instagram_TargetingInformation.ExecutionIntervals = Regex.Unescape(Regex.Replace(profile.SocialProfile_Instagram_TargetingInformation.ExecutionIntervals, @"\t|\n|\r", "")).Replace("\\",@"");

                return profile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //_db.SocialProfile_Payments.Where(g => g.SocialProfileId == profileId).OrderByDescending(g => g.PaymentDateTime).ToList();


        public SocialProfileDTO GetGlobalSocialProfilesById(int profileId)
        {

            //var config = new MapperConfiguration(cfg => cfg.CreateMap<SG2_usp_Customer_GetSocialProfileById_Result, CustomerTargetProfileDTO>());
            //var mapper = new Mapper(config);




            try
            {
                SocialProfileDTO profile = new SocialProfileDTO();
                using (var _db = new SocialGrowth2Connection())
                {
                    //global profile loading

                    _db.Configuration.LazyLoadingEnabled = false;
                    profile.SocialProfile_Instagram_TargetingInformation = _db.SocialProfile_Instagram_TargetingInformation.Where(g => g.IsSystem == true && g.SocialProfileId == null).Single();

                }

                if (profile.SocialProfile_Instagram_TargetingInformation.ExecutionIntervals != null)
                    profile.SocialProfile_Instagram_TargetingInformation.ExecutionIntervals = Regex.Unescape(Regex.Replace(profile.SocialProfile_Instagram_TargetingInformation.ExecutionIntervals, @"\t|\n|\r", "")).Replace("\\", @"");

                return profile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SocialProfileDTO GetAgencySocialTargettingByBrokerCustomerId(int BrokerCustomerId)
        {

            try
            {
                SocialProfileDTO profile = new SocialProfileDTO();
                using (var _db = new SocialGrowth2Connection())
                {
                    //Agency profile loading

                    _db.Configuration.LazyLoadingEnabled = false;
                    profile.SocialProfile_Instagram_TargetingInformation = _db.SocialProfile_Instagram_TargetingInformation.Where(g => g.BrokerCustomerId == BrokerCustomerId && g.IsBrokerDefault == true).SingleOrDefault();
                    if (profile.SocialProfile_Instagram_TargetingInformation == null)
                    {
                        var newBrokerDefaultProfile = _db.SocialProfile_Instagram_TargetingInformation.Where(g => g.IsSystem == true && g.SocialProfileId == null).Single();
                        newBrokerDefaultProfile.BrokerCustomerId = BrokerCustomerId;
                        newBrokerDefaultProfile.SocialProfileId = null;
                        newBrokerDefaultProfile.IsBrokerDefault = true;
                        newBrokerDefaultProfile.IsSystem = false;


                        _db.SocialProfile_Instagram_TargetingInformation.Add(newBrokerDefaultProfile);
                        _db.SaveChanges();

                        profile.SocialProfile_Instagram_TargetingInformation = newBrokerDefaultProfile;

                    }

                }

                if (profile.SocialProfile_Instagram_TargetingInformation.ExecutionIntervals != null)
                    profile.SocialProfile_Instagram_TargetingInformation.ExecutionIntervals = Regex.Unescape(Regex.Replace(profile.SocialProfile_Instagram_TargetingInformation.ExecutionIntervals, @"\t|\n|\r", "")).Replace("\\", @"");

                return profile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SocialProfile_PaymentsDTO> GetCustomerBrokerPaymentHistory(int CustomerId)
        {
            try
            {
                SocialProfileDTO profile = new SocialProfileDTO();
                using (var _db = new SocialGrowth2Connection())
                {
                    var res = (from m in _db.Customer_Payments
                               join r in _db.PaymentPlans on m.PaymentPlanId equals r.PaymentPlanId
                               join ps in _db.EnumerationValues on m.StatusId equals ps.EnumerationValueId
                               where m.CustomerId == CustomerId
                               select new SocialProfile_PaymentsDTO { Description = m.Description, EndDate = m.EndDate, PaymentPlanId = m.PaymentPlanId, Name = m.Name, PaymentDateTime = m.PaymentDateTime, PaymentId = m.PaymentId, PaymentPlanName = r.PlanName, Price = m.Price, StartDate = m.StartDate, Status = ps.Name, StatusId = m.StatusId, StripeInvoiceId = m.StripeInvoiceId, StripePlanId = m.StripePlanId, StripeSubscriptionId = m.StripeSubscriptionId, SubscriptionType = m.SubscriptionType }).ToList();
                    return res;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public (bool Succcess, string Message) AddIntagramSocialProfile(string InstagramSocialUsername, int CustomerId)
        {
            try
            {
                
                using (var _db = new SocialGrowth2Connection())
                {

                    var count =_db.SocialProfiles.Where(g => g.SocialUsername == InstagramSocialUsername && g.CustomerId == CustomerId && g.SocialProfileTypeId == 30).Count();
                    if (count == 0)
                    {
                        Random generator = new Random();
                        String r = generator.Next(0, 999999).ToString("D6");
                        var PaymentPlan = _db.PaymentPlans.Where(g => g.IsDefault == true && g.IsBrokerPlan == false).Single();

                        var Customer = _db.Customers.Where(g => g.CustomerId == CustomerId).Single();

                        SocialProfile_Instagram_TargetingInformation defaultTargetProfile = null;
                        //if broker get default profile from broker if available.
                        if (Customer.IsBroker.HasValue && Customer.IsBroker.Value == true)
                        {
                            defaultTargetProfile =  _db.SocialProfile_Instagram_TargetingInformation.Where(g => g.IsBrokerDefault == true && g.BrokerCustomerId == CustomerId).SingleOrDefault();
                            if (defaultTargetProfile == null)
                            {
                                defaultTargetProfile = _db.SocialProfile_Instagram_TargetingInformation.Where(g => g.IsSystem == true).Single();
                            }
                        }
                        else// get default profile from global.
                        {
                            defaultTargetProfile = _db.SocialProfile_Instagram_TargetingInformation.Where(g => g.IsSystem == true).Single();
                        }

                        var newProfile = new SocialProfile { CreatedBy = "User", SocialProfileTypeId = 30, SocialUsername = InstagramSocialUsername, CreatedOn = DateTime.Now, UpdatedBy="User", UpdatedOn = DateTime.Now, IsArchived = false, CustomerId = CustomerId, PaymentPlanId = PaymentPlan.PaymentPlanId, PinCode = r, StatusId = 25 };

                        newProfile = _db.SocialProfiles.Add(newProfile);
                        _db.SaveChanges();

                        defaultTargetProfile.SocialProfileId = newProfile.SocialProfileId;
                        defaultTargetProfile.IsSystem = false;
                        defaultTargetProfile.BrokerCustomerId = null;
                        defaultTargetProfile.IsBrokerDefault = false;


                        _db.SocialProfile_Instagram_TargetingInformation.Add(defaultTargetProfile);
                        _db.SaveChanges();


                        return (true, "Profile Created Successfully");
                    }
                    else
                    {
                        return (false, "Profile with Username already exists");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ActionBoardJVSData> GetTrelloStatuses()
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var actionJVStatus = _db.EnumerationValues.Where(g => g.EnumerationId == 3 && g.IsVisible == true).ToList();
                    if (actionJVStatus != null)
                    {
                        List<ActionBoardJVSData> actionBoardJVSDatas = new List<ActionBoardJVSData>();
                        foreach (var item in actionJVStatus)
                        {
                            ActionBoardJVSData actionBoardJVSData = new ActionBoardJVSData();
                            actionBoardJVSData.JVBStatusId = item.EnumerationValueId;
                            actionBoardJVSData.JVBStatusName = item.Name;
                            actionBoardJVSData.JVBStatusDesc = item.Description;
                            actionBoardJVSData.SequenceNo = item.SequenceNo;
                            actionBoardJVSDatas.Add(actionBoardJVSData);
                        }
                        return actionBoardJVSDatas;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //public async Task<bool> SetSocialProfileJVStatus(int profileId, int jvStatusId, string updatedBy)
        //{
        //    try
        //    {
        //        using (var _db = new SocialGrowth2Connection())
        //        {
        //            var socialProfile = _db.SocialProfile.FirstOrDefault(m => m.SocialProfileId == profileId);

        //            int? JVBoxStatus = socialProfile.JVBoxStatusId;
        //            int? JVBoxId = socialProfile.JVBoxId;

        //            if (socialProfile != null)
        //            {
        //                if (jvStatusId == 0)
        //                    socialProfile.JVBoxStatusId = null;
        //                else
        //                    socialProfile.JVBoxStatusId = jvStatusId;
        //                socialProfile.UpdatedBy = updatedBy;
        //                socialProfile.UpdatedOn = DateTime.Now;
        //                await _db.SaveChangesAsync();

        //            }

        //            if (JVBoxStatus != null)
        //            {
        //                var SPH = _db.Set<SocialProfile_StatusHistory>();
        //                SPH.Add(new SG2_SocialProfile_StatusHistory { JVBoxStatusId = JVBoxStatus, JVBoxId = JVBoxId, CreatedDate = DateTime.Now, SocialProfileId = profileId });

        //                await _db.SaveChangesAsync();
        //                return true;
        //            }
        //            return false;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public async Task<bool> UpdateSPNoOfAttempt(int profileId, short NoOfAttempt, DateTime JVAttemptsBlockedTill)
        //{
        //    try
        //    {
        //        using (var _db = new SocialGrowth2Connection())
        //        {
        //            var SP = _db.SG2_SocialProfile.FirstOrDefault(x => x.SocialProfileId == profileId);
        //            if (SP != null)
        //            {
        //                SP.JVAttempts = NoOfAttempt;
        //                SP.JVAttemptsBlockedTill = JVAttemptsBlockedTill;
        //                SP.UpdatedOn = DateTime.Now;
        //                await _db.SaveChangesAsync();
        //                return true;
        //            }
        //            return false;

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //public async Task<bool> SetTargetPreferenceQueueStatus(int profileId, short queueStatus, string updatedBy)
        //{
        //    try
        //    {
        //        using (var _db = new SocialGrowth2Connection())
        //        {
        //            var SP = _db.SG2_SocialProfile_TargetingInformation.FirstOrDefault(x => x.SocialProfileId == profileId);
        //            if (SP != null)
        //            {
        //                SP.UpdatedBy = updatedBy;
        //                SP.UpdatedOn = DateTime.Now;
        //                SP.QueueStatus = queueStatus;

        //                await _db.SaveChangesAsync();
        //                return true;
        //            }
        //            return false;

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //public async Task<bool> SetTargetPreferenceLikeyStatus(int profileId, short jvLikeyStatus, int JVNoOfLikes, string updatedBy)
        //{
        //    try
        //    {
        //        using (var _db = new SocialGrowth2Connection())
        //        {
        //            var SP = _db.SocialProfile_Instagram_TargetingInformation.FirstOrDefault(x => x.SocialProfileId == profileId);
        //            if (SP != null)
        //            {
        //                SP.JVLikeyStatus = jvLikeyStatus;
        //                SP.JVNoOfLikes = JVNoOfLikes;

        //                await _db.SaveChangesAsync();
        //                return true;
        //            }
        //            return false;

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //public bool UpdateJVStatus(int profileId, int? jvStatus = null)
        //{
        //    try
        //    {
        //        using (var _db = new SocialGrowth2Connection())
        //        {
        //            var SP = _db.SG2_SocialProfile.FirstOrDefault(x => x.SocialProfileId == profileId);
        //            if (SP != null)
        //            {
        //                SP.UpdatedOn = DateTime.Now;
        //                SP.JVBoxStatusId = jvStatus;

        //                _db.SaveChanges();
        //                return true;
        //            }
        //            return false;

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //public bool UpdateMPBox(int profileId, int? MPBox)
        //{
        //    try
        //    {
        //        using (var _db = new SocialGrowth2Connection())
        //        {
        //            var SP = _db.SG2_SocialProfile.FirstOrDefault(x => x.SocialProfileId == profileId);
        //            if (SP != null && SP.JVBoxId != MPBox)
        //            {
        //                SP.UpdatedOn = DateTime.Now;
        //                SP.JVBoxId = MPBox;

        //                _db.SaveChanges();
        //                return true;
        //            }
        //            return false;

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //public bool UpdateProxyIp(int profileId, int? proxyIp = null)
        //{
        //    try
        //    {
        //        using (var _db = new SocialGrowth2Connection())
        //        {
        //            var SP = _db.SG2_usp_ProxyMapping_Insert(profileId, proxyIp);

        //            return true;


        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //--TODO: Move to specific repository
        //public List<JVBoxDTO> GetMPBoxes()
        //{
        //    try
        //    {
        //        using (var _db = new SocialGrowth2Connection())
        //        {
        //            List<JVBoxDTO> mPBoxDTOs = new List<JVBoxDTO>();
        //            var MPBoxes = _db.SG2_usp_Get_AvailableMPBoxes();
        //            if (MPBoxes != null)
        //            {
        //                foreach (var item in MPBoxes)
        //                {
        //                    JVBoxDTO mPBoxDTO = new JVBoxDTO();
        //                    mPBoxDTO.BoxName = item.BoxName;
        //                    mPBoxDTO.JVBoxId = item.JVBoxId;
        //                    mPBoxDTOs.Add(mPBoxDTO);
        //                }
        //                return mPBoxDTOs;

        //            }
        //            return null;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        ////--TODO: Move to specific repository
        //public List<ProxyIPDTO> GetProxyIPs(int CountryId, int CityId)
        //{
        //    try
        //    {
        //        using (var _db = new SocialGrowth2Connection())
        //        {
        //            List<ProxyIPDTO> ProxyIPDTOs = new List<ProxyIPDTO>();
        //            var proxy = _db.SG2_usp_Get_AvailableProxies(CountryId, CityId);
        //            if (proxy != null)
        //            {
        //                foreach (var item in proxy)
        //                {
        //                    ProxyIPDTO PIPD = new ProxyIPDTO();
        //                    PIPD.ProxyId = item.ProxyId;
        //                    PIPD.ProxyNumber = item.ProxyIPNumber;
        //                    ProxyIPDTOs.Add(PIPD);
        //                }
        //                return ProxyIPDTOs;

        //            }
        //            return null;
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}

        //public bool UpdateNoOfAttempts(int profileId, int noOfAttempts)
        //{
        //    try
        //    {
        //        using (var _db = new SocialGrowth2Connection())
        //        {
        //            var cust = _db.SG2_SocialProfile.FirstOrDefault(x => x.SocialProfileId == profileId);
        //            if (cust != null)
        //            {
        //                cust.JVAttempts = (Int16) noOfAttempts;
        //                cust.UpdatedBy = "MP Bot";
        //                cust.UpdatedOn = DateTime.Now;
        //                _db.SaveChanges();
        //                return true;
        //            }
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //public bool BlockProfile24Hrs(int profileId, int noOfAttempts)
        //{
        //    try
        //    {
        //        using (var _db = new SocialGrowth2Connection())
        //        {
        //            var cust = _db.SG2_SocialProfile.FirstOrDefault(x => x.SocialProfileId == profileId);
        //            if (cust != null)
        //            {
        //                cust.JVAttempts = (Int16)noOfAttempts;
        //                cust.UpdatedBy = "MP Bot";
        //                cust.UpdatedOn = DateTime.Now;
        //                cust.JVAttemptsBlockedTill = DateTime.Now.AddDays(1);
        //                _db.SaveChanges();
        //                return true;
        //            }
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

    }
}
