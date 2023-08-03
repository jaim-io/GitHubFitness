using ErrorOr;

namespace SpartanFitness.Domain.Common.Errors;

public static partial class Errors
{
  public static class Authentication
  {
    public static Error InvalidCredentials => Error.Validation(
      code: "Auth.InvalidCredentials",
      description: "Invalid credentials.");

    public static Error InvalidToken => Error.Validation(
      code: "Auth.InvalidToken",
      description: "Invalid token.");

    public static Error InvalidParameters => Error.Validation(
      code: "Auth.InvalidParameters",
      description: "Invalid parameters.");

    public static Error EmailNotConfirmed => Error.Validation(
      code: "Auth.EmailNotConfirmed",
      description: "Email address has not been confirmed.");
  }
}