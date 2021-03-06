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
            this.EventsLogs = new HashSet<EventsLog>();
            this.EventsLogs1 = new HashSet<EventsLog>();
            this.Follows = new HashSet<Follow>();
            this.Follows1 = new HashSet<Follow>();
            this.FollowMenus = new HashSet<FollowMenu>();
            this.Menus = new HashSet<Menu>();
            this.Page1 = new HashSet<Page>();
            this.PageAdmins = new HashSet<PageAdmin>();
            this.PageAdmins1 = new HashSet<PageAdmin>();
            this.PageComments = new HashSet<PageComment>();
            this.PageComments1 = new HashSet<PageComment>();
            this.PageScores = new HashSet<PageScore>();
            this.PageScores1 = new HashSet<PageScore>();
            this.PageSettings = new HashSet<PageSetting>();
            this.PageMenus = new HashSet<PageMenu>();
            this.Participants = new HashSet<Participant>();
            this.Participants1 = new HashSet<Participant>();
            this.ContactInfoes = new HashSet<ContactInfo>();
            this.Orders = new HashSet<Order>();
            this.Orders1 = new HashSet<Order>();
            this.Tickets = new HashSet<Ticket>();
        }
    
        public System.Guid Id { get; set; }
        public string PageTitle { get; set; }
        public string Description { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int PageType { get; set; }
        public System.Guid UserId { get; set; }
        public Nullable<System.Guid> PageId { get; set; }
        public Nullable<System.Guid> ImageId { get; set; }
        public string URL { get; set; }
        public Nullable<bool> IsMasterPage { get; set; }
        public string Color { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual ICollection<CommentReply> CommentReplies { get; set; }
        public virtual ICollection<EventsLog> EventsLogs { get; set; }
        public virtual ICollection<EventsLog> EventsLogs1 { get; set; }
        public virtual ICollection<Follow> Follows { get; set; }
        public virtual ICollection<Follow> Follows1 { get; set; }
        public virtual ICollection<FollowMenu> FollowMenus { get; set; }
        public virtual ImageTable ImageTable { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<Page> Page1 { get; set; }
        public virtual Page Page2 { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<PageAdmin> PageAdmins { get; set; }
        public virtual ICollection<PageAdmin> PageAdmins1 { get; set; }
        public virtual ICollection<PageComment> PageComments { get; set; }
        public virtual ICollection<PageComment> PageComments1 { get; set; }
        public virtual ICollection<PageScore> PageScores { get; set; }
        public virtual ICollection<PageScore> PageScores1 { get; set; }
        public virtual ICollection<PageSetting> PageSettings { get; set; }
        public virtual ICollection<PageMenu> PageMenus { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<Participant> Participants1 { get; set; }
        public virtual ICollection<ContactInfo> ContactInfoes { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Order> Orders1 { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
