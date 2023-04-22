import axios from "axios";
import { useState, useEffect } from "react";
import { toast } from "react-toastify";
import Exercise from "../types/Exercise";
import { Result, inl, inr } from "../types/Result";
import Exception from "../types/Exception";

const EXERCISE_ENDPOINT = `${import.meta.env.VITE_API_BASE}/exercises`;

const useExercise = (id: string | undefined): Result<Exercise> => {
  const [exercise, setExercise] = useState<Exercise>();
  const [error, setError] = useState<Exception | null>(null);

  useEffect(() => {
    const fetchExercise = async () => {
      setError(null);

      try {
        await axios
          .get<Exercise>(`${EXERCISE_ENDPOINT}/${id}`, {
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
    };

    fetchExercise();
  }, []);

  return exercise == undefined
    ? inl<Exercise>()(error!)
    : inr<Exercise>()(exercise!);
};

export default useExercise;
