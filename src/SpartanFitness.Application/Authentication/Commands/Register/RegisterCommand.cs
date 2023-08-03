using ErrorOr;

using MediatR;

using SpartanFitness.Application.Authentication.Common;
using SpartanFitness.Application.Common.Results;

namespace SpartanFitness.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string ProfileImage,
    string Email,
    string Password) : IRequest<ErrorOr<MessageResult>>;