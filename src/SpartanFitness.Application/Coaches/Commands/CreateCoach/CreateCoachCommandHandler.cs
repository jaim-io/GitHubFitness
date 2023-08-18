using ErrorOr;

using MediatR;

using SpartanFitness.Application.Coaches.Common;
using SpartanFitness.Application.Common.Interfaces.Authentication;
using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Application.Common.Interfaces.Services;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Coaches.Commands.CreateCoach;

public class CreateCoachCommandHandler
  : IRequestHandler<CreateCoachCommand, ErrorOr<CoachResult>>
{
  private readonly IUserRepository _userRepository;
  private readonly ICoachRepository _coachRepository;
  private readonly ICoachCreationTokenProvider _creationTokenProvider;

  public CreateCoachCommandHandler(
    IUserRepository userRepository,
    ICoachRepository coachRepository,
    ICoachCreationTokenProvider creationTokenProvider)
  {
    _userRepository = userRepository;
    _coachRepository = coachRepository;
    _creationTokenProvider = creationTokenProvider;
  }

  public async Task<ErrorOr<CoachResult>> Handle(
    CreateCoachCommand command,
    CancellationToken cancellationToken)
  {
    var userId = UserId.Create(command.UserId);

    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.Authentication.InvalidParameters;
    }

    var isValid = _creationTokenProvider.ValidateToken(token: command.Token, email: user.Email);
    if (!isValid)
    {
      return Errors.Authentication.InvalidParameters;
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