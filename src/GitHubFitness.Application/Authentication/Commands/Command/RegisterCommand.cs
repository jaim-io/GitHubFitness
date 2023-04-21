using ErrorOr;

using GitHubFitness.Application.Authentication.Common;

using MediatR;

namespace GitHubFitness.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string ProfileImage,
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;