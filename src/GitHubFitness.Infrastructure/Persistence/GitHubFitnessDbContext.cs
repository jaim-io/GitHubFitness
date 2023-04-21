using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Common.Interfaces;
using GitHubFitness.Domain.Common.Models;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace GitHubFitness.Infrastructure.Persistence;

public class GitHubFitnessDbContext : DbContext, IUnitOfWork
{
  private IMediator _mediator;

  public DbSet<User> Users { get; set; } = null!;
  public DbSet<Coach> Coaches { get; set; } = null!;
  public DbSet<Administrator> Administrators { get; set; } = null!;
  public DbSet<CoachApplication> CoachApplications { get; set; } = null!;
  public DbSet<MuscleGroup> MuscleGroups { get; set; } = null!;
  public DbSet<Exercise> Exercises { get; set; } = null!;
  public DbSet<Workout> Workouts { get; set; } = null!;

  public GitHubFitnessDbContext(DbContextOptions<GitHubFitnessDbContext> options, IMediator mediator)
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
      .ApplyConfigurationsFromAssembly(typeof(GitHubFitnessDbContext).Assembly);

    base.OnModelCreating(modelBuilder);
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)
    {
      optionsBuilder.UseSqlServer("Name=ConnectionStrings:GitHubFitness");
    }
  }
}