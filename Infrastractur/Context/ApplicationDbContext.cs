
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
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
       // public DbSet<AccountHolder> accountHolders => Set<AccountHolder>();
       // public DbSet<Account> accounts => Set<Account>();
       // public DbSet<Transection> Transections => Set<Transection>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
             * تمام کلاس‌هایی را که در همین (پروژه) هستند و رابط
               IEntityTypeConfiguration<T> 
               را پیاده‌سازی کرده‌اند، به صورت خودکار پیدا و اعمال کن                
             */
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}