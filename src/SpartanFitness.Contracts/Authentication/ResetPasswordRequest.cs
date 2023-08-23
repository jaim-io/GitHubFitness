namespace SpartanFitness.Contracts.Authentication;

public record ResetPasswordRequest(
  string UserId,
  string Token,
  string Password);