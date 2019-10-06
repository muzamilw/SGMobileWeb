//------------------------------------------------------------------------------
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
    
    public partial class SG2_usp_Customer_SignUp_Result
    {
        public int CustomerId { get; set; }
        public string GUID { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public short StatusId { get; set; }
        public System.DateTime LastLoginDate { get; set; }
        public byte LoginAttempts { get; set; }
        public string LastLoginIP { get; set; }
        public string Tocken { get; set; }
        public Nullable<int> JVBoxId { get; set; }
        public string StripeCustomerId { get; set; }
        public string UserName { get; set; }
        public Nullable<int> JVBoxStatusId { get; set; }
        public string Source { get; set; }
        public string Register { get; set; }
        public Nullable<int> ResponsibleTeamMemberId { get; set; }
        public Nullable<bool> AvailableToEveryOne { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> CancelledDate { get; set; }
        public Nullable<bool> IsOptedEducationalEmailSeries { get; set; }
        public Nullable<bool> IsOptedMarketingEmail { get; set; }
        public Nullable<int> ContactDetailsId { get; set; }
        public string JobTitle { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Sate { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string PhoneCode { get; set; }
        public string StripeSubscriptionId { get; set; }
    }
}
