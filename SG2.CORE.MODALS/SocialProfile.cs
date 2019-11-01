//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SG2.CORE.MODAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class SocialProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SocialProfile()
        {
            this.SocialProfile_Statistics = new HashSet<SocialProfile_Statistics>();
            this.SocialProfile_Actions = new HashSet<SocialProfile_Actions>();
            this.SocialProfile_Payments = new HashSet<SocialProfile_Payments>();
            this.SocialProfile_Instagram_TargetingInformation = new HashSet<SocialProfile_Instagram_TargetingInformation>();
            this.SocialProfile_FollowedAccounts = new HashSet<SocialProfile_FollowedAccounts>();
        }
    
        public int SocialProfileId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> SocialProfileTypeId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string StripeSubscriptionId { get; set; }
        public Nullable<int> StripeCustomerId { get; set; }
        public Nullable<int> PaymentPlanId { get; set; }
        public string SocialUsername { get; set; }
        public string SocialPassword { get; set; }
        public string SocialProfileName { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string verificationCode { get; set; }
        public Nullable<bool> IsArchived { get; set; }
        public string IMSI { get; set; }
        public string DeviceIMEI { get; set; }
        public Nullable<int> DeviceStatus { get; set; }
        public string PinCode { get; set; }
        public string Comments { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual PaymentPlan PaymentPlan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SocialProfile_Statistics> SocialProfile_Statistics { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SocialProfile_Actions> SocialProfile_Actions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SocialProfile_Payments> SocialProfile_Payments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SocialProfile_Instagram_TargetingInformation> SocialProfile_Instagram_TargetingInformation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SocialProfile_FollowedAccounts> SocialProfile_FollowedAccounts { get; set; }
    }
}