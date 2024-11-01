﻿using SG2.CORE.DAL.Repositories;
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
using SG2.CORE.MODAL;
using SG2.CORE.MODAL.MobileViewModels;
using SG2.CORE.MODAL.ViewModals.Customers;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using HeyRed.Mime;
using System.Web.Hosting;
using SG2.CORE.MODAL.ViewModals.Statistics;
using AutoMapper;
using SG2.CORE.MODAL.ViewModals.TargetPreferences;
using Newtonsoft.Json;

namespace SG2.CORE.BAL.Managers
{
    public class CustomerManager
    {
        private readonly CustomerRepository _customerRepository;
        private readonly SocialProfileRepository _socialRepository;

        private readonly SessionManager _sessionManager;

        public CustomerManager()
        {
            _customerRepository = new CustomerRepository();
            _socialRepository = new SocialProfileRepository();
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

        public bool DeleteProfile(int customerId, int SocialProfileId)
        {
            try
            {
                return _customerRepository.DeleteProfile(customerId, SocialProfileId);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public (bool LoginSuccessful, string Message, int SocialProfileId, bool PasswordNeeded, int ErrorCode, int CustomerId, bool IsBroker, int BlockCode, DateTime BlockDateTimeUTC, bool InitialStatsReceived) PerformMobileLogin(MobileLoginRequest model)
        {
            return _customerRepository.PerformMobileLogin(model);
        }

        public (bool, int, string) LoginUser(string username, string password, ref string errorMessage)
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
                        return (false, 0, null);
                    }
                    else if (cust.StatusId == 6)
                    {
                        errorMessage = "Your account is inactive. Please contact adminstrator.";
                        return (false, 0, null);
                    }
                    else if (cust.StatusId == 4)
                    {
                        errorMessage = "Your account has been deleted.";
                        return (false, 0, null);
                    }
                    else if (cust.StatusId == 5 || cust.StatusId == 1)
                    {
                        _sessionManager.Set(SessionConstants.Customer, cust);
                        return (true, cust.CustomerId, cust.EmailAddress);
                    }

                }
                errorMessage = "Invalid email or password.";
                return (false, 0, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public IList<CustomerListingViewModel> GetUserData(string SearchCriteria, string PageSize, int PageNumber, int? StatusId, string ProductId, string JVStatus, int? Subscription, int? profileType, int? BlockId, int? AppConnStatus)
        {
            var Model = _customerRepository.GetUserData(SearchCriteria, PageNumber, PageSize, StatusId, ProductId, JVStatus, Subscription, profileType, BlockId, AppConnStatus);

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

        public bool SaveUpdateProfileDataIndividually(string value, string fieldName, int socialProfileId, int TargetingInformationId)
        {
            try
            {
                return _customerRepository.SaveUpdateProfileDataIndividually(value, fieldName, socialProfileId, TargetingInformationId);
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
            return _customerRepository.GetActionBoardData(JVBoxStatusId);
            //return new List<ActionBoardListingViewModel>();
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


        public bool UpdateSocialProfilePhonePackageDetails(DateTime PurchaseDate, string PhonePackagePurchaseSessionID, int SocialProfileId)
        {
            try
            {
                var socialprofile = _socialRepository.UpdateSocialProfilePhonePackageDetails(PurchaseDate, PhonePackagePurchaseSessionID, SocialProfileId);
                return socialprofile;

            }
            catch (Exception e)
            {

                throw e;

            }
        }

        public bool UpdateCustomerContactPhone(string Phone, int SocialProfileId)
        {
            try
            {
                return _customerRepository.UpdateCustomerPhone(SocialProfileId, Phone);

            }
            catch (Exception e)
            {

                throw e;

            }
        }

        public bool UpdateBasicSocialProfile(SocialProfileDTO model) {
            try
            {
                var socialprofile = _socialRepository.UpdateSocialProfile(model.SocialProfile.DeviceBinLocation, model.SocialProfile.SocialUsername, model.SocialProfile.SocialProfileId);
                if (socialprofile)
                {
                    _sessionManager.Set(SessionConstants.SocialProfile, socialprofile);
                }
                return socialprofile;

            }
            catch (Exception e)
            {

                throw e;

            }
        }

        public bool UpdateBasicSocialProfileSocialPassword(string SocialPassword, int SocialProfileId)
        {
            try
            {
                return _socialRepository.UpdateSocialProfileSocialPassword(SocialPassword, SocialProfileId);

            }
            catch (Exception e)
            {

                throw e;

            }
        }

        public bool ResetSocialProfileManifestChangeFlag(bool Flag, int SocialProfileId)
        {
            try
            {
                return _socialRepository.ResetSocialProfileManifestChangeFlag(Flag, SocialProfileId);

            }
            catch (Exception e)
            {

                throw e;

            }
        }


        public bool GetSocialProfileManifestChangeFlag(int SocialProfileId)
        {
            try
            {
                return _socialRepository.GetSocialProfileManifestChangeFlag(SocialProfileId);

            }
            catch (Exception e)
            {

                throw e;

            }
        }

        public bool UpdateBasicSocialProfileBlock(BlockStatus blockStatus, int SocialProfileId, DateTime currentDate)
        {
            try
            {
                var socialprofile = _socialRepository.UpdateSocialProfileBlocks(blockStatus, SocialProfileId, currentDate);
                return socialprofile;

            }
            catch (Exception e)
            {

                throw e;

            }
        }
        public bool UpdateTargetProfile(SocialProfileDTO model, bool SaveFullTargetProfile = true)
        {
            try
            {
                var socialprofile = _socialRepository.UpdateTargetProfile(model, SaveFullTargetProfile);
                if (socialprofile)
                {
                    _sessionManager.Set(SessionConstants.SocialProfile, socialprofile);
                }
                return socialprofile;

            }
            catch (Exception e)
            {

                throw e;

            }
        }

        public bool UpdateTargetProfileLists(SocialProfileDTO model)
        {
            try
            {
                var socialprofile = _socialRepository.UpdateTargetProfileLists(model);
                if (socialprofile)
                {
                    _sessionManager.Set(SessionConstants.SocialProfile, socialprofile);
                }
                return socialprofile;

            }
            catch (Exception e)
            {

                throw e;

            }
        }

        public bool UpdateTargetProfileWhiteList(SocialProfileDTO model)
        {
            try
            {
                var socialprofile = _socialRepository.UpdateTargetProfileWhiteList(model);
                if (socialprofile)
                {
                    _sessionManager.Set(SessionConstants.SocialProfile, socialprofile);
                }
                return socialprofile;

            }
            catch (Exception e)
            {

                throw e;

            }
        }

        public bool UpdateTargetProfileBlackList(SocialProfileDTO model)
        {
            try
            {
                var socialprofile = _socialRepository.UpdateTargetProfileBlackLists(model);
                if (socialprofile)
                {
                    _sessionManager.Set(SessionConstants.SocialProfile, socialprofile);
                }
                return socialprofile;

            }
            catch (Exception e)
            {

                throw e;

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

        public CustomerDTO GetContactDetails(int CustomerId)
        {
            try
            {
                var Customer = _customerRepository.GetContactDetails(CustomerId);

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

        public bool UpdateCustomerBrokerProfile(CustomerBrokerProfileRequest model)
        {
            try
            {

                if (!string.IsNullOrEmpty(model.BrokerLogoUpload))
                {
                    String[] spearator = { "base64," };
                    string ext = "";


                    var match = Regex.Match(model.BrokerLogoUpload, @"data:(?<type>.+?);base64,(?<data>.+)");
                    var base64Data = match.Groups["data"].Value;
                    var contentType = match.Groups["type"].Value;

                    byte[] bytes = Convert.FromBase64String(base64Data);
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        Image image = Image.FromStream(ms);
                        image.Save(HostingEnvironment.MapPath("~/AgencyLogos/" + model.cid.ToString() + "." + MimeTypesMap.GetExtension(contentType)));
                    }

                    model.BrokerLogo = model.cid.ToString() + "." + MimeTypesMap.GetExtension(contentType);
                }

                return _customerRepository.UpdateCustomerBrokerProfile(model);
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
                return _customerRepository.GetCustomerBrokerPaymentHistory(CustomerId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool UpdateCustomerStripeCustomer(int CustomerId, string StripCustomerId, string StripeSubscriptionId, int PaymentPlanId)
        {
            try
            {
                var res = _customerRepository.UpdateCustomerStripeCustomerId(CustomerId, StripCustomerId, StripeSubscriptionId, PaymentPlanId);

                _sessionManager.Set(SessionConstants.Customer, _customerRepository.GetCustomerRefresh(CustomerId));

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateSocialProfileStripeCustomer(int SocialProfileId, string StripCustomerId, string StripeSubscriptionId, int PaymentPlanId, PlanSubscription status = PlanSubscription.ActivePlan)
        {
            try
            {
                return _customerRepository.UpdateSocialProfileStripeCustomerId(SocialProfileId, StripCustomerId, StripeSubscriptionId, PaymentPlanId, status);
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
                return _customerRepository.GetCustomerDTOByCustomerId(CustomerId);

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
                return _customerRepository.GetCustomerByCustomerId(CustomerId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomerSocialProfileDTO> GetFollowList(int socialProfileId)
        {
            try
            {
                return _customerRepository.GetSocialProfilesWithoutExistingFollowers(socialProfileId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SocialProfile_FollowedAccounts> GetAllFollowedAccounts(int socialProfileId)
        {
            try
            {
                return _customerRepository.GetAllFollowedAccounts(socialProfileId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SocialProfile_Messages> GetAllSentMessages(int socialProfileId)
        {
            try
            {
                return _customerRepository.GetAllSentMessages(socialProfileId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SocialProfile_PaymentsDTO> InsertCustomerPayment(SocialProfile_PaymentsDTO sG2_Subscription)
        {
            try
            {
                var sb = await _customerRepository.InsertCustomerPaymentRecord(sG2_Subscription);
                return sb;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SocialProfile_PaymentsDTO> InsertSocialProfilePayment(SocialProfile_PaymentsDTO sG2_Subscription)
        {
            try
            {
                var sb = await _customerRepository.InsertSocialProfilePaymentRecord(sG2_Subscription);
                return sb;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSubscription(SocialProfile_PaymentsDTO sG2_Subscription)
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

        public SocialProfile_PaymentsDTO GetSubscription(int SocialProfileId)
        {
            try
            {
                SocialProfile_PaymentsDTO subscriptionDTO = _customerRepository.GetSubscription(SocialProfileId);

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


        public List<CustomerSocialProfileDTO> GetSocialProfilesByCustomerid(int customerId, ProfilesSearchRequest model)
        {
            try
            {
                return _customerRepository.GetSocialProfilesByCustomerid(customerId, model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public SocialProfileDTO GetSocialProfileById(int profileId)
        {
            try
            {
                if (profileId == -999)
                {
                    return _customerRepository.GetGlobalSocialProfilesById(profileId);
                }
                else
                {
                    return _customerRepository.GetSocialProfilesById(profileId);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public SocialProfile GetSocialProfileByStripeSubscriptionId(string StripeSubscriptionId)
        {
            try
            {

                return _customerRepository.GetSocialProfileByStripeSubscriptionId(StripeSubscriptionId);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public SocialProfileDTO GetSocialProfileTargetByBrokerCustomerId(int BrokerCustomerId)
        {
            try
            {
                return _customerRepository.GetAgencySocialTargettingByBrokerCustomerId(BrokerCustomerId);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public (bool Succcess, string Message) AddInstagramSocialProfile(string InstagramSocialUsername, int CustomerId)
        {
            try
            {
                return _customerRepository.AddIntagramSocialProfile(InstagramSocialUsername, CustomerId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        // Follow  with username    actionid = 60
        // UnFollow  with username  actionid = 61
        // Like with username       actionid = 62
        // Comment with username    actionid = 63
        // StoryView with username  actionid = 64
        // FollowerCount with username  and count in msg actionid = 65
        public bool SaveMobileAppAction(MobileActionRequest model)
        {
            try
            {
                var result = _customerRepository.SaveMobileAppActions(model);

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public List<SocialProfile_ActionsViewModel> ReturnLastActions(int socialProfileId, int NoOfActions)
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<SocialProfile_Actions, SocialProfile_ActionsViewModel>()
                );

                var mapper = new Mapper(config);

                double appoffset = 0;
                var serveroffset = DateTimeOffset.Now.Offset.Hours;//TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);


                var results = _customerRepository.ReturnLastActions(socialProfileId, NoOfActions, out appoffset);

                // +3 +5 
                double offset = 0;
                offset = serveroffset - appoffset;

                var mapped = mapper.Map<List<SocialProfile_ActionsViewModel>>(results);

                foreach (var item in mapped)
                {
                    //if (offset != 0)
                    //{
                    //    item.ActionDateTime = item.ActionDateTime.Value.AddHours(offset);
                    //}
                    switch (item.ActionID)
                    {
                        case 60: item.Action = "Following"; break;
                        case 61: item.Action = "UnFollow"; break;
                        case 62: item.Action = "Like"; break;
                        case 63: item.Action = "Comment"; break;
                        case 64: item.Action = "StoryView"; break;
                        case 65: item.Action = "Followers count update"; break;
                        case 66: item.Action = "Action Block"; break;
                        case 67: item.Action = "Block 2"; break;
                        case 68: item.Action = "Block 3"; break;
                        case 69: item.Action = "Clear Block"; break;
                        case 70: item.Action = "Block 4"; break;
                        case 71: item.Action = "Block 5"; break;
                        case 72: item.Action = "Initial Stats"; break;
                        case 73: item.Action = "Follow Exchange"; break;
                        case 74: item.Action = "Like Exchange"; break;
                        case 86: item.Action = "Bad Hash Tag for Removal"; break;

                        case 90: item.Action = "BoradCast Message Sent"; break;
                        case 91: item.Action = "Sequenced Message Sent"; break;
                        case 92: item.Action = "Message Seen by user"; break;
                        case 93: item.Action = "Reset Broadcast History"; break;
                        case 94: item.Action = "Reset Sequence History"; break;
                        default:
                            break;
                    }
                }
                return mapped;
            }
            catch (Exception e)
            {

                throw e;
            }
        }



        public SocialProfile_PaymentsDTO GetLastCancelledSubscription(int profileId, DateTime dateTime)
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

        public List<ActionBoardJVSData> GetTrelloStatuses()
        {
            var Model = _customerRepository.GetTrelloStatuses();

            return Model;
        }

        public double GetAppTimeZoneOffSet(int socialProfileId)
        {
            double number = 0;
            var vm = _customerRepository.GetAppTimeZoneOffSet(socialProfileId);
            if (vm != null)
            {

                if (!string.IsNullOrEmpty(vm.AppTimeZoneOffSet))
                {
                    Double.TryParse(vm.AppTimeZoneOffSet, out number);
                }
            }
            return number;

        }



        //public async Task<bool> SetTargetPreferenceQueueStatus(int profileId, short queueStatus, string updatedBy)
        //{
        //    try
        //    {
        //        return await _customerRepository.SetTargetPreferenceQueueStatus(profileId, queueStatus, updatedBy);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<bool> SetTargetPreferenceLikeyStatus(int profileId, short jvLikeyStatus, int JVNoOfLikes, string updatedBy)
        //{
        //    try
        //    {
        //        return await _customerRepository.SetTargetPreferenceLikeyStatus(profileId, jvLikeyStatus, JVNoOfLikes, updatedBy);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public bool AddRemoveFollowAccounts(List<MobileActionRequest> list)
        {
            return _socialRepository.AddRemoveFollowAccounts(list);
        }


        public bool AddUpdateSentMesseges(List<MobileActionRequest> list)
        {
            return _socialRepository.AddUpdateResetMeseages(list);
        }


        public bool RemoveBadTags(List<MobileActionRequest> list)
        {
            return _socialRepository.RemoveBagTags(list);
        }

        public List<vwRunNotificationsData> GetRunNotificationsData() 
        {
            List<vwRunNotificationsData> result = new List<vwRunNotificationsData>();
            var data = _customerRepository.GetRunNotificationsData();
            foreach (var item in data)
            {
                var executioninterval = JsonConvert.DeserializeObject<List<ExecutionInterval>>(item.ExecutionIntervals)[0];
                if (executioninterval != null)
                    if (!string.IsNullOrEmpty(executioninterval.starttime))
                    {
                        {
                            /*try
                            {*/
                            var startTime = Convert.ToDateTime(executioninterval.starttime);

                            Double number = 0;
                            if (!string.IsNullOrEmpty(item.AppTimeZoneOffSet))
                                Double.TryParse(item.AppTimeZoneOffSet, out number);
                            else
                                number = 0;

                            var timenow = DateTime.UtcNow.AddHours(number);
                            var minsleft = (timenow - startTime).TotalMinutes;
                            if (minsleft >= 115 && minsleft < 125)
                            {
                                item.startTime = executioninterval.starttime;
                                result.Add(item);
                            }
                            /*}
                            catch (Exception e)
                            {
                                //do nothing
                                var a = 1;
                            }*/
                        }
                }
            }

            return result;

        }


        public bool UpdateSocialProfileWarmupInformation(UpdateProfileWarmupRequest model)
        {
            try
            {
                var socialprofile = _socialRepository.UpdateSocialProfileWarmupInformation(model);
                return socialprofile;

            }
            catch (Exception e)
            {

                throw e;

            }
        }
    }
}
