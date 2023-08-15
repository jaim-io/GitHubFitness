namespace SpartanFitness.Contracts.Users;

public record UpdateUserRequest(
  string Id,
  string FirstName,
  string LastName,
  string ProfileImage);