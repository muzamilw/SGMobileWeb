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
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class vwRunNotificationsData
    {
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<int> SocialProfileTypeId { get; set; }
        public string SocialUsername { get; set; }
        public string AppTimeZoneOffSet { get; set; }
        public string ExecutionIntervals { get; set; }
        [NotMapped]
        public string startTime { get; set; }
    }
}
