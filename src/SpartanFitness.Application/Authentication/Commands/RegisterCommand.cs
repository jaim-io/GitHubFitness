using ErrorOr;

using MediatR;

using SpartanFitness.Application.Authentication.Common;

namespace SpartanFitness.Application.Authentication.Commands;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string ProfileImage,
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;