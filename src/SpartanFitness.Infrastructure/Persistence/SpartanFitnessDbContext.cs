using Microsoft.EntityFrameworkCore;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Infrastructure.Persistence.Interceptors;

namespace SpartanFitness.Infrastructure.Persistence;

public class SpartanFitnessDbContext : DbContext
{
  private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

  public DbSet<User> Users { get; set; } = null!;
  public DbSet<Coach> Coaches { get; set; } = null!;
  public DbSet<Administrator> Administrators { get; set; } = null!;
  public DbSet<CoachApplication> CoachApplications { get; set; } = null!;
  public DbSet<MuscleGroup> MuscleGroups { get; set; } = null!;
  public DbSet<Exercise> Exercises { get; set; } = null!;
  public DbSet<Workout> Workouts { get; set; } = null!;
  public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
  public DbSet<PasswordResetToken> PasswordResetTokens { get; set; } = null!;
  public DbSet<Muscle> Muscles { get; set; } = null!;

  public SpartanFitnessDbContext(
    DbContextOptions<SpartanFitnessDbContext> options,
    PublishDomainEventsInterceptor publishDomainEventsInterceptor)
    : base(options)
  {
    _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder
      .Ignore<List<IDomainEvent>>()
      .ApplyConfigurationsFromAssembly(typeof(SpartanFitnessDbContext).Assembly);

    base.OnModelCreating(modelBuilder);
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);

    if (!optionsBuilder.IsConfigured)
    {
      optionsBuilder.UseSqlServer("Name=ConnectionStrings:SpartanFitness");
    }

    base.OnConfiguring(optionsBuilder);
  }
}