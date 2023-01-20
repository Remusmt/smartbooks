using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBooks.Domains.SaccoEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBooks.Persitence.Data.Config
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasOne(e => e.ApplicationUser)
                .WithMany()
                .HasForeignKey(e => e.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.HomeAddress)
                .WithMany()
                .HasForeignKey(e => e.HomeAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(e => e.PermanentAddress)
                .WithMany()
                .HasForeignKey(e => e.PermanentAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }

        public class MemberApprovalConfiguration : IEntityTypeConfiguration<MemberApproval>
        {
            public void Configure(EntityTypeBuilder<MemberApproval> builder)
            {
                builder.HasOne(e => e.Member)
                    .WithMany(e => e.MemberApprovals)
                    .HasForeignKey(e => e.MemberId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }

        public class MemberAttachmentConfiguration : IEntityTypeConfiguration<MemberAttachment>
        {
            public void Configure(EntityTypeBuilder<MemberAttachment> builder)
            {
                builder.HasOne(e => e.Member)
                    .WithMany(e => e.MemberAttachments)
                    .HasForeignKey(e => e.MemberId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(e => e.Attachment)
                    .WithMany()
                    .HasForeignKey(e => e.AttachmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class NextOfKinConfiguration : IEntityTypeConfiguration<NextOfKin>
        {
            public void Configure(EntityTypeBuilder<NextOfKin> builder)
            {
                builder.HasOne(e => e.Member)
                    .WithMany(e => e.NextOfKins)
                    .HasForeignKey(e => e.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class MemberAccountConfiguration : IEntityTypeConfiguration<MemberAccount>
        {
            public void Configure(EntityTypeBuilder<MemberAccount> builder)
            {
                builder.HasOne(e => e.Member)
                    .WithMany(e => e.MemberAccounts)
                    .HasForeignKey(e => e.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(e => e.MemberAccountType)
                    .WithMany()
                    .HasForeignKey(e => e.MemberAccountTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }

    }
}
