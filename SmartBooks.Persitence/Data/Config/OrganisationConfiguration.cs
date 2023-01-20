using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBooks.Domains.Entities;

namespace SmartBooks.Persitence.Data.Config
{
    public class SubLedgerBaseConfiguration : IEntityTypeConfiguration<SubLedgerBase>
    {
        public void Configure(EntityTypeBuilder<SubLedgerBase> builder)
        {
            builder.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.LedgerAccount)
                .WithMany()
                .HasForeignKey(e => e.LedgerAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
    public class OrganisationConfiguration : IEntityTypeConfiguration<Organisation>
    {
        public void Configure(EntityTypeBuilder<Organisation> builder)
        {
            builder.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(e => e.DefaultAddress)
                .WithMany()
                .HasForeignKey(e => e.DefaultAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(e => e.PaymentTerm)
                .WithMany()
                .HasForeignKey(e => e.PaymentTermId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }

    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasOne(e => e.Bank)
                .WithMany()
                .HasForeignKey(e => e.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(e => e.BankBranch)
                .WithMany()
                .HasForeignKey(e => e.BankBranchId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }

    public class CustomerProjectConfiguration : IEntityTypeConfiguration<CustomerProject>
    {
        public void Configure(EntityTypeBuilder<CustomerProject> builder)
        {
            builder.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class OrganisationAddressConfiguration : IEntityTypeConfiguration<OrganisationAddress>
    {
        public void Configure(EntityTypeBuilder<OrganisationAddress> builder)
        {
            builder.HasOne(e => e.Organisation)
                .WithMany(e => e.OrganisationAddresses)
                .HasForeignKey(e => e.OrganisationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
