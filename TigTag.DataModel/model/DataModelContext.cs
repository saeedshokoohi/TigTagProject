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

            public DbSet<User> Users { get; set; }
            public DbSet<BasePage> BasePages { get; set; }
        }
    
}
