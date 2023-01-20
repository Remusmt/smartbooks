using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.SaccoEntities.Transactions;

namespace SmartBooks.Persitence.Data.Config
{
    public class TransactionItemConfiguration : IEntityTypeConfiguration<TransactionDetail>
    {
        public void Configure(EntityTypeBuilder<TransactionDetail> builder)
        {
            builder.HasOne(e => e.Transaction)
                .WithMany(e => e.TransactionDetails)
                .HasForeignKey(e => e.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.CostCenter)
                .WithMany()
                .HasForeignKey(e => e.CostCenterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.TransactionItem)
                .WithMany()
                .HasForeignKey(e => e.TransactionItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class FeeItemConfiguration : IEntityTypeConfiguration<FeeItem>
    {
        public void Configure(EntityTypeBuilder<FeeItem> builder)
        {
            builder.HasOne(e => e.MemberAccount)
                .WithMany()
                .HasForeignKey(e => e.MemberAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
