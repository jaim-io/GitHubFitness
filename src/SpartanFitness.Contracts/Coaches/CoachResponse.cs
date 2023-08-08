namespace SpartanFitness.Contracts.Coaches;

public record CoachResponse(
  string Id,
  string UserId,
  string FirstName,
  string LastName,
  string ProfileImage,
  string Email,
  DateTime CreatedDateTime,
  DateTime UpdatedDateTime);