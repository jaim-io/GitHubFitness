using ErrorOr;

using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Common.Errors;
using GitHubFitness.Domain.ValueObjects;

using MediatR;

namespace GitHubFitness.Application.CoachApplications.Commands.CreateCoachApplication;

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

        var coachApplication = CoachApplication.Create(
            command.Motivation,
            Status.Pending,
            userId);

        await _coachApplicationRepository.AddAsync(coachApplication);

        return coachApplication;
    }
}