namespace SpartanFitness.Contracts.Authentication;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string ProfileImage,
    string Email,
    string Password);