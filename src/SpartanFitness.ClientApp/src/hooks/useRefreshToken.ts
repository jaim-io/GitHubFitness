import axios from "axios";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import AuthenticationResponse from "../types/authentication/AuthenticationResponse";
import Exception from "../types/domain/Exception";
import useAuth from "./useAuth";

const AUTH_ENDPOINT = `${import.meta.env.VITE_API_URL}/auth/refresh`;

const useRefreshToken = () => {
  const { setAuth } = useAuth();
  const [error, setError] = useState<Exception>();

  const refresh = async () => {
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
          localStorage.setItem("token", res.data.token);
          localStorage.setItem("refreshToken", res.data.refreshToken);
          localStorage.setItem("uid", res.data.id);
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
    } catch {}
  };
  return refresh;
};

export default useRefreshToken;
