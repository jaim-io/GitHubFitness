using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Results;

namespace SpartanFitness.Application.Authentication.Commands.RequestPasswordReset;

public record RequestPasswordResetCommand(string UserId) : IRequest<ErrorOr<MessageResult>>;