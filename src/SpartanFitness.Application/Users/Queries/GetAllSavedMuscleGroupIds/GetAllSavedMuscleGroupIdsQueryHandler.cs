using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Queries.GetAllSavedMuscleGroupIds;

public class
  GetAllSavedMuscleGroupIdsQueryHandler : IRequestHandler<GetAllSavedMuscleGroupIdsQuery, ErrorOr<List<MuscleGroupId>>>
{
  private readonly IUserRepository _userRepository;

  public GetAllSavedMuscleGroupIdsQueryHandler(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task<ErrorOr<List<MuscleGroupId>>> Handle(
    GetAllSavedMuscleGroupIdsQuery query,
    CancellationToken cancellationToken)
  {
    var userId = UserId.Create(query.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    return user.SavedMuscleGroupIds.ToList();
  }
}