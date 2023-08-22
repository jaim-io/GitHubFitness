using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Results;

namespace SpartanFitness.Application.Authentication.Commands.ResetPassword;

public record ResetPasswordCommand(
  string UserId, 
  string Token,
  string Password) : IRequest<ErrorOr<MessageResult>>;