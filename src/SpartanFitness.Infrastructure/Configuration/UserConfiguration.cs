using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUserTable(builder);
        ConfigureRoleIdsTable(builder);
    }

    private void ConfigureRoleIdsTable(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(u => u.RoleIds, rib => {
            rib.ToTable("UserRoleIds");

            rib.WithOwner().HasForeignKey("UserId");

            rib.Property(ri => ri.Value)
                .HasColumnName("RoleId")
                .ValueGeneratedNever();

            rib.HasKey(ri => ri.Value);
            
            rib.HasKey("UserId");
        });

        builder.Metadata.FindNavigation(nameof(User.RoleIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureUserTable(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));

        builder.Property(u => u.FirstName)
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .HasMaxLength(100);

        builder.Property(u => u.ProfileImage)
            .HasMaxLength(250);

        builder.Property(u => u.Salt)
            .ValueGeneratedNever();
    }
}