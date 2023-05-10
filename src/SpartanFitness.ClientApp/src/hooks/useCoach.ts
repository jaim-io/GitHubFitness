import Exception from "../types/domain/Exception";
import Coach from "../types/domain/Coach";
import { useEffect, useState } from "react";
import axios from "axios";
import { toast } from "react-toastify";

const COACH_ENDPOINT = `${import.meta.env.VITE_API_BASE}/coaches`;

const useCoach = (
  id: string,
): [Coach | undefined, Exception | undefined, boolean] => {
  const [coach, setCoach] = useState<Coach>();
  const [error, setError] = useState<Exception>();
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const fetchCoach = async () => {
      setIsLoading(true);

      await axios
        .get(`${COACH_ENDPOINT}/${id}`, {
          headers: {
            Accept: "application/json",
            Authorization: `bearer ${localStorage.getItem("token")}`,
          },
        })
        .then((res) => {
          setCoach(res.data);
          setIsLoading(false);
        })
        .catch((err) => {
          setError({
            message: err.message,
            code: err.code,
          });
          setIsLoading(false);
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
    };

    fetchCoach();
  }, [id]);

  return [coach, error, isLoading];
};
export default useCoach;
