using ErrorOr;

namespace GitHubFitness.Domain.Common.Errors;

public static partial class Errors
{
  public static class Authentication
  {
    public static Error InvalidCredentials => Error.Validation(
      code: "Auth.InvalidCredentials",
      description: "Invalid credentials.");

    public static Error UnAuthorized => Error.Validation(
      code: "Auth.Unauthorized",
      description: "Unauthorized.");
  }
}