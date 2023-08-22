using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IPasswordResetTokenRepository
{
  Task AddAsync(PasswordResetToken token);
  Task<List<PasswordResetToken>> GetByUserIdAsync(UserId id);
  Task UpdateAsync(PasswordResetToken token);
  Task InvalidateAllAsync();
}