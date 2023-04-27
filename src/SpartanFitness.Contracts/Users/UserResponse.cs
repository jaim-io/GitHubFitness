namespace SpartanFitness.Contracts.Users;

public record UserResponse(
  Guid Id,
  string FirstName,
  string LastName,
  string ProfileImage,
  string Email);