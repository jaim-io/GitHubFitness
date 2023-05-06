import { useEffect, useState } from "react";
import useRefreshToken from "../hooks/useRefreshToken";
import useAuth from "../hooks/useAuth";
import { Outlet } from "react-router-dom";

const LoginPersistance = () => {
  const { auth } = useAuth();
  const [, setIsLoading] = useState(true);
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

  return <Outlet />;
};

export default LoginPersistance;
