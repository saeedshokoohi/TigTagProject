﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class tigtag_dbEntities : DbContext
    {
        public tigtag_dbEntities()
            : base("name=tigtag_dbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<CommentReply> CommentReplies { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<FollowCondition> FollowConditions { get; set; }
        public DbSet<FollowMenu> FollowMenus { get; set; }
        public DbSet<FollowMenuPackage> FollowMenuPackages { get; set; }
        public DbSet<ImageTable> ImageTables { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageComment> PageComments { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
