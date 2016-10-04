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
    
    public partial class Ticket : BaseEntity
    {
        public Ticket()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }
    
        public System.Guid Id { get; set; }
        public string Title { get; set; }
        public System.Guid PageId { get; set; }
        public string Description { get; set; }
        public Nullable<int> Capacity { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<System.DateTime> DeadlineDate { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public Nullable<int> SoldCapacity { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual Page Page { get; set; }
    }
}
