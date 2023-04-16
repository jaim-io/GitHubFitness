using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Authentication.Queries.VerifyUserIsCoach;

public class VerifyUserIsCoachQueryHandler
  : IRequestHandler<VerifyUserIsCoachQuery, ErrorOr<Coach>>
{
  private readonly IUserRepository _userRepository;
  private readonly ICoachRepository _coachRepository;

  public VerifyUserIsCoachQueryHandler(
    IUserRepository userRepository,
    ICoachRepository coachRepository)
  {
    _userRepository = userRepository;
    _coachRepository = coachRepository;
  }

  public async Task<ErrorOr<Coach>> Handle(
    VerifyUserIsCoachQuery query,
    CancellationToken cancellationToken)
  {
    var userId = UserId.Create(query.UserId);
    var coachId = CoachId.Create(query.CoachId);

    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    if (await _coachRepository.GetByIdAsync(coachId) is not Coach coach)
    {
      return Errors.Coach.NotFound;
    }

    if (coach.UserId != user.Id) {
      return Errors.Authentication.UnAuthorized;
    }

    return coach;
  }
}