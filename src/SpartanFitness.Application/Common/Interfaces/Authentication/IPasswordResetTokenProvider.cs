using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Common.Interfaces.Authentication;

public interface IPasswordResetTokenProvider
{
  PasswordResetToken GenerateToken(User user);
  bool ValidateToken(string tokenValue, User user);
}