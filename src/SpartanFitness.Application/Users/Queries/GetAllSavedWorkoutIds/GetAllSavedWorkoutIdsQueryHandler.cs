using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Queries.GetAllSavedWorkoutIds;

public class GetAllSavedWorkoutIdsQueryHandler : IRequestHandler<GetAllSavedWorkoutIdsQuery, ErrorOr<List<WorkoutId>>>
{
  private readonly IUserRepository _userRepository;

  public GetAllSavedWorkoutIdsQueryHandler(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task<ErrorOr<List<WorkoutId>>> Handle(
    GetAllSavedWorkoutIdsQuery query,
    CancellationToken cancellationToken)
  {
    var userId = UserId.Create(query.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    return user.SavedWorkoutIds.ToList();
  }
}