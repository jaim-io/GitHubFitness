using ErrorOr;

namespace SpartanFitness.Domain.Common.Errors;

public static partial class Errors
{
    public static class CoachApplication
    {
        public static Error IsClosed => Error.Conflict(
            code: "CoachApplication.IsClosed",
            description: "Coach application has already been closed");

        public static Error NotFound => Error.NotFound(
            code: "CoachApplication.NotFound",
            description: "Coach application not found");

        public static Error NotRelated => Error.Conflict(
            code: "CoachApplication.NotRelated",
            description: "User and coach application are not related");
        
        public static Error UserHasPendingApplication => Error.Conflict(
          code: "CoachApplication.UserHasPendingApplication",
          description: "User has a pending coach application.");
    }
}