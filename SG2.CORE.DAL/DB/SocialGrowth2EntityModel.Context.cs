﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SG2.CORE.DAL.DB
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using SG2.CORE.MODAL;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SocialGrowth2Connection : DbContext
    {
        public SocialGrowth2Connection()
            : base("name=SocialGrowth2Connection")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Customer_ContactDetail> Customer_ContactDetail { get; set; }
        public virtual DbSet<Customer_Title> Customer_Title { get; set; }
        public virtual DbSet<Enumeration> Enumerations { get; set; }
        public virtual DbSet<EnumerationValue> EnumerationValues { get; set; }
        public virtual DbSet<LikeyAccount> LikeyAccounts { get; set; }
        public virtual DbSet<SocialProfile> SocialProfiles { get; set; }
        public virtual DbSet<SocialProfile_Actions> SocialProfile_Actions { get; set; }
        public virtual DbSet<SocialProfile_FollowedAccounts> SocialProfile_FollowedAccounts { get; set; }
        public virtual DbSet<SocialProfile_Instagram_TargetingInformation> SocialProfile_Instagram_TargetingInformation { get; set; }
        public virtual DbSet<SocialProfile_Notification> SocialProfile_Notification { get; set; }
        public virtual DbSet<SocialProfile_Payments> SocialProfile_Payments { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<SystemCity> SystemCities { get; set; }
        public virtual DbSet<SystemConfig> SystemConfigs { get; set; }
        public virtual DbSet<SystemCountry> SystemCountries { get; set; }
        public virtual DbSet<SystemRole> SystemRoles { get; set; }
        public virtual DbSet<SystemState> SystemStates { get; set; }
        public virtual DbSet<SystemUser> SystemUsers { get; set; }
        public virtual DbSet<SocialProfile_Statistics> SocialProfile_Statistics { get; set; }
        public virtual DbSet<PaymentPlan> PaymentPlans { get; set; }
    
        public virtual ObjectResult<SG2_usp_SystemConfig_GetAll_Result> SG2_usp_SystemConfig_GetAll(string rsSearchCrite, Nullable<int> riPageNumber, string riPageSize, Nullable<int> riStatusId)
        {
            var rsSearchCriteParameter = rsSearchCrite != null ?
                new ObjectParameter("rsSearchCrite", rsSearchCrite) :
                new ObjectParameter("rsSearchCrite", typeof(string));
    
            var riPageNumberParameter = riPageNumber.HasValue ?
                new ObjectParameter("riPageNumber", riPageNumber) :
                new ObjectParameter("riPageNumber", typeof(int));
    
            var riPageSizeParameter = riPageSize != null ?
                new ObjectParameter("riPageSize", riPageSize) :
                new ObjectParameter("riPageSize", typeof(string));
    
            var riStatusIdParameter = riStatusId.HasValue ?
                new ObjectParameter("riStatusId", riStatusId) :
                new ObjectParameter("riStatusId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_SystemConfig_GetAll_Result>("SG2_usp_SystemConfig_GetAll", rsSearchCriteParameter, riPageNumberParameter, riPageSizeParameter, riStatusIdParameter);
        }
    
        public virtual ObjectResult<SG2_usp_SystemUser_GetAll_Result> SG2_usp_SystemUser_GetAll(string rsSearchCrite, Nullable<int> riPageNumber, string riPageSize, Nullable<int> riStatusId)
        {
            var rsSearchCriteParameter = rsSearchCrite != null ?
                new ObjectParameter("rsSearchCrite", rsSearchCrite) :
                new ObjectParameter("rsSearchCrite", typeof(string));
    
            var riPageNumberParameter = riPageNumber.HasValue ?
                new ObjectParameter("riPageNumber", riPageNumber) :
                new ObjectParameter("riPageNumber", typeof(int));
    
            var riPageSizeParameter = riPageSize != null ?
                new ObjectParameter("riPageSize", riPageSize) :
                new ObjectParameter("riPageSize", typeof(string));
    
            var riStatusIdParameter = riStatusId.HasValue ?
                new ObjectParameter("riStatusId", riStatusId) :
                new ObjectParameter("riStatusId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_SystemUser_GetAll_Result>("SG2_usp_SystemUser_GetAll", rsSearchCriteParameter, riPageNumberParameter, riPageSizeParameter, riStatusIdParameter);
        }
    
        public virtual ObjectResult<SG2_usp_Customer_ProfileUpdate_Result> SG2_usp_Customer_ProfileUpdate(Nullable<int> iCustomerId, string rvcUserName, string rvcFirstName, string rvcSurName, string rvcPhoneNumber, string rvcPhoneCode)
        {
            var iCustomerIdParameter = iCustomerId.HasValue ?
                new ObjectParameter("iCustomerId", iCustomerId) :
                new ObjectParameter("iCustomerId", typeof(int));
    
            var rvcUserNameParameter = rvcUserName != null ?
                new ObjectParameter("rvcUserName", rvcUserName) :
                new ObjectParameter("rvcUserName", typeof(string));
    
            var rvcFirstNameParameter = rvcFirstName != null ?
                new ObjectParameter("rvcFirstName", rvcFirstName) :
                new ObjectParameter("rvcFirstName", typeof(string));
    
            var rvcSurNameParameter = rvcSurName != null ?
                new ObjectParameter("rvcSurName", rvcSurName) :
                new ObjectParameter("rvcSurName", typeof(string));
    
            var rvcPhoneNumberParameter = rvcPhoneNumber != null ?
                new ObjectParameter("rvcPhoneNumber", rvcPhoneNumber) :
                new ObjectParameter("rvcPhoneNumber", typeof(string));
    
            var rvcPhoneCodeParameter = rvcPhoneCode != null ?
                new ObjectParameter("rvcPhoneCode", rvcPhoneCode) :
                new ObjectParameter("rvcPhoneCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_Customer_ProfileUpdate_Result>("SG2_usp_Customer_ProfileUpdate", iCustomerIdParameter, rvcUserNameParameter, rvcFirstNameParameter, rvcSurNameParameter, rvcPhoneNumberParameter, rvcPhoneCodeParameter);
        }
    
        public virtual int SG2_Delete_Customer_All(Nullable<int> riCustomerId, Nullable<int> riSocialProfileId)
        {
            var riCustomerIdParameter = riCustomerId.HasValue ?
                new ObjectParameter("riCustomerId", riCustomerId) :
                new ObjectParameter("riCustomerId", typeof(int));
    
            var riSocialProfileIdParameter = riSocialProfileId.HasValue ?
                new ObjectParameter("riSocialProfileId", riSocialProfileId) :
                new ObjectParameter("riSocialProfileId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SG2_Delete_Customer_All", riCustomerIdParameter, riSocialProfileIdParameter);
        }
    
        public virtual ObjectResult<SG2_usp_Get_Customer_Instagram_TargetingInformation_Result> SG2_usp_Get_Customer_Instagram_TargetingInformation(Nullable<int> riSocialProfileId)
        {
            var riSocialProfileIdParameter = riSocialProfileId.HasValue ?
                new ObjectParameter("riSocialProfileId", riSocialProfileId) :
                new ObjectParameter("riSocialProfileId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_Get_Customer_Instagram_TargetingInformation_Result>("SG2_usp_Get_Customer_Instagram_TargetingInformation", riSocialProfileIdParameter);
        }
    
        public virtual ObjectResult<SG2_usp_Get_SpecificCustomerDetail_Result> SG2_usp_Get_SpecificCustomerDetail(Nullable<int> riCustomerId, Nullable<int> riProfileId)
        {
            var riCustomerIdParameter = riCustomerId.HasValue ?
                new ObjectParameter("riCustomerId", riCustomerId) :
                new ObjectParameter("riCustomerId", typeof(int));
    
            var riProfileIdParameter = riProfileId.HasValue ?
                new ObjectParameter("riProfileId", riProfileId) :
                new ObjectParameter("riProfileId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_Get_SpecificCustomerDetail_Result>("SG2_usp_Get_SpecificCustomerDetail", riCustomerIdParameter, riProfileIdParameter);
        }
    
        public virtual ObjectResult<SG2_usp_Get_CustomerOrderHistory_Result> SG2_usp_Get_CustomerOrderHistory(Nullable<int> riCustomerId, Nullable<int> riSocialProfileId, Nullable<int> riPageNumber, string riPageSize)
        {
            var riCustomerIdParameter = riCustomerId.HasValue ?
                new ObjectParameter("riCustomerId", riCustomerId) :
                new ObjectParameter("riCustomerId", typeof(int));
    
            var riSocialProfileIdParameter = riSocialProfileId.HasValue ?
                new ObjectParameter("riSocialProfileId", riSocialProfileId) :
                new ObjectParameter("riSocialProfileId", typeof(int));
    
            var riPageNumberParameter = riPageNumber.HasValue ?
                new ObjectParameter("riPageNumber", riPageNumber) :
                new ObjectParameter("riPageNumber", typeof(int));
    
            var riPageSizeParameter = riPageSize != null ?
                new ObjectParameter("riPageSize", riPageSize) :
                new ObjectParameter("riPageSize", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_Get_CustomerOrderHistory_Result>("SG2_usp_Get_CustomerOrderHistory", riCustomerIdParameter, riSocialProfileIdParameter, riPageNumberParameter, riPageSizeParameter);
        }
    
        public virtual ObjectResult<SG2_usp_GetUserDetailsForbackOffice_Result> SG2_usp_GetUserDetailsForbackOffice(string rsSearchCrite, Nullable<int> riPageNumber, string riPageSize, Nullable<int> riStatusId, string riProductId, string riJVStatus, Nullable<int> riSubscription)
        {
            var rsSearchCriteParameter = rsSearchCrite != null ?
                new ObjectParameter("rsSearchCrite", rsSearchCrite) :
                new ObjectParameter("rsSearchCrite", typeof(string));
    
            var riPageNumberParameter = riPageNumber.HasValue ?
                new ObjectParameter("riPageNumber", riPageNumber) :
                new ObjectParameter("riPageNumber", typeof(int));
    
            var riPageSizeParameter = riPageSize != null ?
                new ObjectParameter("riPageSize", riPageSize) :
                new ObjectParameter("riPageSize", typeof(string));
    
            var riStatusIdParameter = riStatusId.HasValue ?
                new ObjectParameter("riStatusId", riStatusId) :
                new ObjectParameter("riStatusId", typeof(int));
    
            var riProductIdParameter = riProductId != null ?
                new ObjectParameter("riProductId", riProductId) :
                new ObjectParameter("riProductId", typeof(string));
    
            var riJVStatusParameter = riJVStatus != null ?
                new ObjectParameter("riJVStatus", riJVStatus) :
                new ObjectParameter("riJVStatus", typeof(string));
    
            var riSubscriptionParameter = riSubscription.HasValue ?
                new ObjectParameter("riSubscription", riSubscription) :
                new ObjectParameter("riSubscription", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_GetUserDetailsForbackOffice_Result>("SG2_usp_GetUserDetailsForbackOffice", rsSearchCriteParameter, riPageNumberParameter, riPageSizeParameter, riStatusIdParameter, riProductIdParameter, riJVStatusParameter, riSubscriptionParameter);
        }
    
        public virtual ObjectResult<SG2_usp_Login_Customers_Result> SG2_usp_Login_Customers(string rvcEmailAddress, string rvcPassword, string rvcCreatedBy, string rvcLastLoginIP, Nullable<int> rvcStatusId)
        {
            var rvcEmailAddressParameter = rvcEmailAddress != null ?
                new ObjectParameter("rvcEmailAddress", rvcEmailAddress) :
                new ObjectParameter("rvcEmailAddress", typeof(string));
    
            var rvcPasswordParameter = rvcPassword != null ?
                new ObjectParameter("rvcPassword", rvcPassword) :
                new ObjectParameter("rvcPassword", typeof(string));
    
            var rvcCreatedByParameter = rvcCreatedBy != null ?
                new ObjectParameter("rvcCreatedBy", rvcCreatedBy) :
                new ObjectParameter("rvcCreatedBy", typeof(string));
    
            var rvcLastLoginIPParameter = rvcLastLoginIP != null ?
                new ObjectParameter("rvcLastLoginIP", rvcLastLoginIP) :
                new ObjectParameter("rvcLastLoginIP", typeof(string));
    
            var rvcStatusIdParameter = rvcStatusId.HasValue ?
                new ObjectParameter("rvcStatusId", rvcStatusId) :
                new ObjectParameter("rvcStatusId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_Login_Customers_Result>("SG2_usp_Login_Customers", rvcEmailAddressParameter, rvcPasswordParameter, rvcCreatedByParameter, rvcLastLoginIPParameter, rvcStatusIdParameter);
        }
    
        public virtual ObjectResult<SG2_usp_Customers_Get_Result> SG2_usp_Customers_Get(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_Customers_Get_Result>("SG2_usp_Customers_Get", idParameter);
        }
    
        public virtual ObjectResult<SG2_usp_SocialProfile_Payments_Save_Result> SG2_usp_SocialProfile_Payments_Save(Nullable<int> riSocialProfileId, string riStripeSubscriptionId, string rvcDescription, string rvcName, Nullable<decimal> riPrice, string riStripePlanId, string rvcSubscriptionType, Nullable<System.DateTime> rdtStartDate, Nullable<System.DateTime> rdtEndDate, Nullable<int> riStatusId, Nullable<int> riPaymentPlanId, string rvcStripeInvoiceId)
        {
            var riSocialProfileIdParameter = riSocialProfileId.HasValue ?
                new ObjectParameter("riSocialProfileId", riSocialProfileId) :
                new ObjectParameter("riSocialProfileId", typeof(int));
    
            var riStripeSubscriptionIdParameter = riStripeSubscriptionId != null ?
                new ObjectParameter("riStripeSubscriptionId", riStripeSubscriptionId) :
                new ObjectParameter("riStripeSubscriptionId", typeof(string));
    
            var rvcDescriptionParameter = rvcDescription != null ?
                new ObjectParameter("rvcDescription", rvcDescription) :
                new ObjectParameter("rvcDescription", typeof(string));
    
            var rvcNameParameter = rvcName != null ?
                new ObjectParameter("rvcName", rvcName) :
                new ObjectParameter("rvcName", typeof(string));
    
            var riPriceParameter = riPrice.HasValue ?
                new ObjectParameter("riPrice", riPrice) :
                new ObjectParameter("riPrice", typeof(decimal));
    
            var riStripePlanIdParameter = riStripePlanId != null ?
                new ObjectParameter("riStripePlanId", riStripePlanId) :
                new ObjectParameter("riStripePlanId", typeof(string));
    
            var rvcSubscriptionTypeParameter = rvcSubscriptionType != null ?
                new ObjectParameter("rvcSubscriptionType", rvcSubscriptionType) :
                new ObjectParameter("rvcSubscriptionType", typeof(string));
    
            var rdtStartDateParameter = rdtStartDate.HasValue ?
                new ObjectParameter("rdtStartDate", rdtStartDate) :
                new ObjectParameter("rdtStartDate", typeof(System.DateTime));
    
            var rdtEndDateParameter = rdtEndDate.HasValue ?
                new ObjectParameter("rdtEndDate", rdtEndDate) :
                new ObjectParameter("rdtEndDate", typeof(System.DateTime));
    
            var riStatusIdParameter = riStatusId.HasValue ?
                new ObjectParameter("riStatusId", riStatusId) :
                new ObjectParameter("riStatusId", typeof(int));
    
            var riPaymentPlanIdParameter = riPaymentPlanId.HasValue ?
                new ObjectParameter("riPaymentPlanId", riPaymentPlanId) :
                new ObjectParameter("riPaymentPlanId", typeof(int));
    
            var rvcStripeInvoiceIdParameter = rvcStripeInvoiceId != null ?
                new ObjectParameter("rvcStripeInvoiceId", rvcStripeInvoiceId) :
                new ObjectParameter("rvcStripeInvoiceId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_SocialProfile_Payments_Save_Result>("SG2_usp_SocialProfile_Payments_Save", riSocialProfileIdParameter, riStripeSubscriptionIdParameter, rvcDescriptionParameter, rvcNameParameter, riPriceParameter, riStripePlanIdParameter, rvcSubscriptionTypeParameter, rdtStartDateParameter, rdtEndDateParameter, riStatusIdParameter, riPaymentPlanIdParameter, rvcStripeInvoiceIdParameter);
        }
    
        public virtual ObjectResult<SG2_usp_Customer_SignUpCustomerWithPreference_Result> SG2_usp_Customer_SignUpCustomerWithPreference(string rvcFirstName, string rvcLastName, string rvcEmailAddress, string rvcPassword, string rvcGUID, string rvcLastLoginIP, string rvcPreference1, string rvcPreference2, string rvcPreference3, string rvcPreference4, Nullable<int> iPreference5, Nullable<int> iPreference6, Nullable<int> rvcCity, Nullable<int> rvcStatusId)
        {
            var rvcFirstNameParameter = rvcFirstName != null ?
                new ObjectParameter("rvcFirstName", rvcFirstName) :
                new ObjectParameter("rvcFirstName", typeof(string));
    
            var rvcLastNameParameter = rvcLastName != null ?
                new ObjectParameter("rvcLastName", rvcLastName) :
                new ObjectParameter("rvcLastName", typeof(string));
    
            var rvcEmailAddressParameter = rvcEmailAddress != null ?
                new ObjectParameter("rvcEmailAddress", rvcEmailAddress) :
                new ObjectParameter("rvcEmailAddress", typeof(string));
    
            var rvcPasswordParameter = rvcPassword != null ?
                new ObjectParameter("rvcPassword", rvcPassword) :
                new ObjectParameter("rvcPassword", typeof(string));
    
            var rvcGUIDParameter = rvcGUID != null ?
                new ObjectParameter("rvcGUID", rvcGUID) :
                new ObjectParameter("rvcGUID", typeof(string));
    
            var rvcLastLoginIPParameter = rvcLastLoginIP != null ?
                new ObjectParameter("rvcLastLoginIP", rvcLastLoginIP) :
                new ObjectParameter("rvcLastLoginIP", typeof(string));
    
            var rvcPreference1Parameter = rvcPreference1 != null ?
                new ObjectParameter("rvcPreference1", rvcPreference1) :
                new ObjectParameter("rvcPreference1", typeof(string));
    
            var rvcPreference2Parameter = rvcPreference2 != null ?
                new ObjectParameter("rvcPreference2", rvcPreference2) :
                new ObjectParameter("rvcPreference2", typeof(string));
    
            var rvcPreference3Parameter = rvcPreference3 != null ?
                new ObjectParameter("rvcPreference3", rvcPreference3) :
                new ObjectParameter("rvcPreference3", typeof(string));
    
            var rvcPreference4Parameter = rvcPreference4 != null ?
                new ObjectParameter("rvcPreference4", rvcPreference4) :
                new ObjectParameter("rvcPreference4", typeof(string));
    
            var iPreference5Parameter = iPreference5.HasValue ?
                new ObjectParameter("iPreference5", iPreference5) :
                new ObjectParameter("iPreference5", typeof(int));
    
            var iPreference6Parameter = iPreference6.HasValue ?
                new ObjectParameter("iPreference6", iPreference6) :
                new ObjectParameter("iPreference6", typeof(int));
    
            var rvcCityParameter = rvcCity.HasValue ?
                new ObjectParameter("rvcCity", rvcCity) :
                new ObjectParameter("rvcCity", typeof(int));
    
            var rvcStatusIdParameter = rvcStatusId.HasValue ?
                new ObjectParameter("rvcStatusId", rvcStatusId) :
                new ObjectParameter("rvcStatusId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_Customer_SignUpCustomerWithPreference_Result>("SG2_usp_Customer_SignUpCustomerWithPreference", rvcFirstNameParameter, rvcLastNameParameter, rvcEmailAddressParameter, rvcPasswordParameter, rvcGUIDParameter, rvcLastLoginIPParameter, rvcPreference1Parameter, rvcPreference2Parameter, rvcPreference3Parameter, rvcPreference4Parameter, iPreference5Parameter, iPreference6Parameter, rvcCityParameter, rvcStatusIdParameter);
        }
    
        public virtual ObjectResult<SG2_usp_Get_AllUser_Result> SG2_usp_Get_AllUser()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_Get_AllUser_Result>("SG2_usp_Get_AllUser");
        }
    
        public virtual ObjectResult<SG2_usp_Customer_GetSocialProfilesByCustomerId_Result> SG2_usp_Customer_GetSocialProfilesByCustomerId(Nullable<int> customerId)
        {
            var customerIdParameter = customerId.HasValue ?
                new ObjectParameter("CustomerId", customerId) :
                new ObjectParameter("CustomerId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_Customer_GetSocialProfilesByCustomerId_Result>("SG2_usp_Customer_GetSocialProfilesByCustomerId", customerIdParameter);
        }
    
        public virtual ObjectResult<SG2_usp_LikeyAccount_GetAll_Result> SG2_usp_LikeyAccount_GetAll(string rsSearchCrite, Nullable<int> riPageNumber, string riPageSize, Nullable<int> riStatusId)
        {
            var rsSearchCriteParameter = rsSearchCrite != null ?
                new ObjectParameter("rsSearchCrite", rsSearchCrite) :
                new ObjectParameter("rsSearchCrite", typeof(string));
    
            var riPageNumberParameter = riPageNumber.HasValue ?
                new ObjectParameter("riPageNumber", riPageNumber) :
                new ObjectParameter("riPageNumber", typeof(int));
    
            var riPageSizeParameter = riPageSize != null ?
                new ObjectParameter("riPageSize", riPageSize) :
                new ObjectParameter("riPageSize", typeof(string));
    
            var riStatusIdParameter = riStatusId.HasValue ?
                new ObjectParameter("riStatusId", riStatusId) :
                new ObjectParameter("riStatusId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_LikeyAccount_GetAll_Result>("SG2_usp_LikeyAccount_GetAll", rsSearchCriteParameter, riPageNumberParameter, riPageSizeParameter, riStatusIdParameter);
        }
    
        public virtual ObjectResult<SG2_usp_PlanInformation_GetAll_Result> SG2_usp_PlanInformation_GetAll(string rsSearchCrite, Nullable<int> riPageNumber, string riPageSize, Nullable<int> riStatusId)
        {
            var rsSearchCriteParameter = rsSearchCrite != null ?
                new ObjectParameter("rsSearchCrite", rsSearchCrite) :
                new ObjectParameter("rsSearchCrite", typeof(string));
    
            var riPageNumberParameter = riPageNumber.HasValue ?
                new ObjectParameter("riPageNumber", riPageNumber) :
                new ObjectParameter("riPageNumber", typeof(int));
    
            var riPageSizeParameter = riPageSize != null ?
                new ObjectParameter("riPageSize", riPageSize) :
                new ObjectParameter("riPageSize", typeof(string));
    
            var riStatusIdParameter = riStatusId.HasValue ?
                new ObjectParameter("riStatusId", riStatusId) :
                new ObjectParameter("riStatusId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_PlanInformation_GetAll_Result>("SG2_usp_PlanInformation_GetAll", rsSearchCriteParameter, riPageNumberParameter, riPageSizeParameter, riStatusIdParameter);
        }
    
        public virtual ObjectResult<SG2_usp_Get_SocialProfile_PaymentPlan_Result> SG2_usp_Get_SocialProfile_PaymentPlan()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_Get_SocialProfile_PaymentPlan_Result>("SG2_usp_Get_SocialProfile_PaymentPlan");
        }
    
        public virtual ObjectResult<SG2_usp_SystemUser_Login_Result> SG2_usp_SystemUser_Login(string rvcEmailAddress, string rvcPassword)
        {
            var rvcEmailAddressParameter = rvcEmailAddress != null ?
                new ObjectParameter("rvcEmailAddress", rvcEmailAddress) :
                new ObjectParameter("rvcEmailAddress", typeof(string));
    
            var rvcPasswordParameter = rvcPassword != null ?
                new ObjectParameter("rvcPassword", rvcPassword) :
                new ObjectParameter("rvcPassword", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_SystemUser_Login_Result>("SG2_usp_SystemUser_Login", rvcEmailAddressParameter, rvcPasswordParameter);
        }
    
        public virtual ObjectResult<SG2_usp_Report_GetMostUsedProductData_Result> SG2_usp_Report_GetMostUsedProductData(Nullable<System.DateTime> dtFromDate, Nullable<System.DateTime> dtToDate)
        {
            var dtFromDateParameter = dtFromDate.HasValue ?
                new ObjectParameter("dtFromDate", dtFromDate) :
                new ObjectParameter("dtFromDate", typeof(System.DateTime));
    
            var dtToDateParameter = dtToDate.HasValue ?
                new ObjectParameter("dtToDate", dtToDate) :
                new ObjectParameter("dtToDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_Report_GetMostUsedProductData_Result>("SG2_usp_Report_GetMostUsedProductData", dtFromDateParameter, dtToDateParameter);
        }
    
        public virtual ObjectResult<SG2_usp_SocialProfile_GetNotificationsByStatus_Result> SG2_usp_SocialProfile_GetNotificationsByStatus(Nullable<short> statusId)
        {
            var statusIdParameter = statusId.HasValue ?
                new ObjectParameter("StatusId", statusId) :
                new ObjectParameter("StatusId", typeof(short));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_SocialProfile_GetNotificationsByStatus_Result>("SG2_usp_SocialProfile_GetNotificationsByStatus", statusIdParameter);
        }
    
        public virtual ObjectResult<SG2_usp_Get_EnumerationValue_Result> SG2_usp_Get_EnumerationValue()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_Get_EnumerationValue_Result>("SG2_usp_Get_EnumerationValue");
        }
    
        public virtual ObjectResult<SG2_usp_Customer_GetSocialProfileById_Result> SG2_usp_Customer_GetSocialProfileById(Nullable<int> riSocialProfileId)
        {
            var riSocialProfileIdParameter = riSocialProfileId.HasValue ?
                new ObjectParameter("riSocialProfileId", riSocialProfileId) :
                new ObjectParameter("riSocialProfileId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_Customer_GetSocialProfileById_Result>("SG2_usp_Customer_GetSocialProfileById", riSocialProfileIdParameter);
        }
    
        public virtual ObjectResult<SG2_usp_Customer_SignUp_Result> SG2_usp_Customer_SignUp(string rvcFirstName, string rvcSurName, string rvcEmailAddress, string rvcPassword, string rvcCreatedBy, string rvcGUID, string rvcLastLoginIP, Nullable<int> rvcStatusId)
        {
            var rvcFirstNameParameter = rvcFirstName != null ?
                new ObjectParameter("rvcFirstName", rvcFirstName) :
                new ObjectParameter("rvcFirstName", typeof(string));
    
            var rvcSurNameParameter = rvcSurName != null ?
                new ObjectParameter("rvcSurName", rvcSurName) :
                new ObjectParameter("rvcSurName", typeof(string));
    
            var rvcEmailAddressParameter = rvcEmailAddress != null ?
                new ObjectParameter("rvcEmailAddress", rvcEmailAddress) :
                new ObjectParameter("rvcEmailAddress", typeof(string));
    
            var rvcPasswordParameter = rvcPassword != null ?
                new ObjectParameter("rvcPassword", rvcPassword) :
                new ObjectParameter("rvcPassword", typeof(string));
    
            var rvcCreatedByParameter = rvcCreatedBy != null ?
                new ObjectParameter("rvcCreatedBy", rvcCreatedBy) :
                new ObjectParameter("rvcCreatedBy", typeof(string));
    
            var rvcGUIDParameter = rvcGUID != null ?
                new ObjectParameter("rvcGUID", rvcGUID) :
                new ObjectParameter("rvcGUID", typeof(string));
    
            var rvcLastLoginIPParameter = rvcLastLoginIP != null ?
                new ObjectParameter("rvcLastLoginIP", rvcLastLoginIP) :
                new ObjectParameter("rvcLastLoginIP", typeof(string));
    
            var rvcStatusIdParameter = rvcStatusId.HasValue ?
                new ObjectParameter("rvcStatusId", rvcStatusId) :
                new ObjectParameter("rvcStatusId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SG2_usp_Customer_SignUp_Result>("SG2_usp_Customer_SignUp", rvcFirstNameParameter, rvcSurNameParameter, rvcEmailAddressParameter, rvcPasswordParameter, rvcCreatedByParameter, rvcGUIDParameter, rvcLastLoginIPParameter, rvcStatusIdParameter);
        }
    }
}
