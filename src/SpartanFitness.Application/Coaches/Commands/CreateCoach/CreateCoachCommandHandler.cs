using ErrorOr;

using MediatR;

using SpartanFitness.Application.Coaches.Common;
using SpartanFitness.Application.Common.Interfaces.Authentication;
using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Coaches.Commands.CreateCoach;

public class CreateCoachCommandHandler
  : IRequestHandler<CreateCoachCommand, ErrorOr<CoachResult>>
{
  private readonly IUserRepository _userRepository;
  private readonly ICoachRepository _coachRepository;
  private readonly IRoleRepository _roleRepository;

  public CreateCoachCommandHandler(
    IUserRepository userRepository,
    ICoachRepository coachRepository,
    IRoleRepository roleRepository)
  {
    _userRepository = userRepository;
    _coachRepository = coachRepository;
    _roleRepository = roleRepository;
  }

  public async Task<ErrorOr<CoachResult>> Handle(
    CreateCoachCommand command,
    CancellationToken cancellationToken)
  {
    var userId = UserId.Create(command.UserId);

    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    if (await _coachRepository.GetByUserIdAsync(userId) is Coach)
    {
      return Errors.Coach.DuplicateUserId;
    }

    var socialMedia = SocialMedia.Create(
      linkedInUrl: command.LinkedInUrl,
      websiteUrl: command.WebsiteUrl,
      instagramUrl: command.InstagramUrl,
      facebookUrl: command.FacebookUrl);

    var coach = Coach.Create(
      userId: userId,
      biography: command.Biography,
      socialMedia: socialMedia);

    await _coachRepository.AddAsync(coach);

    return new CoachResult(user, coach);
  }
}