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
    
    public partial class PageScore : BaseEntity
    {
        public System.Guid Id { get; set; }
        public System.Guid ProfileId { get; set; }
        public System.Guid PageToScore { get; set; }
        public Nullable<int> Score { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    
        public virtual Page Page { get; set; }
        public virtual Page Page1 { get; set; }
    }
}
