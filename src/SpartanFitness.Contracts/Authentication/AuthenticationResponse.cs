namespace SpartanFitness.Contracts.Authentication;

public record AuthenticationResponse(
  string Id,
  string FirstName,
  string LastName,
  string ProfileImage,
  string Email,
  string Token,
  string RefreshToken);