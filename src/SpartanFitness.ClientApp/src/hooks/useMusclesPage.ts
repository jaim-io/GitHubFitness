import axios from "axios";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import Page from "../types/domain/Page";
import Exception from "../types/domain/Exception";
import Muscle from "../types/domain/Muscle";
import SearchParamsFactory from "../types/SearchParamsFactory";

const MUSCLES_ENDPOINT = `${import.meta.env.VITE_API_BASE}/page/muscles`;

export type MusclePage = { muscles: Muscle[] } & Page;

const useMusclesPage = (
  page?: number,
  size?: number,
  sort?: string,
  order?: string,
  query?: string,
): [MusclePage | undefined, Exception | undefined, boolean] => {
  const [musclePage, setMusclePage] = useState<MusclePage>();
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
          .get<MusclePage>(`${MUSCLES_ENDPOINT}${queryString}`, {
            headers: {
              Accept: "application/json",
              Authorization: `bearer ${localStorage.getItem("token")}`,
            },
          })
          .then((res) => {
            setMusclePage(res.data);
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
