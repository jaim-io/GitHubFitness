using ErrorOr;

namespace SpartanFitness.Domain.Common.Errors;

public static partial class Errors
{
    public static class Administrator
    {
        public static Error DuplicateUserId => Error.Conflict(
            code: "Administrator.DuplicateUserId",
            description: "Administrator with given UserId already exists");

        public static Error NotFound => Error.NotFound(
            code: "Administrator.NotFound",
            description: "Administrator with given Id does not exist");
    }
}