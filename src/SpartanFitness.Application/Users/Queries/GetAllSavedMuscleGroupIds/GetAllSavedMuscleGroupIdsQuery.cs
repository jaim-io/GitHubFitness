using ErrorOr;

using MediatR;

using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Queries.GetAllSavedMuscleGroupIds;

public record GetAllSavedMuscleGroupIdsQuery(string UserId) : IRequest<ErrorOr<List<MuscleGroupId>>>;