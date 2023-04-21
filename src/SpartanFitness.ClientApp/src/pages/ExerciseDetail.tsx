import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import Exercise from "../types/Exercise";
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";

const EXERCISE_ENDPOINT = `${import.meta.env.VITE_API_BASE}/exercises`;

const ExerciseDetailPage = () => {
  const params = useParams();
  const [isLoading, setIsLoading] = useState(false);
  const [exercise, setExercise] = useState<Exercise>();
  const [error, setError] = useState<Exception | null>(null);

  useEffect(() => {
    const fetchExercise = async () => {
      setIsLoading(true);
      setError(null);

      try {
        await axios
          .get<Exercise>(`${EXERCISE_ENDPOINT}/${params.exerciseId}`, {
            headers: {
              Accept: "application/json",
              Authorization: `bearer ${localStorage.getItem("token")}`,
            },
          })
          .then((res) => {
            setExercise(res.data);
          })
          .catch((err) => {
            toast.dismiss();
            toast.error(
              err.code == "ERR_NETWORK"
                ? "Unable to reach the server"
                : err.response.statusText,
              {
                position: "bottom-right",
                autoClose: 5000,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
                theme: "colored",
              },
            );
            setError({
              message: err.response.statusText,
              code: err.response.status,
            });
          });
      } catch {}

      setIsLoading(false);
    };

    fetchExercise();
  }, []);

  return (
    <>
      <h1>Exercise details</h1>
      {exercise && <p>{exercise.name}</p>}
      <p>
        <Link to=".." relative="path">
          Back
        </Link>
      </p>
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
