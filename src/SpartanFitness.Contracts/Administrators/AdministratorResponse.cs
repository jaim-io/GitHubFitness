namespace SpartanFitness.Contracts.Administrators;

public record AdministratorResponse(
    string Id,
    string UserId,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime);