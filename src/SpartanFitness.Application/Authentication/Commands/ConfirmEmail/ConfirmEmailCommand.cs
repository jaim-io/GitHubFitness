using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Results;

namespace SpartanFitness.Application.Authentication.Commands.ConfirmEmail;

public record ConfirmEmailCommand(string UserId, string Token) : IRequest<ErrorOr<MessageResult>>;