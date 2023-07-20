using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Workouts.Queries.GetAllWorkouts;

public record GetAllWorkoutsQuery() : IRequest<ErrorOr<List<Workout>>>;