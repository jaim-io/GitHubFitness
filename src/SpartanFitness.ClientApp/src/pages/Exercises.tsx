import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Exercise from "../types/Exercise";
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";

const EXERCISE_ENDPOINT = `${import.meta.env.VITE_API_BASE}/exercises`;

const ExercisesPage = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [exercises, setExercises] = useState<Exercise[]>();
  const [error, setError] = useState<Exception | null>(null);

  useEffect(() => {
    const fetchExercises = async () => {
      setIsLoading(true);
      setError(null);

      try {
        await axios
          .get<Exercise[]>(EXERCISE_ENDPOINT, {
            headers: {
              Accept: "application/json",
              Authorization: `bearer ${localStorage.getItem("token")}`,
            },
          })
          .then((res) => {
            setExercises(res.data);
          })
          .catch((err) => {
            toast.dismiss();
            toast.error(
              err.code == "ERR_NETWORK"
                ? "Unable to reach server"
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

    fetchExercises();
  }, []);

  return (
    <>
      <h1>The exercises page</h1>
      <ul>
        {exercises &&
          exercises.map((e) => (
            <li key={e.id}>
              <Link to={e.id}>{e.name}</Link>
            </li>
          ))}
      </ul>
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

export default ExercisesPage;
