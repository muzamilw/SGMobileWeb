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
    
    public partial class SG2_JVBox
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SG2_JVBox()
        {
            this.SG2_SocialProfile = new HashSet<SG2_SocialProfile>();
        }
    
        public int JVBoxId { get; set; }
        public string BoxName { get; set; }
        public string AdminName { get; set; }
        public string AdminPassword { get; set; }
        public string BoxManagedBy { get; set; }
        public string SupportPhone { get; set; }
        public string SupportEmail { get; set; }
        public string HostedBy { get; set; }
        public string HostingPhone { get; set; }
        public string HostingWebsite { get; set; }
        public string HostingAccount { get; set; }
        public string HostingPassword { get; set; }
        public string HostingPriceInfo { get; set; }
        public int StatusId { get; set; }
        public Nullable<int> MaxLimit { get; set; }
        public Nullable<int> JVBoxType { get; set; }
        public string ExchangeName { get; set; }
        public Nullable<short> QueueStatusId { get; set; }
        public string PRCExecProfileId { get; set; }
        public Nullable<int> ServerRunningStatusId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SG2_SocialProfile> SG2_SocialProfile { get; set; }
    }
}
