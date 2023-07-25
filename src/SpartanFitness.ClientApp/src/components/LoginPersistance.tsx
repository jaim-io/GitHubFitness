import { useEffect, useState } from "react";
import { Outlet } from "react-router-dom";
import useAuth from "../hooks/useAuth";
import useRefreshToken from "../hooks/useRefreshToken";
import LoadingIcon from "./icons/LoadingIcon";

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

  return !persist ? (
    <Outlet />
  ) : isLoading ? (
    <div className="absolute left-[50%] top-[50%]">
      <LoadingIcon classNames="mr-2 animate-spin fill-blue text-gray w-8 h-8" />
    </div>
  ) : (
    <Outlet />
  );
};

export default LoginPersistance;
