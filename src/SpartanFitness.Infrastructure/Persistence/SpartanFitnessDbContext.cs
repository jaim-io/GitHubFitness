using MediatR;

using Microsoft.EntityFrameworkCore;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Interfaces;
using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Infrastructure.Persistence;

public class SpartanFitnessDbContext : DbContext, IUnitOfWork
{
  private IMediator _mediator;

  public DbSet<User> Users { get; set; } = null!;
  public DbSet<Coach> Coaches { get; set; } = null!;
  public DbSet<Administrator> Administrators { get; set; } = null!;
  public DbSet<CoachApplication> CoachApplications { get; set; } = null!;
  public DbSet<MuscleGroup> MuscleGroups { get; set; } = null!;
  public DbSet<Exercise> Exercises { get; set; } = null!;
  public DbSet<Workout> Workouts { get; set; } = null!;
  public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

  public SpartanFitnessDbContext(DbContextOptions<SpartanFitnessDbContext> options, IMediator mediator)
    : base(options)
  {
    _mediator = mediator;
  }

  public async Task<bool> SaveEntitiesAsync<TId>(CancellationToken cancellationToken = default(CancellationToken))
    where TId : ValueObject
  {
    await _mediator.DispatchDomainEventsAsync<TId>(this);

    var result = await SaveChangesAsync(cancellationToken);

    return true;
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder
      .ApplyConfigurationsFromAssembly(typeof(SpartanFitnessDbContext).Assembly);

    base.OnModelCreating(modelBuilder);
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)
    {
      optionsBuilder.UseSqlServer("Name=ConnectionStrings:SpartanFitness");
    }
  }
}