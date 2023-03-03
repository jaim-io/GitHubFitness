using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Configuration;

public class AdministratorConfiguration : IEntityTypeConfiguration<Administrator>
{
    public void Configure(EntityTypeBuilder<Administrator> builder)
    {
        ConfigureAdministratorTable(builder);
    }

    private void ConfigureAdministratorTable(EntityTypeBuilder<Administrator> builder)
    {
        builder.ToTable("Administrators");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => AdministratorId.Create(value));

        builder.Property(c => c.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));
    }
}