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
    
    public partial class SystemUser
    {
        public int SystemUserId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public short SystemRoleId { get; set; }
        public string Password { get; set; }
        public short StatusId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public bool HostUser { get; set; }
    
        public virtual SystemRole SystemRole { get; set; }
    }
}
