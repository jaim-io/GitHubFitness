using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface ICoachRepository
{
  Task<Coach?> GetByIdAsync(CoachId id);
  Task<List<Coach>> GetByIdAsync(List<CoachId> ids);
  Task<Coach?> GetByUserIdAsync(UserId id);
  Task AddAsync(Coach coach);
  Task UpdateAsync(Coach coach);
}