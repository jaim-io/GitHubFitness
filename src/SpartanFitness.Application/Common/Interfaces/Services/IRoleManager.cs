using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Services;

public interface IRoleManager {
   Task<string[]> GetByUserIdAsync(UserId id);
}