using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBooks.Domains.Entities;

namespace SmartBooks.Persitence.Data.Config
{
    public class BinConfiguration : IEntityTypeConfiguration<Bin>
    {
        public void Configure(EntityTypeBuilder<Bin> builder)
        {
            builder.HasOne(e => e.Warehouse)
                .WithMany(e => e.Bins)
                .HasForeignKey(e => e.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Property<bool>("isDeleted");
            //builder.HasQueryFilter(m => EF.Property<bool>(m, "isDeleted") == false);
        }
    }

    public class BinCardConfiguration : IEntityTypeConfiguration<BinCard>
    {
        public void Configure(EntityTypeBuilder<BinCard> builder)
        {
            builder.HasOne(e => e.Bin)
                .WithMany()
                .HasForeignKey(e => e.BinId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.InventoryItem)
                .WithMany()
                .HasForeignKey(e => e.InventoryItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
    {
        public void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            builder.HasOne(e => e.UnitofMeasure)
                .WithMany()
                .HasForeignKey(e => e.UnitofMeasureId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.InventoryCategory)
                .WithMany()
                .HasForeignKey(e => e.InventoryCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }

    public class UomConversionConfiguration : IEntityTypeConfiguration<UomConversion>
    {
        public void Configure(EntityTypeBuilder<UomConversion> builder)
        {
            builder.HasOne(e => e.UnitofMeasureFrom)
                .WithMany()
                .HasForeignKey(e => e.UnitofMeasureFromId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.UnitofMeasureTo)
                .WithMany()
                .HasForeignKey(e => e.UnitofMeasureToId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class InventoryLedgerConfiguration : IEntityTypeConfiguration<InventoryLedger>
    {
        public void Configure(EntityTypeBuilder<InventoryLedger> builder)
        {
            builder.HasOne(e => e.BinCard)
                .WithMany()
                .HasForeignKey(e => e.BinCardId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.UnitofMeasure)
                .WithMany()
                .HasForeignKey(e => e.UnitofMeasureId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ReorderLevelConfiguration : IEntityTypeConfiguration<ReorderLevel>
    {
        public void Configure(EntityTypeBuilder<ReorderLevel> builder)
        {
            builder.HasOne(e => e.InventoryItem)
                .WithMany()
                .HasForeignKey(e => e.InventoryItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Bin)
                .WithMany()
                .HasForeignKey(e => e.BinId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.UnitofMeasure)
                .WithMany()
                .HasForeignKey(e => e.UnitofMeasureId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Warehouse)
                .WithMany()
                .HasForeignKey(e => e.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
