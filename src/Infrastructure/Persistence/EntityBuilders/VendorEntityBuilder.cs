using Domain.Aggregates.VendorManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityBuilders;

public class VendorEntityBuilder : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.VendorGuid)
            .IsRequired();

        builder.HasIndex(x => x.VendorGuid)
            .IsUnique();
        
        builder.Property(x => x.UserName)
            .HasColumnType("char(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.UserName)
            .IsUnique();
        
        builder.Property(x => x.Password)
            .HasColumnType("char(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.ClientId)
            .HasColumnType("char(10)")
            .HasMaxLength(10)
            .IsRequired();

        builder.HasIndex(x => x.ClientId)
            .IsUnique();
        
        builder.Property(x => x.ClientSecret)
            .HasColumnType("char(50)")
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasIndex(x => x.ClientSecret)
            .IsUnique();
        
        builder.Property(x => x.VendorCode)
            .HasColumnType("char(10)")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.VendorId)
            .IsRequired(false);

        builder.HasIndex(x => x.VendorId)
            .IsUnique();
        
        builder.Property(x => x.UpdatedByUserId)
            .IsRequired();

        builder.Property(x => x.LastModifiedDate)
            .IsRequired();
        //     .HasDefaultValueSql("now()");
        //.ValueGeneratedOnAddOrUpdate();
    }
}