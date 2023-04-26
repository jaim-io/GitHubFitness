import { Link, useParams } from "react-router-dom";
import { ToastContainer, toast } from "react-toastify";
import useExercise from "../hooks/useExercise";

const ExerciseDetailPage = () => {
  const params = useParams();

  const [result, isLoading] = useExercise(params.exerciseId);
  const [_, exercise] = result.extract();

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
