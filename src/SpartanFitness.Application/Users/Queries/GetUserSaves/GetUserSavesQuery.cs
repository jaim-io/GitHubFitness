using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Results;

namespace SpartanFitness.Application.Users.Queries.GetUserSaves;

public record GetUserSavesQuery(string UserId) : IRequest<ErrorOr<UserSavesResult>>;