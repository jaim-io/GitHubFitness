namespace SpartanFitness.Contracts.Coaches;

public record CoachResponse(
  string Id,
  string FirstName,
  string LastName,
  string ProfileImage,
  string Email,
  DateTime CreatedDateTime,
  DateTime UpdatedDateTime);