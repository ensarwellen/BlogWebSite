using Blog.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data
{
    public class DataContext : DbContext
    {
        public DataContext(string connectionString) : base(new DbContextOptionsBuilder().UseMySQL(connectionString).Options)
        {
        }

        public DbSet<Model.Author> Authors { get; set; }
        public DbSet<Model.Category> Categories { get; set; }
        public DbSet<Model.Comment> Comments { get; set; }
        public DbSet<Model.Content> Contents { get; set; }
        public DbSet<Model.ContentCategory> ContentCategories { get; set; }
        public DbSet<Model.ContentTag> ContentTags { get; set; }
        public DbSet<Model.Media> Medias { get; set; }
        public DbSet<Model.Tag> Tags { get; set; }
        public DbSet<Model.Setting> Setting { get; set; }
        public DbSet<Model.Role> Roles { get; set; }
        public DbSet<Model.RolePage> RolePages { get; set; }





        protected override void OnModelCreating(ModelBuilder builder) 
        {
            builder.Entity<Model.Author>(entity => entity.ToTable("bl_authors"));
            builder.Entity<Model.Category>(entity => entity.ToTable("bl_categories"));
            builder.Entity<Model.Comment>(entity => entity.ToTable("bl_comments"));
            builder.Entity<Model.Content>(entity => entity.ToTable("bl_contents"));
            builder.Entity<Model.ContentCategory>(entity => entity.ToTable("bl_content_categories"));
            builder.Entity<Model.ContentTag>(entity => entity.ToTable("bl_content_tags"));
            builder.Entity<Model.Media>(entity => entity.ToTable("bl_medias"));
            builder.Entity<Model.Tag>(entity => entity.ToTable("bl_tags"));
            builder.Entity<Model.Setting>(entity => entity.ToTable("bl_setting"));
            builder.Entity<Model.Role>(entity => entity.ToTable("bl_roles"));
            builder.Entity<Model.RolePage>(entity => entity.ToTable("bl_role_pages"));




            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if(property.ClrType == typeof(bool))
                    {
                        property.SetValueConverter(new BoolToIntConverter());
                    }
                }
            }
        }
    }
}
