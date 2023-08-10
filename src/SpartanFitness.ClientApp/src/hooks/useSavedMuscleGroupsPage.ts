import axios from "axios";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import Exception from "../types/domain/Exception";
import MuscleGroup from "../types/domain/MuscleGroup";
import Page from "../types/domain/Page";
import SearchParamsFactory from "../utils/SearchParamsFactory";

const USER_ENDPOINT = `${import.meta.env.VITE_API_BASE}/users`;

type ApiResponse = Omit<Page<MuscleGroup>, "values"> & {
  muscleGroups: MuscleGroup[];
};

type PageArguments = {
  userId: string;
  forceRefreshValue?: boolean;
  page?: number;
  size?: number;
  sort?: string;
  order?: string;
  query?: string;
};

const useSavedMuscleGroupsPage = ({
  userId,
  forceRefreshValue,
  page,
  size,
  sort,
  order,
  query,
}: PageArguments): [
  Page<MuscleGroup> | undefined,
  Exception | undefined,
  boolean,
] => {
  const [muscleGroupsPage, setMuscleGroupsPage] = useState<Page<MuscleGroup>>();
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
    const fetchMuscleGroups = async () => {
      setIsLoading(true);

      try {
        await axios
          .get<ApiResponse>(
            `${USER_ENDPOINT}/${userId}/saved/muscle-groups/page${queryString}`,
            {
              headers: {
                Accept: "application/json",
                Authorization: `bearer ${localStorage.getItem("token")}`,
              },
            },
          )
          .then((res) => {
            setMuscleGroupsPage({ ...res.data, values: res.data.muscleGroups });
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

    fetchMuscleGroups();
  }, [page, size, sort, order, query, forceRefreshValue]);

  return [muscleGroupsPage, error, isLoading];
};

export default useSavedMuscleGroupsPage;
