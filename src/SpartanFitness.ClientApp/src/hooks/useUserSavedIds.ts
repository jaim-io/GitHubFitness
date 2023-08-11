import { useEffect, useState } from "react";
import Exception from "../types/domain/Exception";
import axios from "axios";
import { toast } from "react-toastify";

const USER_ENDPOINT = `${import.meta.env.VITE_API_BASE}/users`;

type RouteArgument = "exercises" | "workouts" | "muscles" | "muscle-groups";

const useUserSavedIds = (
  userId: string,
  routArg: RouteArgument,
): [string[] | undefined, Exception | undefined, boolean] => {
  const [ids, setIds] = useState<string[]>();
  const [error, setError] = useState<Exception>();
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const fetchIdsAsync = async () => {
      setIsLoading(true);

      try {
        await axios
          .get<{ ids: string[] }>(
            `${USER_ENDPOINT}/${userId}/saved/${routArg}/all/ids`,
            {
              headers: {
                Accept: "application/json",
                Authorization: `bearer ${localStorage.getItem("token")}`,
              },
            },
          )
          .then((res) => {
            setIds(res.data.ids);
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

    fetchIdsAsync();
  }, []);

  return [ids, error, isLoading];
};

export default useUserSavedIds;
