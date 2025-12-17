using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context.Configurations
{
    public class LmsUserConfiguration : IEntityTypeConfiguration<LmsUser>
    {
        public void Configure(EntityTypeBuilder<LmsUser> builder)
        {
            builder.ToTable("LmsUsers");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.IdentityUserId)
                   .IsRequired();

            builder.Property(u => u.UserType)
                   .IsRequired();

            builder.Property(u => u.IsActive)
                   .IsRequired();

            builder.Property(u => u.CreatedAt)
                   .IsRequired();

            // ---------- UserProfile ----------
            builder.OwnsOne(u => u.Profile, profile =>
            {
                profile.Property(p => p.FirstName)
                       .HasMaxLength(100)
                       .IsRequired();

                profile.Property(p => p.LastName)
                       .HasMaxLength(100)
                       .IsRequired();

                profile.Property(p => p.NationalCode)
                       .HasMaxLength(10)
                       .IsRequired();

                profile.Property(p => p.FatherName)
                       .HasMaxLength(100);

                profile.Property(p => p.AvatarUrl)
                       .HasMaxLength(500);

                profile.HasIndex(p => p.NationalCode)
                       .IsUnique()
                       .HasDatabaseName("UX_LmsUser_NationalCode");
            });

            // ---------- Email ----------
            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Address)
                     .HasMaxLength(320)
                     .IsRequired();

                email.HasIndex(e => e.Address)
                     .IsUnique()
                     .HasDatabaseName("UX_LmsUser_Email");
            });

            // ---------- Mobile ----------
            builder.OwnsOne(u => u.Mobile, mobile =>
            {
                mobile.Property(m => m.Number)
                      .HasMaxLength(11)
                      .IsRequired();

                mobile.HasIndex(m => m.Number)
                      .IsUnique()
                      .HasDatabaseName("UX_LmsUser_Mobile");
            });
        }
    }
}
