using ErrorOr;

using MediatR;

using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Queries.GetAllSavedWorkoutIds;

public record GetAllSavedWorkoutIdsQuery(string UserId) : IRequest<ErrorOr<List<WorkoutId>>>;