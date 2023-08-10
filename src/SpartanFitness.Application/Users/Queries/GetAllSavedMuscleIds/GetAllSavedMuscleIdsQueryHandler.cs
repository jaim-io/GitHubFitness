using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Queries.GetAllSavedMuscleIds;

public class GetAllSavedMuscleIdsQueryHandler : IRequestHandler<GetAllSavedMuscleIdsQuery, ErrorOr<List<MuscleId>>>
{
  private readonly IUserRepository _userRepository;

  public GetAllSavedMuscleIdsQueryHandler(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task<ErrorOr<List<MuscleId>>> Handle(
    GetAllSavedMuscleIdsQuery query,
    CancellationToken cancellationToken)
  {
    var userId = UserId.Create(query.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    return user.SavedMuscleIds.ToList();
  }
}