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
    
    public partial class Order : BaseEntity
    {
        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }
    
        public System.Guid Id { get; set; }
        public System.Guid PageId { get; set; }
        public System.Guid CustomerPageId { get; set; }
        public long OrderNumber { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string Description { get; set; }
    
        public virtual Page Page { get; set; }
        public virtual Page Page1 { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}