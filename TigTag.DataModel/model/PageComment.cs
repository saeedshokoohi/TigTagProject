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
    
    public partial class PageComment : BaseEntity
    {
        public PageComment()
        {
            this.CommentReplies = new HashSet<CommentReply>();
        }
    
        public System.Guid Id { get; set; }
        public System.Guid AutherId { get; set; }
        public System.Guid PageId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CommentText { get; set; }
    
        public virtual ICollection<CommentReply> CommentReplies { get; set; }
        public virtual Page Page { get; set; }
        public virtual Page Page1 { get; set; }
    }
}
