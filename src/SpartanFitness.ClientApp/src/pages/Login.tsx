import { useContext, useEffect, useRef, useState } from "react";
import { Link, NavLink, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import AuthContext from "../contexts/AuthProvider";
import axios from "axios";
import LogoSvg from "../assets/logo.svg";
import Exception from "../types/domain/Exception";
import AuthenticationResponse from "../types/authentication/AuthenticationResponse";
import LoadingIcon from "../components/Icons/LoadingIcon";

const LOGIN_ENDPOINT = `${import.meta.env.VITE_API_URL}/auth/login`;

const LoginPage = () => {
  const { auth, setAuth } = useContext(AuthContext);
  const emailRef = useRef<HTMLInputElement>();

  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const [, setError] = useState<Exception | null>(null);

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  useEffect(() => {
    if (emailRef.current) {
      emailRef.current.focus();
    }
    if (auth.user != null) {
      navigate("/");
    }
  });

  const handleLogin = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setIsLoading(true);
    setError(null);

    try {
      await axios
        .post<AuthenticationResponse>(
          LOGIN_ENDPOINT,
          { email: email, password: password },
          {
            headers: { "Content-Type": "application/json" },
          },
        )
        .then((res) => {
          if (res.data.id) {
            localStorage.setItem("token", res.data.token);
            localStorage.setItem("refreshToken", res.data.refreshToken);
            localStorage.setItem("uid", res.data.id);
            setAuth(res.data);
            navigate("/");
          }
        })
        .catch((err) => {
          setError({
            message: err.message,
            code: err.code,
          });
          toast.error(
            err.code == "ERR_NETWORK"
              ? "Unable to reach the server"
              : err.response.status == 400
              ? "Invalid credentials."
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
    } catch {}

    setEmail("");
    setPassword("");
    setIsLoading(false);
  };

  return (
    <>
      <div className="flex justify-center pt-28 pb-10">
        <div className="">
          <img src={LogoSvg} className="w-[10rem] h-[10rem] mx-auto" />
          <p>Sign in to SpartanFitness</p>
        </div>
      </div>

      <div className="flex justify-center pb-1">
        <form
          onSubmit={(e) => handleLogin(e)}
          id="login-form"
          className="w-full max-w-sm m-auto bg-semi-black shadow-xl rounded-lg px-8 pt-6 pb-5 mb-4 border border-gray"
        >
          <div className="mb-4">
            <label className="block text-white mb-2 ml-1">Email address</label>
            <input
              className="shadow appearance-none border border-gray rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black"
              id="email"
              type="text"
              placeholder="example@gmail.com"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              autoComplete="off"
            />
          </div>
          <div className="mb-1">
            <div className="relative">
              <label className="block text-white mb-2 ml-1">Password</label>
              <NavLink to="/temp" className="absolute top-0 right-0 text-blue">
                Forgot Password?
              </NavLink>
            </div>
            <input
              className="shadow appearance-none border border-gray rounded-lg w-full py-1.5 px-3 text-white mb-3 leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black"
              id="password"
              type="password"
              placeholder="******************"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          <div className="flex items-center justify-between pb-1">
            <button
              className="bg-dark-green hover:bg-light-green text-white py-1.5 px-4 rounded-lg focus:outline-none focus:shadow-outline w-full block"
              type="submit"
              value="Submit"
              form="login-form"
            >
              {isLoading == false && <p>Sign In</p>}
              {(isLoading == true || isLoading == undefined) && (
                <div className="flex items-center justify-center animate-pulse">
                  <LoadingIcon
                    classNames="mr-2 text-gray-200 dark:text-gray-600 fill-white"
                    size={6}
                  />
                  <p>Logging in...</p>
                </div>
              )}
            </button>
          </div>
        </form>
      </div>

      <div className="flex justify-center">
        <p className="px-10 pt-6 pb-5 mb-4 border border-gray max-w-sm rounded-lg">
          Not a Spartan yet?{" "}
          <Link to="/register" className="text-blue">
            Create an account
          </Link>
          .
        </p>
      </div>
    </>
  );
};

export default LoginPage;
