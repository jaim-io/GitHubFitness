import axios from "axios";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import Page from "../types/domain/Page";
import Exception from "../types/domain/Exception";
import Exercise from "../types/domain/Exercise";
import SearchParamsFactory from "../utils/SearchParamsFactory";

const EXERCISE_ENDPOINT = `${import.meta.env.VITE_API_BASE}/exercises/page`;

type ApiResponse = Omit<Page<Exercise>, "values"> & { exercises: Exercise[] };

type PageArguments = {
  page?: number;
  size?: number;
  sort?: string;
  order?: string;
  query?: string;
};

const useExercisesPage = ({
  page,
  size,
  sort,
  order,
  query,
}: PageArguments): [
  Page<Exercise> | undefined,
  Exception | undefined,
  boolean,
] => {
  const [exercisesPage, setExercisePage] = useState<Page<Exercise>>();
  const [error, setError] = useState<Exception>();
  const [isLoading, setIsLoading] = useState(false);

  const queryString = SearchParamsFactory.CreateQueryString(
    page,
    size,
    sort,
    order,
    query,
  );

  useEffect(() => {
    const fetchExercises = async () => {
      setIsLoading(true);

      try {
        await axios
          .get<ApiResponse>(`${EXERCISE_ENDPOINT}${queryString}`, {
            headers: {
              Accept: "application/json",
              Authorization: `bearer ${localStorage.getItem("token")}`,
            },
          })
          .then((res) => {
            setExercisePage({ ...res.data, values: res.data.exercises });
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

    fetchExercises();
  }, [page, size, sort, order, query]);

  return [exercisesPage, error, isLoading];
};

export default useExercisesPage;
