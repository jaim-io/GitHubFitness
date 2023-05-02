import { useContext, useEffect, useRef, useState } from "react";
import { Link, NavLink, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import AuthContext from "../contexts/AuthProvider";
import axios from "axios";
import LogoSvg from "../assets/logo.svg";
import Exception from "../types/domain/Exception";
import AuthenticationResponse from "../types/authentication/AuthenticationResponse";

const LOGIN_ENDPOINT = `${import.meta.env.VITE_API_URL}/auth/login`;

const LoginPage = () => {
  const { auth, setAuth } = useContext(AuthContext);
  const emailRef = useRef<HTMLInputElement>();

  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<Exception | null>(null);

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
            console.log(res.data);

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
          className="w-full max-w-sm m-auto bg-[#161b22] shadow-xl rounded-lg px-8 pt-6 pb-5 mb-4 border border-[#30363d]"
        >
          <div className="mb-4">
            <label className="block text-white mb-2 ml-1">Email adress</label>
            <input
              className="shadow appearance-none border border-[#30363d] rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-[#2f81f7] focus:shadow-outline bg-[#0d1117]"
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
              <NavLink
                to="/temp"
                className="absolute top-0 right-0 text-[#2f81f7]"
              >
                Forgot Password?
              </NavLink>
            </div>
            <input
              className="shadow appearance-none border border-[#30363d] rounded-lg w-full py-1.5 px-3 text-white mb-3 leading-tight focus:outline focus:outline-[#2f81f7] focus:shadow-outline bg-[#0d1117]"
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
              className="bg-[#238636] hover:bg-[#2EA043] text-white py-1.5 px-4 rounded-lg focus:outline-none focus:shadow-outline w-full block"
              type="submit"
              value="Submit"
              form="login-form"
            >
              {isLoading == false && <p>Sign In</p>}
              {(isLoading == true || isLoading == undefined) && (
                <div className="flex items-center justify-center animate-pulse">
                  <svg
                    aria-hidden="true"
                    className="w-6 h-6 mr-2 text-gray-200 animate-spin dark:text-gray-600 fill-white"
                    viewBox="0 0 100 101"
                    fill="none"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z"
                      fill="currentColor"
                    />
                    <path
                      d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z"
                      fill="currentFill"
                    />
                  </svg>
                  <p>Logging in...</p>
                </div>
              )}
            </button>
          </div>
        </form>
      </div>

      <div className="flex justify-center">
        <p className="px-10 pt-6 pb-5 mb-4 border border-[#30363d] max-w-sm rounded-lg">
          Not a Spartan yet?{" "}
          <Link to="/register" className="text-[#2f81f7]">
            Create an account
          </Link>
          .
        </p>
      </div>
    </>
  );
};

export default LoginPage;
