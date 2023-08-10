using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Queries.GetAllSavedExerciseIds;

public class
  GetAllSavedExerciseIdsQueryHandler : IRequestHandler<GetAllSavedExerciseIdsQuery, ErrorOr<List<ExerciseId>>>
{
  private readonly IUserRepository _userRepository;

  public GetAllSavedExerciseIdsQueryHandler(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task<ErrorOr<List<ExerciseId>>> Handle(
    GetAllSavedExerciseIdsQuery query,
    CancellationToken cancellationToken)
  {
    var userId = UserId.Create(query.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    return user.SavedExerciseIds.ToList();
  }
}