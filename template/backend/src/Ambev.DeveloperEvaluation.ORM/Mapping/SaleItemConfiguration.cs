using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(si => si.Id);
        builder.Property(si => si.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(si => si.SaleId).IsRequired();
        builder.Property(si => si.ProductId).IsRequired().HasMaxLength(50);
        builder.Property(si => si.ProductName).IsRequired().HasMaxLength(200);
        builder.Property(si => si.Quantity).IsRequired();
        builder.Property(si => si.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(si => si.Discount).IsRequired().HasColumnType("decimal(5,2)");
        builder.Property(si => si.IsCancelled).IsRequired().HasDefaultValue(false);

        // Configure the many-to-one relationship with Sale
        builder.HasOne(si => si.Sale)
            .WithMany(s => s.Items)
            .HasForeignKey(si => si.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Create index on SaleId for performance
        builder.HasIndex(si => si.SaleId);
    }
} 