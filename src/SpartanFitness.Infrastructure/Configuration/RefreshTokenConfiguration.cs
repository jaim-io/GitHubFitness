using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Configuration;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
  public void Configure(EntityTypeBuilder<RefreshToken> builder)
  {
    ConfigureRefreshTokensTable(builder);
  }

  private void ConfigureRefreshTokensTable(EntityTypeBuilder<RefreshToken> builder)
  {
    builder.ToTable("RefreshTokens");

    builder.HasKey(rt => rt.Id);

    builder.Property(rt => rt.Id)
      .ValueGeneratedNever()
      .HasConversion(
        id => id.Value,
        value => RefreshTokenId.Create(value));

    builder.Property(rt => rt.UserId)
      .ValueGeneratedNever()
      .HasConversion(
        id => id.Value,
        value => UserId.Create(value));
  }
}