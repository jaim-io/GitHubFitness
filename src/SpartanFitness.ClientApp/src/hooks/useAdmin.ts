import axios from "axios";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import Administrator from "../types/domain/Administrator";
import Exception from "../types/domain/Exception";

const ADMIN_ENDPOINT = `${import.meta.env.VITE_API_BASE}/admins`;

const useAdmin = (
  id: string,
): [Administrator | undefined, Exception | undefined, boolean] => {
  const [admin, setAdmin] = useState<Administrator>();
  const [error, setError] = useState<Exception>();
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const fetchCoach = async () => {
      setIsLoading(true);

      await axios
        .get(`${ADMIN_ENDPOINT}/${id}`, {
          headers: {
            Accept: "application/json",
            Authorization: `bearer ${localStorage.getItem("token")}`,
          },
        })
        .then((res) => {
          setAdmin(res.data);
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

  return [admin, error, isLoading];
};
export default useAdmin;
