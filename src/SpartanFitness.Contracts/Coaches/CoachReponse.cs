namespace SpartanFitness.Contracts.Coaches;

public record CoachReponse(
    string Id,
    string UserId,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime);