using ErrorOr;

namespace SpartanFitness.Domain.Common.Errors;

public static partial class Errors
{
    public static class Coach
    {
        public static Error DuplicateUserId => Error.Conflict(
            code: "Coach.DuplicateUserId",
            description: "Coach with given UserId already exists");
    }
}