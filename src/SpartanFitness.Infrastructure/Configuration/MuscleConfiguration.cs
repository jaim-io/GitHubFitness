using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Configuration;

public class MuscleConfiguration : IEntityTypeConfiguration<Muscle>
{
  public void Configure(EntityTypeBuilder<Muscle> builder)
  {
    ConfigureMusclesTable(builder);
  }

  private void ConfigureMusclesTable(EntityTypeBuilder<Muscle> builder)
  {
    builder.ToTable("Muscles");

    builder.HasKey(m => m.Id);

    builder.Property(m => m.Id)
      .HasConversion(
        id => id.Value,
        value => MuscleId.Create(value));

    builder.Property(m => m.MuscleGroupId)
      .HasConversion(
        id => id.Value,
        value => MuscleGroupId.Create(value));

    builder.Property(m => m.Description)
      .HasMaxLength(2048);

    builder.Property(m => m.Image)
      .HasMaxLength(2048);
  }
}