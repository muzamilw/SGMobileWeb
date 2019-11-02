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
                    var item = _db.SG2_usp_Customer_ProfileUpdate(entity.CustomerId, entity.UserName, entity.FirstName, entity.SurName, entity.PhoneNumber, entity.PhoneCode);
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
                    var result = _db.Customers.SqlQuery("SELECT * from SG2_Customer where CustomerId=@CustomerId "
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
                    else if ((fieldName == "firstName") || (fieldName == "lastName") || (fieldName == "Source") || (fieldName == "Comment") || (fieldName == "Title") || (fieldName == "ResTeamMember") || (fieldName == "AvaToEveryOne") || (fieldName == "CustomerStatus"))
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
                            else if (fieldName == "lastName")
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
        //public List<ActionBoardListingViewModel> GetActionBoardData(int? JVBoxStatusId)
        //{
        //    try
        //    {
        //        using (var _db = new SocialGrowth2Connection())
        //        {
        //            var actiondata = _db.SG2_usp_GetProfilebyJVStatusId(JVBoxStatusId).ToList();
        //            if (actiondata != null)
        //            {
        //                List<ActionBoardListingViewModel> actionBoardViewModelList = new List<ActionBoardListingViewModel>();

        //                foreach (var item in actiondata)
        //                {
        //                    ActionBoardListingViewModel actionBoardViewModel = new ActionBoardListingViewModel();
        //                    actionBoardViewModel.InstaUsrName = item.InstaUsrName ?? "--";
        //                    actionBoardViewModel.Status = item.JVStatusName;
        //                    actionBoardViewModel.UserName = item.FullName ?? "--";
        //                    actionBoardViewModel.CustomerId = HttpUtility.UrlEncode(CryptoEngine.Encrypt(item.CustomerId.ToString()));
        //                    actionBoardViewModel.Email = item.Email;
        //                    actionBoardViewModel.ProfileId = HttpUtility.UrlEncode(CryptoEngine.Encrypt(item.SPId.ToString()));
        //                    actionBoardViewModel.EnumerationValueId = (short)item.JVStatusId;
        //                    actionBoardViewModel.Comment = item.Comment ?? "--";
        //                    actionBoardViewModel.IsArchived = item.IsArchived;
        //                    actionBoardViewModelList.Add(actionBoardViewModel);
        //                }
        //                return actionBoardViewModelList;
        //            }
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //-- TODO: Change Method and move to exact repository
        public CustomerTargetPreferencesViewModel GetSpecificUserTargettingInformation(int CustomerId, int SocialPId)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<SG2_usp_Get_Customer_Instagram_TargetingInformation_Result, CustomerTargetPreferencesViewModel>());
            var mapper = new Mapper(config);
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var actiondata = _db.SG2_usp_Get_Customer_Instagram_TargetingInformation (SocialPId).FirstOrDefault();
                    if (actiondata != null)
                    {
                        CustomerTargetPreferencesViewModel customerTargetPreferencesViewModel = mapper.Map<CustomerTargetPreferencesViewModel>(actiondata);

                        //CustomerTargetPreferencesViewModel customerTargetPreferencesViewModel = new CustomerTargetPreferencesViewModel();
                        //customerTargetPreferencesViewModel.Preference1 = actiondata.Preference1;
                        //customerTargetPreferencesViewModel.Preference2 = actiondata.Preference2;
                        //customerTargetPreferencesViewModel.Preference3 = actiondata.Preference3;
                        //customerTargetPreferencesViewModel.Preference4 = actiondata.Preference4;
                        //customerTargetPreferencesViewModel.Preference5 = actiondata.Preference5;
                        //customerTargetPreferencesViewModel.Preference6 = actiondata.Preference6;
                        //customerTargetPreferencesViewModel.Preference7 = actiondata.Preference7;
                        //customerTargetPreferencesViewModel.Preference8 = actiondata.Preference8;
                        //customerTargetPreferencesViewModel.Preference9 = actiondata.Preference9;
                        //customerTargetPreferencesViewModel.Preference10 = actiondata.Preference10;
                        //customerTargetPreferencesViewModel.Country = actiondata.SocialPrefferedCountry;
                        //customerTargetPreferencesViewModel.InstaUser = actiondata.SocialUsername;
                        //customerTargetPreferencesViewModel.Id = Convert.ToString(actiondata.CustomerId);
                        //customerTargetPreferencesViewModel.Status = actiondata.JVBoxStatus;
                        //customerTargetPreferencesViewModel.UserName = actiondata.SocialUsername;
                        //customerTargetPreferencesViewModel.InstaPassword = actiondata.SocialPassword;
                        //customerTargetPreferencesViewModel.City = actiondata.City;
                        //customerTargetPreferencesViewModel.SPId = actiondata.SocialProfileId.ToString();
                        //customerTargetPreferencesViewModel.CustomerUserName = actiondata.UserName;
                        //customerTargetPreferencesViewModel.Email = actiondata.Email;
                        //customerTargetPreferencesViewModel.SocialProfileName = actiondata.SocialProfileName;
                        //customerTargetPreferencesViewModel.SPStatus = actiondata.SPStatus;
                        //customerTargetPreferencesViewModel.JVStatus = actiondata.JVBoxStatusName;
                        //customerTargetPreferencesViewModel.NoOfProfile = actiondata.NoOfProfile;
                        //customerTargetPreferencesViewModel.JVName = actiondata.BoxName;
                        //customerTargetPreferencesViewModel.IP = actiondata.ProxyIPNumber;
                        //customerTargetPreferencesViewModel.SocialAccAS = actiondata.SocialAccAs;
                        //customerTargetPreferencesViewModel.VerificationCode = actiondata.VerificationCode;
                        //customerTargetPreferencesViewModel.MPBox = actiondata.JVBoxId;
                        //customerTargetPreferencesViewModel.Notes = actiondata.Comment;
                        //customerTargetPreferencesViewModel.ProxyId = actiondata.ProxyId;
                        //customerTargetPreferencesViewModel.ProxyPort = actiondata.ProxyPort;
                        //customerTargetPreferencesViewModel.IsArchived = actiondata.IsArchived;
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

        public IList<CustomerListingViewModel> GetUserData(string SearchCriteria, int PageNumber, string PageSize, int? Status, string ProductId, string JVStatus, int? Subscription)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    //-- TODO: Change proper sp
                    var usrdata = _db.SG2_usp_GetUserDetailsForbackOffice(SearchCriteria, PageNumber, PageSize, Status, (ProductId), JVStatus, Subscription).ToList();
                    if (usrdata != null)
                    {
                        List<CustomerListingViewModel> customerListingViewModelsList = new List<CustomerListingViewModel>();
                        foreach (var item in usrdata)
                        {
                            CustomerListingViewModel customerListingViewModel = new CustomerListingViewModel();
                            customerListingViewModel.Proxy = item.ProxyIPNumber;
                            customerListingViewModel.Products = item.Products;
                            customerListingViewModel.Status = item.Status;
                            customerListingViewModel.JVBoxStatus = item.JVBoxStatus;
                            customerListingViewModel.Name = item.UserName;
                            customerListingViewModel.InstaUsrName = item.InstaName;
                            customerListingViewModel.ID = Convert.ToString(item.CustomerId);
                            customerListingViewModel.Box = item.BoxName;
                            customerListingViewModel.TotalRecord = item.TotalRecord;
                            customerListingViewModel.SocialProfileId = Convert.ToString(item.SocialProfileId);
                            customerListingViewModel.SocialProfileName = item.SocialProfileName;
                            customerListingViewModel.CustomerEmail = item.CustomerEmail;
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

        public (bool, string, int) PerformMobileLogin(string SocialUserName, string Pin, string DeviceIMEI, bool ForceSwitchDevice)
        {

            using (var _db = new SocialGrowth2Connection())
            {
                var profile = _db.SocialProfiles.Where(g => g.SocialUsername == SocialUserName && g.PinCode == Pin && g.PinCode == Pin).SingleOrDefault();
                if (profile != null)
                {
                    if (string.IsNullOrEmpty(profile.DeviceIMEI))
                    {
                        profile.DeviceIMEI = DeviceIMEI;
                        profile.DeviceStatus = (int)DeviceStatus.Connected;
                        profile.LastConnectedDateTime = DateTime.Now;
                        _db.SaveChanges();
                        return (true, "Login Sucessful", profile.SocialProfileId);
                    }
                    else if (profile.DeviceIMEI == DeviceIMEI)
                    {
                        profile.DeviceIMEI = DeviceIMEI;
                        profile.DeviceStatus = (int)DeviceStatus.Connected;
                        profile.LastConnectedDateTime = DateTime.Now;
                        _db.SaveChanges();
                        return (true, "Login Sucessful", profile.SocialProfileId);
                    }
                    else if (profile.DeviceIMEI != DeviceIMEI && ForceSwitchDevice == false)
                    {
                        return (false, "Device IMEI does not match", profile.SocialProfileId);
                    }
                    else if (profile.DeviceIMEI != DeviceIMEI && ForceSwitchDevice == true)
                    {
                        profile.DeviceIMEI = DeviceIMEI;
                        profile.DeviceStatus = (int)DeviceStatus.Connected;
                        profile.LastConnectedDateTime = DateTime.Now;
                        _db.SaveChanges();
                        return (true, "Login Sucessful", profile.SocialProfileId);
                    }
                    else
                    {
                        return (false, "Device IMEI does not match", profile.SocialProfileId);
                    }


                }
                else
                {
                    return (false, "Invalid username or Pin", 0);
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
                            StripeSubscriptionId = usr.PaymentId.Value.ToString(),
                            DefaultSocialProfileId = usr.DefaultSocialProfileId.Value
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

        public async Task<bool> UpdateStripeCustomerId(int CustomerId, string StripCustomerId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var cus = _db.Customers.FirstOrDefault(x => x.CustomerId == CustomerId);
                    if (cus != null)
                    {
                        cus.StripeCustomerId = StripCustomerId;
                        await _db.SaveChangesAsync();
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

        public CustomerDTO GetCustomerByCustomerId(int CustomerId)
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
                            StripeSubscriptionId = usr.StripeSubscriptionId
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

        public async Task<SubscriptionDTO> InsertSubscription(SubscriptionDTO subscriptionDTO)
        {
            using (var _db = new SocialGrowth2Connection())
            {
                try
                {
                    var result = await Task.Run(() => _db.SG2_usp_SocialProfile_Payments_Save(subscriptionDTO.SocialProfileId, subscriptionDTO.StripeSubscriptionId, subscriptionDTO.Description, subscriptionDTO.Description,
                    subscriptionDTO.Price, subscriptionDTO.StripePlanId, subscriptionDTO.SubscriptionType, subscriptionDTO.StartDate, subscriptionDTO.EndDate, subscriptionDTO.StatusId
                    , subscriptionDTO.PaymentPlanId, subscriptionDTO.StripeInvoiceId).ToList());
                    if (result != null)
                    {
                        subscriptionDTO.SubscriptionId = result.FirstOrDefault().SubscriptionId;
                        return subscriptionDTO;
                    }
                    return null;
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
        }

        //TODO: Not usable
        public void UpdateSubscription(SubscriptionDTO subscriptionDTO)
        {
            using (var _db = new SocialGrowth2Connection())
            {

                SocialProfile_Payments subscription = new SocialProfile_Payments();

                var query =
               from sub in _db.SocialProfile_Payments
               where sub.PaymentId == subscriptionDTO.SubscriptionId
               select sub;

                subscription = query.FirstOrDefault();
                // Execute the query, and change the column values
                // you want to change.
                subscription.PaymentId = subscriptionDTO.SubscriptionId;
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

        public SubscriptionDTO GetSubscription(int SocialProfileId)
        {
            try
            {
                SocialProfile_Payments sub = new SocialProfile_Payments();
                using (var _db = new SocialGrowth2Connection())
                {

                    var query =
                   from subscription in _db.SocialProfile_Payments
                   where subscription.SocialProfileId == SocialProfileId
                   && subscription.StatusId == 25 // Active Subscription 
                   select subscription;
                    sub = query.FirstOrDefault();
                }
                SubscriptionDTO subscriptionDTO = new SubscriptionDTO();
                if (sub != null)
                {
                    subscriptionDTO.CustomerId  //TODO Ass Social Profileid
                        = sub.SocialProfileId;
                    subscriptionDTO.SubscriptionId = sub.PaymentId;
                    subscriptionDTO.StripeSubscriptionId = sub.StripeSubscriptionId;
                    subscriptionDTO.Description = sub.Description;
                    subscriptionDTO.Name = sub.Description;
                    subscriptionDTO.Price = (long)(sub.Price.Value);
                    subscriptionDTO.StripePlanId = sub.StripePlanId;
                    subscriptionDTO.SubscriptionType = sub.SubscriptionType;
                    subscriptionDTO.StartDate = sub.StartDate;
                    subscriptionDTO.EndDate = sub.EndDate;
                }
                return subscriptionDTO;

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

        public List<CustomerSocialProfileDTO> GetSocialProfilesByCustomerid(int customerId)
        {
            try
            {
                List<CustomerSocialProfileDTO> profiles = null;
                using (var _db = new SocialGrowth2Connection())
                {
                    var profs = _db.SG2_usp_Customer_GetSocialProfilesByCustomerId(customerId).ToList();
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
                            SubscriptionName = p.SubscriptionName
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

        public SubscriptionDTO GetLastCancelledSubscription(int profileId,  DateTime dateTime)
        {
            SocialProfile_Payments sub = new SocialProfile_Payments();
          
            using (var _db = new SocialGrowth2Connection())
            {
                
                var query =
               from subscription in _db.SocialProfile_Payments
               where subscription.SocialProfileId == profileId
                     && subscription.StatusId == (int)GlobalEnums.PlanSubscription.canceled
               select subscription;

                sub = query.FirstOrDefault();

                SubscriptionDTO subscriptionDTO = new SubscriptionDTO();
                if (sub != null)
                {
                    subscriptionDTO.CustomerId  //TODO Ass Social Profileid
                        = sub.SocialProfileId;
                    subscriptionDTO.SubscriptionId = sub.PaymentId;
                    subscriptionDTO.StripeSubscriptionId = sub.StripeSubscriptionId;
                    subscriptionDTO.Description = sub.Description;
                    subscriptionDTO.Name = sub.Description;
                    subscriptionDTO.Price = (long)(sub.Price.Value);
                    subscriptionDTO.StripePlanId = sub.StripePlanId;
                    subscriptionDTO.SubscriptionType = sub.SubscriptionType;
                    subscriptionDTO.StartDate = sub.StartDate;
                    subscriptionDTO.EndDate = sub.EndDate;
                    subscriptionDTO.StripeInvoiceId = sub.StripeInvoiceId;
                }
                return subscriptionDTO;
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

                    //var prof = _db.SG2_usp_Customer_GetSocialProfileById(profileId).FirstOrDefault();
                    //targetProfile = mapper.Map<SocialProfileDTO>(prof);
                    profile.SocialProfile = _db.SocialProfiles.Where(g => g.SocialProfileId == profileId).SingleOrDefault();
                    profile.SocialProfile_Instagram_TargetingInformation = _db.SocialProfile_Instagram_TargetingInformation.Where(g => g.SocialProfileId == profileId).SingleOrDefault();
                    profile.CurrentPaymentPlan = _db.PaymentPlans.Where(g => g.PaymentPlanId == profile.SocialProfile.PaymentPlanId).SingleOrDefault();
                    profile.LastSocialProfile_Payments = _db.SocialProfile_Payments.Where(g => g.SocialProfileId == profileId).OrderByDescending(g => g.PaymentDateTime).FirstOrDefault();
                }
                return profile;
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
