namespace GitHubFitness.Contracts.CoachApplications;

public record CoachApplicationResponse(
    string Id,
    string Motivation,
    string Remarks,
    string Status,
    string UserId,
    DateTime CreatedDateTime,
    DateTime ClosedDateTime);