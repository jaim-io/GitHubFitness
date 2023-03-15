using Mapster;

using SpartanFitness.Application.Exercises.Common;
using SpartanFitness.Application.Exercises.CreateExercise;
using SpartanFitness.Application.Exercises.Queries.GetExerciseById;
using SpartanFitness.Contracts.Exercises;

namespace SpartanFitness.Api.Common.Mappings;

public class ExerciseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateExerciseRequest Request, string UserId), CreateExerciseCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<GetExerciseRequest, GetExerciseByIdQuery>();

        config.NewConfig<ExerciseResult, ExerciseResponse>()
            .Map(dest => dest.Id, src => src.Exercise.Id.Value)
            .Map(dest => dest.CreatorId, src => src.Exercise.CreatorId.Value)
            .Map(dest => dest.MuscleGroupIds, src => src.Exercise.MuscleGroupIds.Select(muscleGroupId => muscleGroupId.Value))
            .Map(dest => dest, src => src.Exercise);
    }
}