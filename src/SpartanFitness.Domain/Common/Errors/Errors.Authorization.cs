using ErrorOr;

namespace SpartanFitness.Domain.Common.Errors;

public static partial class Errors
{
  public static class Authorization
  {
    public static Error UnAuthorized => Error.Validation(
      code: "Auth.Unauthorized",
      description: "Unauthorized.");
  }
}