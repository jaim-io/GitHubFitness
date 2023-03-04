using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface ICoachApplicationRepository
{
    Task AddAsync(CoachApplication coachApplication);
}