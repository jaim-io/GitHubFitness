import axios from "axios";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import Page from "../types/base/Page";
import Exception from "../types/domain/Exception";
import Muscle from "../types/domain/Muscle";
import { createException, createValue, Result } from "../types/domain/Result";
import SearchParamsFactory from "../types/SearchParamsFactory";

const MUSCLES_ENDPOINT = `${import.meta.env.VITE_API_BASE}/muscles`;

export type MusclePage = { muscles: Muscle[] } & Page;

const useMuscles = (
  page?: number,
  size?: number,
  sort?: string,
  order?: string,
  query?: string,
): [Result<MusclePage>, boolean] => {
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
      } catch {}
    };

    fetchMuscles();
  }, [page, size, sort, order, query]);

  return musclePage == undefined
    ? [createException<MusclePage>()(error!), isLoading]
    : [createValue<MusclePage>()(musclePage!), isLoading];
};

export default useMuscles;
