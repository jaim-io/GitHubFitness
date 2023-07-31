using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Muscles.Command.CreateMuscle;

public class CreateMuscleCommandHandler : IRequestHandler<CreateMuscleCommand, ErrorOr<Muscle>>
{
  private readonly IMuscleRepository _muscleRepository;

  public CreateMuscleCommandHandler(IMuscleRepository muscleRepository)
  {
    _muscleRepository = muscleRepository;
  }

  public async Task<ErrorOr<Muscle>> Handle(CreateMuscleCommand command, CancellationToken cancellationToken)
  {
    if (await _muscleRepository.GetByNameAsync(command.Name) is Muscle)
    {
      return Errors.Muscle.DuplicateName;
    }

    var muscleGroupId = MuscleGroupId.Create(command.MuscleGroupId);

    var muscle = Muscle.Create(
      command.Name,
      command.Description,
      command.Image);

    await _muscleRepository.AddAsync(muscle, muscleGroupId);

    return muscle;
  }
}