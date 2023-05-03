using ErrorOr;

namespace SpartanFitness.Domain.Common.Errors;

public static partial class Errors
{
  public static class Muscle
  {
    public static Error DuplicateName => Error.Conflict(
      code: "Muscle.DuplicateName",
      description: "Name already in use");

    public static Error NotFound => Error.NotFound(
      code: "Muscle.NotFound",
      description: "Muscle not found");
  }
}