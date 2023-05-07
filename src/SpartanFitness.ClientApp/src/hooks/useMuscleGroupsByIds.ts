import { useEffect, useState } from "react";
import Exception from "../types/domain/Exception";
import axios from "axios";
import { toast } from "react-toastify";
import MuscleGroup from "../types/domain/MuscleGroup";

const MUSCLEGROUPS_ENDPOINT = `${import.meta.env.VITE_API_BASE}/muscle-groups`;

const createQueryString = (ids: string[]): string => {
  const params: string[] = [];

  ids.forEach((id) => params.push(`id=${id}`));

  const queryString = `?${params.join("&")}`;

  return ids.length == 0 ? "" : queryString;
};

const useMuscleGroupsById = (
  ids: string[],
): [MuscleGroup[] | undefined, Exception | undefined, boolean] => {
  const [muscleGroups, setMuscleGroups] = useState<MuscleGroup[]>();
  const [error, setError] = useState<Exception>();
  const [isLoading, setIsLoading] = useState(false);

  const queryString = createQueryString(ids);

  useEffect(() => {
    const fetchMuscleGroupsAsync = async () => {
      setIsLoading(true);

      try {
        await axios
          .get<MuscleGroup[]>(`${MUSCLEGROUPS_ENDPOINT}${queryString}`, {
            headers: {
              Accept: "application/json",
              Authorization: `bearer ${localStorage.getItem("token")}`,
            },
          })
          .then((res) => {
            setMuscleGroups(res.data);
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

    fetchMuscleGroupsAsync();
  }, [ids]);

  return [muscleGroups, error, isLoading];
};

export default useMuscleGroupsById;
