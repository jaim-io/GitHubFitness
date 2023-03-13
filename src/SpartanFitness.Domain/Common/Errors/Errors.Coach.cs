using ErrorOr;

namespace SpartanFitness.Domain.Common.Errors;

public static partial class Errors
{
    public static class Coach
    {
        public static Error DuplicateUserId => Error.Conflict(
            code: "Coach.DuplicateUserId",
            description: "Coach with given UserId already exists");

        public static Error NotFound => Error.NotFound(
            code: "Coach.NotFound",
            description: "Coach with given UserId/CoachId does not exist");
    }
}