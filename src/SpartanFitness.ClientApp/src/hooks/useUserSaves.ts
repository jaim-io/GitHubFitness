import { useEffect, useState } from "react";
import Exception from "../types/domain/Exception";
import axios from "axios";
import { toast } from "react-toastify";
import UserSavesResponse from "../types/responses/UserSavesResponse";

const USER_ENDPOINT = `${import.meta.env.VITE_API_BASE}/users`;

const useUserSaves = (
  userId: string,
): [UserSavesResponse | undefined, Exception | undefined, boolean] => {
  const [userSaves, setUserSaves] = useState<UserSavesResponse>();
  const [error, setError] = useState<Exception>();
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const fetchUseSavesAsync = async () => {
      setIsLoading(true);

      try {
        await axios
          .get<UserSavesResponse>(`${USER_ENDPOINT}/${userId}/saves`, {
            headers: {
              Accept: "application/json",
              Authorization: `bearer ${localStorage.getItem("token")}`,
            },
          })
          .then((res) => {
            setUserSaves(res.data);
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

    fetchUseSavesAsync();
  }, []);

  return [userSaves, error, isLoading];
};

export default useUserSaves;
