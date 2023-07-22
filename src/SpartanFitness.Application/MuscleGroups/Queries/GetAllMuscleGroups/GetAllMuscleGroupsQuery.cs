using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.MuscleGroups.Queries.GetAllMuscleGroups;

public record GetAllMuscleGroupsQuery() : IRequest<ErrorOr<List<MuscleGroup>>>;