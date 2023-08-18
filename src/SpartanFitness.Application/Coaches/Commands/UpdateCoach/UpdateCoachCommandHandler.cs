using ErrorOr;

using MediatR;

using SpartanFitness.Application.Coaches.Common;
using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Coaches.Commands.UpdateCoach;

public class UpdateCoachCommandHandler : IRequestHandler<UpdateCoachCommand, ErrorOr<CoachResult>>
{
  private readonly ICoachRepository _coachRepository;
  private readonly IUserRepository _userRepository;

  public UpdateCoachCommandHandler(ICoachRepository coachRepository, IUserRepository userRepository)
  {
    _coachRepository = coachRepository;
    _userRepository = userRepository;
  }

  public async Task<ErrorOr<CoachResult>> Handle(UpdateCoachCommand command, CancellationToken cancellationToken)
  {
    var coachId = CoachId.Create(command.CoachId);
    if (await _coachRepository.GetByIdAsync(coachId) is not Coach coach)
    {
      return Errors.Coach.NotFound;
    }

    if (await _userRepository.GetByCoachIdAsync(coachId) is not User user)
    {
      // Should never happen, log error
      return Errors.User.NotFound;
    }

    var socialMedia = SocialMedia.Create(
      linkedInUrl: command.LinkedInUrl,
      websiteUrl: command.WebsiteUrl,
      instagramUrl: command.InstagramUrl,
      facebookUrl: command.FacebookUrl);

    coach.SetBiography(command.Biography);
    coach.SetSocialMedia(socialMedia);

    await _coachRepository.UpdateAsync(coach);

    return new CoachResult(user, coach);
  }
}