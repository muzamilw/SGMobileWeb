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
    
    public partial class SG2_EnumerationValue
    {
        public short EnumerationValueId { get; set; }
        public short EnumerationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public int SequenceNo { get; set; }
        public Nullable<bool> IsVisible { get; set; }
    
        public virtual SG2_Enumeration SG2_Enumeration { get; set; }
    }
}
