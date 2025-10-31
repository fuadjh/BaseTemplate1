
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context.Configurations
{
    //internal class AccountConfig : IEntityTypeConfiguration<Account>
    //{
    //    public void Configure(EntityTypeBuilder<Account> builder)
    //    {
    //        builder
    //              .ToTable("Accounts", "Banking")
    //              .HasIndex(a => a.AccountNumber)
    //              .IsUnique()
    //              .HasDatabaseName("IX_Accounts_AccountNumber");
    //        builder
    //            .Property(a => a.Type)
    //            .HasConversion(new EnumToStringConverter<AccuontType>());
    //    }
    //}

    //internal class AccountHolderConfig : IEntityTypeConfiguration<AccountHolder>
    //{
    //    public void Configure(EntityTypeBuilder<AccountHolder> builder)
    //    {
    //        builder
    //            .ToTable("AccountHolders", "Banking");
    //    }
    //}

    //internal class TransectionConfig : IEntityTypeConfiguration<Transection>
    //{
    //    public void Configure(EntityTypeBuilder<Transection> builder)
    //    {
    //        builder
    //            .ToTable("Transections", "Banking")
    //            .Property(a => a.Type)
    //            .HasConversion(new EnumToStringConverter<TransectionType>());
    //    }
    //}
}