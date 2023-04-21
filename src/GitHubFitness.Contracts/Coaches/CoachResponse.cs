namespace GitHubFitness.Contracts.Coaches;

public record CoachResponse(
    string Id,
    string UserId,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime);