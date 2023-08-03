namespace SpartanFitness.Application.Common.Interfaces.Authentication;

public interface IEmailConfirmationTokenProvider
{
  string GenerateToken(string emailAddress);
  bool ValidateToken(string token, string email);
}