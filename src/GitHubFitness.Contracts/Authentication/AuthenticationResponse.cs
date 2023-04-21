namespace GitHubFitness.Contracts.Authentication;

public record AuthenticationResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string ProfileImage,
    string Email,
    string Token);