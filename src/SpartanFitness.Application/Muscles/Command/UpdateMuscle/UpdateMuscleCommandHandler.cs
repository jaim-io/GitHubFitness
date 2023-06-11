using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Muscles.Command.UpdateMuscle;

public class UpdateMuscleCommandHandler : IRequestHandler<UpdateMuscleCommand, ErrorOr<Muscle>>
{
  private readonly IMuscleRepository _muscleRepository;

  public UpdateMuscleCommandHandler(IMuscleRepository muscleRepository)
  {
    _muscleRepository = muscleRepository;
  }

  public async Task<ErrorOr<Muscle>> Handle(UpdateMuscleCommand command, CancellationToken cancellationToken)
  {
    var id = MuscleId.Create(command.Id);
    if (await _muscleRepository.GetByIdAsync(id) is not Muscle muscle)
    {
      return Errors.Muscle.NotFound;
    }

    muscle.SetName(command.Name);
    muscle.SetDescription(command.Description);
    muscle.SetImage(command.Image);
    muscle.SetUpdatedDateTime();

    await _muscleRepository.UpdateAsync(muscle);

    return muscle;
  }
}