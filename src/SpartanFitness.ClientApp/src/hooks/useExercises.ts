import axios from "axios";
import { useState, useEffect } from "react";
import { toast } from "react-toastify";
import Exercise from "../types/Exercise";
import { Result, createException, createValue } from "../types/Result";
import Exception from "../types/Exception";
import Page from "../types/base/Page";

const EXERCISE_ENDPOINT = `${import.meta.env.VITE_API_BASE}/exercises`;

export type ExercisesPage = { exercises: Exercise[] } & Page;

const useExercises = (page: number, size: number): Result<ExercisesPage> => {
  const [exercisesPage, setExercisePage] = useState<ExercisesPage>();
  const [error, setError] = useState<Exception>();

  useEffect(() => {
    const fetchExercises = async () => {
      try {
        await axios
          .get<ExercisesPage>(`${EXERCISE_ENDPOINT}?page=${page}&size=${size}`, {
            headers: {
              Accept: "application/json",
              Authorization: `bearer ${localStorage.getItem("token")}`,
            },
          })
          .then((res) => {
            setExercisePage(res.data);
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
  }, [page, size]);

  return exercisesPage == undefined
    ? createException<ExercisesPage>()(error!)
    : createValue<ExercisesPage>()(exercisesPage!);
};

export default useExercises;
