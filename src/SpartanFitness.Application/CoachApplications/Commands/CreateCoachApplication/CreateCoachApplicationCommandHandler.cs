using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.CoachApplications.Commands.CreateCoachApplication;

public class CreateCoachApplicationCommandHandler
  : IRequestHandler<CreateCoachApplicationCommand, ErrorOr<CoachApplication>>
{
  private readonly IUserRepository _userRepository;
  private readonly ICoachRepository _coachRepository;
  private readonly ICoachApplicationRepository _coachApplicationRepository;

  public CreateCoachApplicationCommandHandler(
    IUserRepository userRepository,
    ICoachRepository coachRepository,
    ICoachApplicationRepository coachApplicationRepository)
  {
    _userRepository = userRepository;
    _coachRepository = coachRepository;
    _coachApplicationRepository = coachApplicationRepository;
  }

  public async Task<ErrorOr<CoachApplication>> Handle(
    CreateCoachApplicationCommand command,
    CancellationToken cancellationToken)
  {
    var userId = UserId.Create(command.UserId);

    if (!await _userRepository.ExistsAsync(userId))
    {
      return Errors.User.NotFound;
    }

    if (await _coachRepository.GetByUserIdAsync(userId) is Coach)
    {
      return Errors.Coach.DuplicateUserId;
    }

    if (await _coachApplicationRepository.HasPendingApplications(userId))
    {
      return Errors.CoachApplication.UserHasPendingApplication;
    }

    var coachApplication = CoachApplication.Create(
      command.Motivation,
      Status.Pending,
      userId);

    await _coachApplicationRepository.AddAsync(coachApplication);

    return coachApplication;
  }
}