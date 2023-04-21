using ErrorOr;

namespace GitHubFitness.Domain.Common.Errors;

public static partial class Errors
{
  public static class Workout
  {
    public static Error NotFound => Error.NotFound(
      code: "Workout.NotFound",
      description: "Workout not found");
  }
}