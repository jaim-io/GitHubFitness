using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Http;

namespace SpartanFitness.Application.Users.Commands.UploadUserImage;

public record UploadUserImageCommand(string UserId, IFormFile File) : IRequest<ErrorOr<Unit>>;