using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Results;

namespace SpartanFitness.Application.Authentication.Commands.ForgotPassword;

public record ForgotPasswordCommand(string EmailAddress) : IRequest<ErrorOr<MessageResult>>;