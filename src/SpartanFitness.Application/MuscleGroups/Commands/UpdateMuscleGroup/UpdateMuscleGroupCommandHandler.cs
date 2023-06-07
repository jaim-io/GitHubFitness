using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.MuscleGroups.Commands.UpdateMuscleGroup;

public class UpdateMuscleGroupCommandHandler : IRequestHandler<UpdateMuscleGroupCommand, ErrorOr<MuscleGroup>>
{
  private readonly IMuscleGroupRepository _muscleGroupRepository;

  public UpdateMuscleGroupCommandHandler(IMuscleGroupRepository muscleGroupRepository)
  {
    _muscleGroupRepository = muscleGroupRepository;
  }

  public async Task<ErrorOr<MuscleGroup>> Handle(UpdateMuscleGroupCommand command, CancellationToken cancellationToken)
  {
    var muscleGroupId = MuscleGroupId.Create(command.Id);
    if (await _muscleGroupRepository.GetByIdAsync(muscleGroupId) is not MuscleGroup muscleGroup)
    {
      return Errors.MuscleGroup.NotFound;
    }

    var muscleIds = command.MuscleIds?.ConvertAll(MuscleId.Create);

    muscleGroup.SetName(command.Name);
    muscleGroup.SetDescription(command.Description);
    muscleGroup.SetMuscleIds(muscleIds ?? new());
    muscleGroup.SetImage(command.Image);

    await _muscleGroupRepository.UpdateAsync(muscleGroup);

    return muscleGroup;
  }
}