//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TigTag.DataModel.model
{
    using System;
    using System.Collections.Generic;
    
    public partial class FollowMenu : BaseEntity
    {
        public FollowMenu()
        {
            this.FollowMenuPackages = new HashSet<FollowMenuPackage>();
        }
    
        public System.Guid Id { get; set; }
        public string title { get; set; }
        public System.DateTime date { get; set; }
        public Nullable<System.DateTime> lastVisitDate { get; set; }
        public System.Guid FollowerUserId { get; set; }
    
        public virtual Page Page { get; set; }
        public virtual ICollection<FollowMenuPackage> FollowMenuPackages { get; set; }
    }
}
