import { Link, useParams } from "react-router-dom";
import { ToastContainer, toast } from "react-toastify";
import useExercise from "../hooks/useExercise";

const ExerciseDetailPage = () => {
  const params = useParams();

  const result = useExercise(params.exerciseId);
  const [_, exercise] = result.extract();

  return (
    <>
      <h1>Exercise details</h1>
      {exercise && <p>{exercise.name}</p>}

      <Link to=".." relative="path">
        Back
      </Link>

      <ToastContainer
        position="bottom-right"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
        theme="colored"
      />
    </>
  );
};

export default ExerciseDetailPage;
