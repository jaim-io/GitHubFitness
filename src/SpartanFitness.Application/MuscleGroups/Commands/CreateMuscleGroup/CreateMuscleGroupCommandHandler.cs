using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.MuscleGroups.Commands.CreateMuscleGroup;

public class CreateMuscleGroupCommandHandler
    : IRequestHandler<CreateMuscleGroupCommand, ErrorOr<MuscleGroup>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMuscleGroupRepository _muscleGroupRepository;

    public CreateMuscleGroupCommandHandler(
        IUserRepository userRepository,
        IMuscleGroupRepository muscleGroupRepository)
    {
        _userRepository = userRepository;
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

        var muscleGroup = MuscleGroup.Create(
            command.Name,
            command.Description,
            command.Image);

        await _muscleGroupRepository.AddAsync(muscleGroup);

        return muscleGroup;
    }
}