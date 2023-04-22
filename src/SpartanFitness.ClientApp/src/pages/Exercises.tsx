import { useEffect, useState } from "react";
import Exercise from "../types/Exercise";
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";
import ExerciseCard from "../components/ExerciseCard";
import { Link } from "react-router-dom";
import { Listbox } from "@headlessui/react";

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
            toast.error(
              err.code == "ERR_NETWORK"
                ? "Unable to reach the server"
                : err.response.statusText,
              {
                toastId: err.code,
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
    <div className="px-24 pt-6">
      <Link
        to=""
        className="text-xl font-semibold text-[#2f81f7] hover:underline hover:underline-[#2f81f7] max-w-[8rem]"
      >
        All exercises
      </Link>

      <ul className="flex justify-center gap-4">
        <p>Search-balk</p>

        <p>sort-by-date</p>

        {/* <Listbox value={}> 
        <Listbox.Button></Listbox.Button>
        </Listbox> */}

        <p>new</p>
      </ul>

      <ul className="flex flex-wrap gap-4 justify-center mb-4 pt-6">
        {exercises &&
          exercises.map((e) => <ExerciseCard exercise={e} key={e.id} />)}
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
    </div>
  );
};

export default ExercisesPage;
