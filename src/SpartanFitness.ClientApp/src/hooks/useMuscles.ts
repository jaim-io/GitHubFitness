import { useEffect, useState } from "react";
import Muscle from "../types/domain/Muscle";
import Exception from "../types/domain/Exception";
import axios from "axios";
import { toast } from "react-toastify";

const MUSCLES_ENDPOINT = `${import.meta.env.VITE_API_BASE}/muscles`;

const useMuscles = (): [
  Muscle[] | undefined,
  Exception | undefined,
  boolean,
] => {
  const [muscles, setMuscles] = useState<Muscle[]>();
  const [error, setError] = useState<Exception>();
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const fetchMusclesAsync = async () => {
      setIsLoading(true);

      try {
        await axios
          .get<Muscle[]>(`${MUSCLES_ENDPOINT}`, {
            headers: {
              Accept: "application/json",
              Authorization: `bearer ${localStorage.getItem("token")}`,
            },
          })
          .then((res) => {
            setMuscles(res.data);
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

    fetchMusclesAsync();
  }, []);

  return [muscles, error, isLoading];
};

export default useMuscles;
