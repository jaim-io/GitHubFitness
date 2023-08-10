using ErrorOr;

using MediatR;

using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Queries.GetAllSavedMuscleIds;

public record GetAllSavedMuscleIdsQuery(string UserId) : IRequest<ErrorOr<List<MuscleId>>>;