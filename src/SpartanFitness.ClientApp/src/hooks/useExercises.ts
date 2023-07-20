import axios from "axios";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import Exception from "../types/domain/Exception";
import Exercise from "../types/domain/Exercise";

const EXERCISE_ENDPOINT = `${import.meta.env.VITE_API_BASE}/exercises`;

const useExercises = (): [
  Exercise[] | undefined,
  Exception | undefined,
  boolean,
] => {
  const [exercises, setExercises] = useState<Exercise[]>();
  const [error, setError] = useState<Exception>();
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const fetchExercisesAsync = async () => {
      setIsLoading(true);

      try {
        await axios
          .get<Exercise[]>(`${EXERCISE_ENDPOINT}`, {
            headers: {
              Accept: "application/json",
              Authorization: `bearer ${localStorage.getItem("token")}`,
            },
          })
          .then((res) => {
            setExercises(res.data);
            setIsLoading(false);
          })
          .catch((err) => {
            setIsLoading(false);
            setError({
              message: err.message,
              code: err.code,
            });
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
          });
      } catch {
        /* empty */
      }
    };

    fetchExercisesAsync();
  }, []);

  return [exercises, error, isLoading];
};

export default useExercises;
