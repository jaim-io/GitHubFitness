using ErrorOr;

using MediatR;

using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Queries.GetAllSavedExerciseIds;

public record GetAllSavedExerciseIdsQuery(string UserId) : IRequest<ErrorOr<List<ExerciseId>>>;