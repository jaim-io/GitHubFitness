using ErrorOr;

using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Common.Errors;
using GitHubFitness.Domain.ValueObjects;

using MediatR;

namespace GitHubFitness.Application.MuscleGroups.Commands.CreateMuscleGroup;

public class CreateMuscleGroupCommandHandler
    : IRequestHandler<CreateMuscleGroupCommand, ErrorOr<MuscleGroup>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICoachRepository _coachRepository;
    private readonly IMuscleGroupRepository _muscleGroupRepository;

    public CreateMuscleGroupCommandHandler(
        IUserRepository userRepository,
        ICoachRepository coachRepository,
        IMuscleGroupRepository muscleGroupRepository)
    {
        _userRepository = userRepository;
        _coachRepository = coachRepository;
        _muscleGroupRepository = muscleGroupRepository;
    }

    public async Task<ErrorOr<MuscleGroup>> Handle(
        CreateMuscleGroupCommand command,
        CancellationToken cancellationToken)
    {
        var userId = UserId.Create(command.UserId);

        if (!await _userRepository.ExistsAsync(userId))
        {
            return Errors.User.NotFound;
        }

        if (await _coachRepository.GetByUserIdAsync(userId) is not Coach coach)
        {
            return Errors.Coach.NotFound;
        }

        var muscleGroup = MuscleGroup.Create(
            command.Name,
            command.Description,
            coach.Id,
            command.Image);

        await _muscleGroupRepository.AddAsync(muscleGroup);

        return muscleGroup;
    }
}