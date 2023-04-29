import { useEffect, useState } from "react";
import useRefreshToken from "../hooks/useRefreshToken";
import useAuth from "../hooks/useAuth";
import { Outlet } from "react-router-dom";

// const PersistLogin = () => {
//   const { auth } = useAuth();
//   const [isLoading, setIsLoading] = useState(true);
//   const refresh = useRefreshToken();

//   if (!auth.accessToken && auth.persist) setIsLoading(false);

//   return (
//     <>
//       {!auth.persist ? <Outlet /> : isLoading ? <p>Loading...</p> : <Outlet />}
//     </>
//   );
// };
let trigger = true;

const PersistLogin = () => {
  const refresh = useRefreshToken();

  useEffect(() => {
    const callRefresh = async () => {
      await refresh();
    }
    callRefresh();
  }, []);

  return (
    <>
      <Outlet />
    </>
  );
};

export default PersistLogin;
