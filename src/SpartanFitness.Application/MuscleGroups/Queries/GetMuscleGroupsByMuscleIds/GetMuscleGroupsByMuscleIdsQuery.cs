using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupsByMuscleIds;

public record GetMuscleGroupsByMuscleIdsQuery(List<string> MuscleIds) : IRequest<ErrorOr<List<MuscleGroup>>>;