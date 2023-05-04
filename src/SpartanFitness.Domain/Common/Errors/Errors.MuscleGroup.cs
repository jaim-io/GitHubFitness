using ErrorOr;

namespace SpartanFitness.Domain.Common.Errors;

public static partial class Errors
{
  public static class MuscleGroup
  {
    public static Error NotFound => Error.NotFound(
      code: "MuscleGroup.NotFound",
      description: "MuscleGroup with given id does not exist");

    public static Error DuplicateName => Error.Conflict(
      code: "MuscleGroup.DuplicateName",
      description: "Name already in use");
  }
}