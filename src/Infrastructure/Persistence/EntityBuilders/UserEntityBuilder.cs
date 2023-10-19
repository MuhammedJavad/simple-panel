using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Aggregates.UserManagement.Entities;
using Domain.Aggregates.UserManagement;
using Domain.Aggregates.UserManagement.ValueTypes;
using Microsoft.EntityFrameworkCore;

namespace Persistence.EntityBuilders;

class UserEntityBuilder : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User));
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Email)
            .HasMaxLength(250)
            .IsRequired();

        builder
            .Property(x => x.HashedPassword)
            .IsRequired();

        builder
            .Property(x => x.FullName)
            .HasMaxLength(250);

        builder
            .Property(x => x.Policy)
            .HasMaxLength(600)
            .HasConversion<string>(
                x => x.ToString(),
                x => (Policy)x)
            .IsRequired();
        
        Seed(builder);
    }

    private static void Seed(EntityTypeBuilder<User> builder)
    {
        var aggregate = UserAggregate.RegisterUser("Test Testy", "test@snappgrocery.com", "1234");
        builder.HasData(aggregate.User);
    }
}