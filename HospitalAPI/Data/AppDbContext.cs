using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using APIhospital.Models;
using System.Collections.Generic;

namespace APIhospital.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //Tablas
        public DbSet<RoleModel> roles { get; set; }
        public DbSet<UserModel> users { get; set; }



    }
}
