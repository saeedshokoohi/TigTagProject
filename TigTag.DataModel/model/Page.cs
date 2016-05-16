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
    
    public partial class Page : BaseEntity
    {
        public Page()
        {
            this.CommentReplies = new HashSet<CommentReply>();
            this.Follows = new HashSet<Follow>();
            this.Menus = new HashSet<Menu>();
            this.Page1 = new HashSet<Page>();
            this.PageComments = new HashSet<PageComment>();
            this.PageComments1 = new HashSet<PageComment>();
        }
    
        public System.Guid Id { get; set; }
        public string PageTitle { get; set; }
        public string Description { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool PageType { get; set; }
        public System.Guid UserId { get; set; }
        public Nullable<System.Guid> PageId { get; set; }
        public Nullable<System.Guid> ImageId { get; set; }
    
        public virtual ICollection<CommentReply> CommentReplies { get; set; }
        public virtual ICollection<Follow> Follows { get; set; }
        public virtual ImageTable ImageTable { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<Page> Page1 { get; set; }
        public virtual Page Page2 { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<PageComment> PageComments { get; set; }
        public virtual ICollection<PageComment> PageComments1 { get; set; }
    }
}
