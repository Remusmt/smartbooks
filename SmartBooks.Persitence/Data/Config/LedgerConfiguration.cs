using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBooks.Domains.Entities;
using SmartBooks.Domains.SaccoEntities;

namespace SmartBooks.Persitence.Data.Config
{
    public class LedgerConfiguration : IEntityTypeConfiguration<LedgerAccount>
    {
        public void Configure(EntityTypeBuilder<LedgerAccount> builder)
        {
            builder.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ParentAccount)
                .WithMany(e => e.ChildAccounts)
                .HasForeignKey(e => e.ParentAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(e => e.TaxRate)
                .WithMany()
                .HasForeignKey(e => e.TaxRateId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }

    public class JournalConfiguration : IEntityTypeConfiguration<JournalDetail>
    {
        public void Configure(EntityTypeBuilder<JournalDetail> builder)
        {
            builder.HasOne(e => e.Journal)
                .WithMany(e => e.JournalDetails)
                .HasForeignKey(e => e.JournalId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.CostCenter)
                .WithMany()
                .HasForeignKey(e => e.CostCenterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.BusinessUnit)
                .WithMany()
                .HasForeignKey(e => e.BusinessUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.LedgerAccount)
                .WithMany()
                .HasForeignKey(e => e.LedgerAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Organisation)
                .WithMany()
                .HasForeignKey(e => e.SubLedgerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.TaxRate)
                .WithMany()
                .HasForeignKey(e => e.TaxRateId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Transaction)
                .WithMany()
                .HasForeignKey(e => e.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.TransactionDetail)
                .WithMany()
                .HasForeignKey(e => e.TransactionDetailId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class GeneralLedgerConfiguration : IEntityTypeConfiguration<GeneralLedger>
    {
        public void Configure(EntityTypeBuilder<GeneralLedger> builder)
        {
            builder.HasOne(e => e.LedgerAccount)
                .WithMany()
                .HasForeignKey(e => e.LedgerAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Journal)
                .WithMany()
                .HasForeignKey(e => e.JournalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.JournalDetail)
                .WithMany()
                .HasForeignKey(e => e.JournalDetailId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }

    public class LedgerEntryConfiguration : IEntityTypeConfiguration<LedgerEntry>
    {
        public void Configure(EntityTypeBuilder<LedgerEntry> builder)
        {
            builder.HasOne(e => e.Journal)
                .WithMany()
                .HasForeignKey(e => e.JournalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.DebitAccount)
                .WithMany()
                .HasForeignKey(e => e.DebitAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.CreditAccount)
                .WithMany()
                .HasForeignKey(e => e.CreditAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.JournalDetail)
                .WithMany()
                .HasForeignKey(e => e.JournalDetailId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }

    public class FinancialYearConfiguration : IEntityTypeConfiguration<FinancialYear>
    {
        public void Configure(EntityTypeBuilder<FinancialYear> builder)
        {
            builder.HasOne(e => e.OpeningJournal)
                .WithMany()
                .HasForeignKey(e => e.OpeningJournalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ClosingJournal)
                .WithMany()
                .HasForeignKey(e => e.ClosingJournalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }

    public class TaxConfiguration : IEntityTypeConfiguration<Tax>
    {
        public void Configure(EntityTypeBuilder<Tax> builder)
        {
            builder.HasOne(e => e.TaxAgency)
                .WithMany()
                .HasForeignKey(e => e.TaxAgencyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.SalesAccount)
                .WithMany()
                .HasForeignKey(e => e.SalesAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.PurchasesAccount)
                .WithMany()
                .HasForeignKey(e => e.PurchasesAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class TaxRateConfiguration : IEntityTypeConfiguration<TaxRate>
    {
        public void Configure(EntityTypeBuilder<TaxRate> builder)
        {
            builder.HasOne(e => e.Tax)
                .WithMany(e => e.TaxRates)
                .HasForeignKey(e => e.TaxId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.HasOne(e => e.LedgerAccount)
                .WithMany()
                .HasForeignKey(e => e.LedgerAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class NonRefundableItemConfiguration : IEntityTypeConfiguration<MemberAccountType>
    {
        public void Configure(EntityTypeBuilder<MemberAccountType> builder)
        {
            builder.HasOne(e => e.LedgerAccount)
                .WithMany()
                .HasForeignKey(e => e.LedgerAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
