using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Infrastructure.Persistence;

public class SpartanFitnessDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Coach> Coaches { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public SpartanFitnessDbContext(DbContextOptions<SpartanFitnessDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(SpartanFitnessDbContext).Assembly);

        modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties())
            .Where(p => p.IsPrimaryKey())
            .ToList()
            .ForEach(p => p.ValueGenerated = ValueGenerated.Never);

        modelBuilder.Entity<Role>().HasData(
            Role.Create("User", "A simple user, with limited rights."),
            Role.Create("Coach", "A user which is able to create and remove MuscleGroups, Exercises and Workouts."),
            Role.Create("Admin", "A user with full access to the API."));

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