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
    
    public partial class SG2_usp_SocialProfile_Statistics_GetFollowers_Result
    {
        public int SocialProfileId { get; set; }
        public string Username { get; set; }
        public Nullable<long> FollowersGain { get; set; }
        public Nullable<long> followers { get; set; }
        public Nullable<long> Followings { get; set; }
        public Nullable<decimal> FollowingsRatio { get; set; }
        public Nullable<long> AVGFollowers { get; set; }
        public Nullable<long> Like { get; set; }
        public Nullable<long> Comment { get; set; }
        public Nullable<long> Engagement { get; set; }
        public Nullable<long> LikeComments { get; set; }
        public System.DateTime Date { get; set; }
    }
}
