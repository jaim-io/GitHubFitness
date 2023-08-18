namespace SpartanFitness.Application.Common.Interfaces.Authentication;

public interface ICoachCreationTokenProvider
{
  string GenerateToken(string emailAddress);
  bool ValidateToken(string token, string email);
}