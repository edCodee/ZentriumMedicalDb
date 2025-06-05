using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using HospitalAPI.Models;

namespace APIhospital.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //Tablas
        public DbSet<UserModel> user { get; set; }
        public DbSet<RoleModel> role { get; set; }
        public DbSet<UserRoleModel> user_role { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRoleModel>()
                .HasKey(ur => new { ur.userrole_userserial, ur.userrole_roleserial });

            modelBuilder.Entity<UserRoleModel>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.userrole_userserial);

            modelBuilder.Entity<UserRoleModel>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.userrole_roleserial);

            // También es buena práctica mapear el nombre de tabla si es lowercase
            modelBuilder.Entity<RoleModel>().ToTable("role");
            modelBuilder.Entity<UserRoleModel>().ToTable("user_role");
        }

    }
}
