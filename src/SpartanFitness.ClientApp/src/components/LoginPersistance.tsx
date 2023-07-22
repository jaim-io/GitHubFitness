import { useEffect, useState } from "react";
import useRefreshToken from "../hooks/useRefreshToken";
import useAuth from "../hooks/useAuth";
import { Outlet } from "react-router-dom";
import LoadingIcon from "./Icons/LoadingIcon";
import LoginPage from "../pages/Login";

const LoginPersistance = () => {
  const { auth } = useAuth();
  const [isLoading, setIsLoading] = useState(true);
  const refresh = useRefreshToken();

  const persist =
    JSON.parse(localStorage.getItem("persist") ?? "false") || false;

  useEffect(() => {
    const verifyRefreshToken = async () => {
      await refresh();
      setIsLoading(false);
    };

    !auth.accessToken && persist ? verifyRefreshToken() : setIsLoading(false);
  }, []);

  return !persist && auth.user === undefined ? (
    <LoginPage />
  ) : isLoading ? (
    <div className="absolute left-[50%] top-[50%]">
      <LoadingIcon classNames="mr-2 animate-spin fill-blue text-gray w-8 h-8" />
    </div>
  ) : auth.user === undefined ? (
    <LoginPage />
  ) : (
    <Outlet />
  );
};

export default LoginPersistance;
