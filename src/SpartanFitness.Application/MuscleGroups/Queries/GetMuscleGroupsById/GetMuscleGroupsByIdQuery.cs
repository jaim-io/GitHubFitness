using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupsById;

public record GetMuscleGroupsByIdQuery(List<string> Ids) : IRequest<ErrorOr<List<MuscleGroup>>>;