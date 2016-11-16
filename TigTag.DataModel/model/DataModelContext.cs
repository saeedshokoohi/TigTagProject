using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DataModel.model
{
    
        public partial class DataModelContext : DbContext
        {
            public DataModelContext()
                : base("name=tigtag_dbEntities")
            {
            }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
               
            }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<FollowCondition> FollowConditions { get; set; }
        public DbSet<FollowMenu> FollowMenus { get; set; }
        public DbSet<FollowMenuPackage> FollowMenuPackages { get; set; }
        public DbSet<ImageTable> ImageTables { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PageMenu> PageMenus { get; set; }
        public DbSet<CommentReply> CommentReplys { get; set; }
        public DbSet<PageComment> PageComments { get; set; }
        public DbSet<PageSetting> PageSettings { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<PageAdmin> PageAdmins  { get; set; }
        public DbSet<PageScore> PageScores { get; set; }
        public DbSet<EventsLog> EventsLogs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceTransaction> InvoiceTransactions { get; set; }



    }
    
}
