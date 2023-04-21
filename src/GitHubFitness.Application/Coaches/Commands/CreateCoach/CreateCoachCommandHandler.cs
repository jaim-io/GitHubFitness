using ErrorOr;

using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Common.Errors;
using GitHubFitness.Domain.ValueObjects;

using MediatR;

namespace GitHubFitness.Application.Coaches.Commands.CreateCoach;

public class CreateCoachCommandHandler
    : IRequestHandler<CreateCoachCommand, ErrorOr<Coach>>
{
    private readonly IUserRepository _userResponsitory;
    private readonly ICoachRepository _coachRepository;
    private readonly IRoleRepository _roleRepository;

    public CreateCoachCommandHandler(
        IUserRepository userResponsitory,
        ICoachRepository coachRepository,
        IRoleRepository roleRepository)
    {
        _userResponsitory = userResponsitory;
        _coachRepository = coachRepository;
        _roleRepository = roleRepository;
    }

    public async Task<ErrorOr<Coach>> Handle(
        CreateCoachCommand command,
        CancellationToken cancellationToken)
    {
        var userId = UserId.Create(command.UserId);

        if (!await _userResponsitory.ExistsAsync(userId))
        {
            return Errors.User.NotFound;
        }

        if (await _coachRepository.GetByUserIdAsync(userId) is Coach)
        {
            return Errors.Coach.DuplicateUserId;
        }

        var coach = Coach.Create(
            userId);

        await _coachRepository.AddAsync(coach);

        return coach;
    }
}