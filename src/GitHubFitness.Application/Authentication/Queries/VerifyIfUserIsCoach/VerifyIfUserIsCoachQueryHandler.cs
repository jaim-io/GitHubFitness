using ErrorOr;

using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Common.Errors;
using GitHubFitness.Domain.ValueObjects;

using MediatR;

namespace GitHubFitness.Application.Authentication.Queries.VerifyIfUserIsCoach;

public class VerifyIfUserIsCoachQueryHandler
  : IRequestHandler<VerifyIfUserIsCoachQuery, ErrorOr<Coach>>
{
  private readonly IUserRepository _userRepository;
  private readonly ICoachRepository _coachRepository;

  public VerifyIfUserIsCoachQueryHandler(
    IUserRepository userRepository,
    ICoachRepository coachRepository)
  {
    _userRepository = userRepository;
    _coachRepository = coachRepository;
  }

  public async Task<ErrorOr<Coach>> Handle(
    VerifyIfUserIsCoachQuery query,
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