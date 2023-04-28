namespace SpartanFitness.Contracts.Authentication;

public record RefreshTokenRequest(
  string Token,
  string RefreshToken);