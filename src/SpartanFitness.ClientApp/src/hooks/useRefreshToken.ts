import axios from "axios";
import { useState } from "react";
import { toast } from "react-toastify";
import AuthenticationResponse from "../types/authentication/AuthenticationResponse";
import Exception from "../types/domain/Exception";
import useAuth from "./useAuth";

const AUTH_ENDPOINT = `${import.meta.env.VITE_API_URL}/v1/auth/refresh`;

const useRefreshToken = () => {
  const { setAuth } = useAuth();
  const [, setError] = useState<Exception>();

  return async () => {
    try {
      await axios
        .post<AuthenticationResponse>(
          AUTH_ENDPOINT,
          {
            token: localStorage.getItem("token"),
            refreshToken: localStorage.getItem("refreshToken"),
          },
          {
            headers: {
              Accept: "application/json",
            },
          },
        )
        .then((res) => {
          setAuth(res.data);
        })
        .catch((err) => {
          setError({
            message: err.message,
            code: err.code,
          });
          if (err.code == "ERR_NETWORK") {
            toast.error("Unable to reach the server", {
              toastId: err.code,
              position: "bottom-right",
              autoClose: 5000,
              hideProgressBar: false,
              closeOnClick: true,
              pauseOnHover: true,
              draggable: true,
              progress: undefined,
              theme: "colored",
            });
          }
        });
    } catch {
      /* empty */
    }
  };
};

export default useRefreshToken;
