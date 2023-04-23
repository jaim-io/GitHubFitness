import axios from "axios";
import { useState, useEffect } from "react";
import { toast } from "react-toastify";
import Exercise from "../types/Exercise";
import { Result, createException, createValue } from "../types/Result";
import Exception from "../types/Exception";

const EXERCISE_ENDPOINT = `${import.meta.env.VITE_API_BASE}/exercises`;

const useExercises = (): Result<Exercise[]> => {
  const [exercises, setExercises] = useState<Exercise[]>();
  const [error, setError] = useState<Exception>();

  useEffect(() => {
    const fetchExercises = async () => {

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
    };

    fetchExercises();
  }, []);

  return exercises == undefined
    ? createException<Exercise[]>()(error!)
    : createValue<Exercise[]>()(exercises!);
};

export default useExercises;
