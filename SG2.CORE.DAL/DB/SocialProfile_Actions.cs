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
    
    public partial class SocialProfile_Actions
    {
        public long SPSHId { get; set; }
        public Nullable<int> SocialProfileId { get; set; }
        public Nullable<int> ActionID { get; set; }
        public string TargetProfile { get; set; }
        public Nullable<System.DateTime> ActionDateTime { get; set; }
    
        public virtual SocialProfile SocialProfile { get; set; }
    }
}