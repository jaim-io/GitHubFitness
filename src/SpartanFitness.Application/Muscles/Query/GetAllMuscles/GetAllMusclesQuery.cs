using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Muscles.Query.GetAllMuscles;

public record GetAllMusclesQuery() : IRequest<ErrorOr<List<Muscle>>>;