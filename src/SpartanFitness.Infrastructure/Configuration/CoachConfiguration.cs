using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Configuration;

public class CoachConfiguration : IEntityTypeConfiguration<Coach>
{
  public void Configure(EntityTypeBuilder<Coach> builder)
  {
    ConfigureCoachesTable(builder);
  }

  private void ConfigureCoachesTable(EntityTypeBuilder<Coach> builder)
  {
    builder.ToTable("Coaches");

    builder.HasKey(c => c.Id);

    builder.Property(c => c.Id)
      .ValueGeneratedNever()
      .HasConversion(
        id => id.Value,
        value => CoachId.Create(value));

    builder.Property(c => c.UserId)
      .ValueGeneratedNever()
      .HasConversion(
        id => id.Value,
        value => UserId.Create(value));

    builder.Property(c => c.Biography)
      .HasMaxLength(2048);

    builder.OwnsOne(c => c.SocialMedia);
  }
}