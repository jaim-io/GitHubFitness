using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Authentication;
using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Coaches.Commands.CreateCoach;

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