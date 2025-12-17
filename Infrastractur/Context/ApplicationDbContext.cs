
using Infrastructure.IdentityModels;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)

        {
        }


        public DbSet<ApplicationPermission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        // public DbSet<AccountHolder> accountHolders => Set<AccountHolder>();
        // public DbSet<Account> accounts => Set<Account>();
        // public DbSet<Transection> Transections => Set<Transection>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RolePermission>()
        .HasKey(rp => new { rp.RoleId, rp.PermissionId }); // تعریف کلید مرکب

            builder.Entity<ApplicationPermission>().HasData(
                new ApplicationPermission { Id = 1, Name = "Create", DisplayName= "Create" },
                new ApplicationPermission { Id = 2, Name = "Edit" , DisplayName = "Edit" },
                new ApplicationPermission { Id = 3, Name = "Delete", DisplayName= "Delete" }
            );




            /*
             * تمام کلاس‌هایی را که در همین (پروژه) هستند و رابط
               IEntityTypeConfiguration<T> 
               را پیاده‌سازی کرده‌اند، به صورت خودکار پیدا و اعمال کن                
             */
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(builder);
        }
    }
}