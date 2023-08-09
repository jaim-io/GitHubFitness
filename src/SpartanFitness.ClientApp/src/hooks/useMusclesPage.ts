import axios from "axios";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import Page from "../types/domain/Page";
import Exception from "../types/domain/Exception";
import Muscle from "../types/domain/Muscle";
import SearchParamsFactory from "../utils/SearchParamsFactory";

const MUSCLES_ENDPOINT = `${import.meta.env.VITE_API_BASE}/muscles/page`;

type ApiResponse = Omit<Page<Muscle>, "values"> & {
  muscles: Muscle[];
};

type PageArguments = {
  page?: number;
  size?: number;
  sort?: string;
  order?: string;
  query?: string;
};

const useMusclesPage = ({
  page,
  size,
  sort,
  order,
  query,
}: PageArguments): [
  Page<Muscle> | undefined,
  Exception | undefined,
  boolean,
] => {
  const [musclePage, setMusclePage] = useState<Page<Muscle>>();
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
    const fetchMuscles = async () => {
      setIsLoading(true);

      try {
        await axios
          .get<ApiResponse>(`${MUSCLES_ENDPOINT}${queryString}`, {
            headers: {
              Accept: "application/json",
              Authorization: `bearer ${localStorage.getItem("token")}`,
            },
          })
          .then((res) => {
            setMusclePage({ ...res.data, values: res.data.muscles });
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

    fetchMuscles();
  }, [page, size, sort, order, query]);

  return [musclePage, error, isLoading];
};

export default useMusclesPage;
