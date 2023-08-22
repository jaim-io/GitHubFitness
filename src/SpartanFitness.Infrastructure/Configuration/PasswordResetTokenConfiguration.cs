using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Configuration;

public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
  public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
  {
    ConfigurePasswordResetTokensTable(builder);
  }

  private void ConfigurePasswordResetTokensTable(EntityTypeBuilder<PasswordResetToken> builder)
  {
    builder.ToTable("PasswordResetTokens");

    builder.HasKey(prt => prt.Id);

    builder.Property(prt => prt.Value)
      .ValueGeneratedNever();

    builder.Property(prt => prt.Id)
      .ValueGeneratedNever()
      .HasConversion(
        id => id.Value,
        value => PasswordResetTokenId.Create(value));

    builder.Property(rt => rt.UserId)
      .ValueGeneratedNever()
      .HasConversion(
        id => id.Value,
        value => UserId.Create(value));
  }
}