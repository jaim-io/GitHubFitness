using ErrorOr;

using MediatR;

using SpartanFitness.Application.Authentication.Common;

namespace SpartanFitness.Application.Authentication.Queries;

public record LoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;