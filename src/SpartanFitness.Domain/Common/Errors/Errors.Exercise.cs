using ErrorOr;

namespace SpartanFitness.Domain.Common.Errors;

public static partial class Errors
{
  public static class Exercise
  {
    public static Error NotFound => Error.NotFound(
      code: "Exercise.NotFound",
      description: "Exercise with given id does not exist");

    public static Error DuplicateName => Error.Conflict(
      code: "Exercise.DuplicateName",
      description: "Name already in use");
  }
}