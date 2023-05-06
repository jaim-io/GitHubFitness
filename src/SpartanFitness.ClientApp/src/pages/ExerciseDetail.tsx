import { Link, useParams } from "react-router-dom";
import useExercise from "../hooks/useExercise";

const ExerciseDetailPage = () => {
  const params = useParams();

  const [result] = useExercise(params.exerciseId!);
  const [, exercise] = result.extract();

  return (
    <>
      <h1>Exercise details</h1>
      {exercise && <p>{exercise.name}</p>}

      <Link to=".." relative="path">
        Back
      </Link>
    </>
  );
};

export default ExerciseDetailPage;
