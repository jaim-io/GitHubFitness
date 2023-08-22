using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Common.Interfaces.Authentication;

public interface IPasswordResetTokenProvider
{
  PasswordResetToken GenerateToken(User user);
  bool ValidateToken(PasswordResetToken token, User user);
}