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
    using System.Collections.Generic;
    
    public partial class SG2_Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SG2_Customer()
        {
            this.SG2_Customer_ContactDetail = new HashSet<SG2_Customer_ContactDetail>();
            this.SG2_SocialProfile = new HashSet<SG2_SocialProfile>();
        }
    
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
        public string StripeCustomerId { get; set; }
        public string UserName { get; set; }
        public string Source { get; set; }
        public string Register { get; set; }
        public Nullable<int> ResponsibleTeamMemberId { get; set; }
        public Nullable<bool> AvailableToEveryOne { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> CancelledDate { get; set; }
        public Nullable<bool> IsOptedEducationalEmailSeries { get; set; }
        public Nullable<bool> IsOptedMarketingEmail { get; set; }
        public string Title { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SG2_Customer_ContactDetail> SG2_Customer_ContactDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SG2_SocialProfile> SG2_SocialProfile { get; set; }
    }
}
