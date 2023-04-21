using ErrorOr;

namespace GitHubFitness.Domain.Common.Errors;

public static partial class Errors {
    public static class Exercise {
        public static Error NotFound => Error.NotFound(
            code: "Exercise.NotFound",
            description: "Exerices with given id does not exist");
    }
}