using ErrorOr;

namespace SpartanFitness.Domain.Common.Errors;

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

    public static Error InvalidToken => Error.Validation(
      code: "Auth.InvalidToken",
      description: "Invalid token.");
  }
}