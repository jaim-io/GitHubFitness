import { useEffect, useState } from "react";
import useRefreshToken from "../hooks/useRefreshToken";
import useAuth from "../hooks/useAuth";
import { Outlet } from "react-router-dom";
import LoadingIcon from "./Icons/LoadingIcon";

const LoginPersistance = () => {
  const { auth } = useAuth();
  const [isLoading, setIsLoading] = useState(true);
  const refresh = useRefreshToken();

  useEffect(() => {
    const verifyRefreshToken = async () => {
      await refresh();
      setIsLoading(false);
    };

    !auth.accessToken && auth.persist
      ? verifyRefreshToken()
      : setIsLoading(false);
  }, []);

  return !auth.persist ? (
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
