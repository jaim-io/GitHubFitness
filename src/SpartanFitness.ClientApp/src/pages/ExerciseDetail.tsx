import { Link, useParams } from "react-router-dom";
import useExercise from "../hooks/useExercise";
import LoadingIcon from "../components/Icons/LoadingIcon";
import useMusclesByIds from "../hooks/useMusclesByIds";
import Muscle from "../types/domain/Muscle";
import MuscleGroup from "../types/domain/MuscleGroup";
import useMuscleGroupsByIds from "../hooks/useMuscleGroupsByIds";

const ExerciseDetailPage = () => {
  const params = useParams();

  const [exercise, , exerciseIsLoading] = useExercise(params.exerciseId!);

  let musclesAreLoading = false;
  let muscles: Muscle[] | undefined = undefined;
  if (exercise?.muscleIds != undefined && exercise?.muscleIds.length != 0) {
    [muscles, , musclesAreLoading] = useMusclesByIds(exercise.muscleIds);
  } else {
    muscles = [];
  }

  let muscleGroupsAreLoading = false;
  let muscleGroups: MuscleGroup[] | undefined = undefined;
  if (
    exercise?.muscleGroupIds != undefined &&
    exercise?.muscleGroupIds.length != 0
  ) {
    [muscleGroups, , muscleGroupsAreLoading] = useMuscleGroupsByIds(
      exercise.muscleGroupIds,
    );
  } else {
    muscleGroups = [];
  }

  return (
    <div className={"flex items-center"}>
      {exerciseIsLoading && <LoadingIcon />}

      {exercise && (
        <div className={"flex items-center"}>
          <div>
            <img src={exercise.image} alt={`${exercise.name} image`} />
          </div>
          <div>
            <h1>Exercise details</h1>
            <p>{exercise.name}</p>
            <p>{exercise.description}</p>

            {muscles && (
              <div>
                {muscles.length != 0 ? (
                  muscles.map((m) => <p key={m.id}>{m.name}</p>)
                ) : (
                  <p>No muscles specified</p>
                )}
              </div>
            )}
            {musclesAreLoading && <p>Muscles are loading</p>}

            {muscleGroups && (
              <div>
                {muscleGroups.length != 0 ? (
                  muscleGroups.map((mg) => <p key={mg.id}>{mg.name}</p>)
                ) : (
                  <p>No muscles groups specified</p>
                )}
              </div>
            )}
            {muscleGroupsAreLoading && <p>MuscleGroups are loading</p>}

            <Link to=".." relative="path">
              Back
            </Link>
          </div>
        </div>
      )}
    </div>
  );
};

export default ExerciseDetailPage;
