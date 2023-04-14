using Mapster;

using SpartanFitness.Application.Exercises.CreateExercise;
using SpartanFitness.Contracts.Exercises;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Api.Common.Mappings;

public class ExerciseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateExerciseRequest Request, string UserId), CreateExerciseCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<Exercise, ExerciseResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.CreatorId, src => src.CreatorId.Value)
            .Map(dest => dest.MuscleGroupIds, src => src.MuscleGroupIds.Select(muscleGroupId => muscleGroupId.Value))
            .Map(dest => dest, src => src);
    }
}