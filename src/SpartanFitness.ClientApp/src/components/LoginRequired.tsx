import { useEffect } from "react";
import { Outlet, useNavigate } from "react-router-dom";
import useAuth from "../hooks/useAuth";
import LoadingIcon from "./Icons/LoadingIcon";

const LoginRequired = () => {
  const { auth } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (auth.user === undefined) {
      navigate("/login");
    }
  }, []);

  return auth.user === undefined ? (
    <>
      <div className="absolute left-0 top-0 w-full h-full z-11 opacity-80 bg-black" />

      <div className="absolute left-0 top-0 w-full h-full z-12">
        <div className="z-12 flex justify-center items-center alignmi w-full h-full">
          <LoadingIcon classNames="mr-2 fill-blue text-gray w-10 h-10" />
          <p className="ml-1 text-white">Verifying login...</p>
          <span className="sr-only">Loading...</span>
        </div>
      </div>

      <div
        role="status"
        className="py-[30rem] flex justify-center items-center"
      ></div>
    </>
  ) : (
    <Outlet />
  );
};

export default LoginRequired;
