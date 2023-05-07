import axios from "axios";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import Page from "../types/domain/Page";
import Exception from "../types/domain/Exception";
import MuscleGroup from "../types/domain/MuscleGroup";
import SearchParamsFactory from "../types/SearchParamsFactory";

const MUSCLEGROUP_ENDPOINT = `${
  import.meta.env.VITE_API_BASE
}/muscle-groups/page`;

export type MuscleGroupPage = { muscleGroups: MuscleGroup[] } & Page;

const useMuscleGroupsPage = (
  page?: number,
  size?: number,
  sort?: string,
  order?: string,
  query?: string,
): [MuscleGroupPage | undefined, Exception | undefined, boolean] => {
  const [muscleGroupPage, setMuscleGroupPage] = useState<MuscleGroupPage>();
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
          .get<MuscleGroupPage>(`${MUSCLEGROUP_ENDPOINT}${queryString}`, {
            headers: {
              Accept: "application/json",
              Authorization: `bearer ${localStorage.getItem("token")}`,
            },
          })
          .then((res) => {
            setMuscleGroupPage(res.data);
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

  return [muscleGroupPage, error, isLoading];
};

export default useMuscleGroupsPage;
