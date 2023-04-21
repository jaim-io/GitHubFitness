import { useContext, useEffect, useRef, useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import AuthenticationResponse from "../types/AuthenticationResponse";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import AuthContext from "../contexts/AuthProvider";
import axios from "axios";

const LOGIN_ENDPOINT = `${import.meta.env.VITE_API_URL}/auth/login`;

const LoginPage = () => {
  const { setAuth } = useContext(AuthContext);
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
  });

  const handleLogin = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

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
            setAuth(res.data);
            navigate("/");
          }
        })
        .catch((err) => {
          toast.error(
            err.code == "ERR_NETWORK"
              ? "Unable to reach server"
              : err.response.status == 400
              ? "Invalid credentials."
              : err.response.statusText,
            {
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
          setError({
            message: err.response.statusText,
            code: err.response.status,
          });
        });
    } catch {}

    setEmail("");
    setPassword("");
    setIsLoading(false);
  };

  return (
    <div className="flex justify-center pt-28">
      <form
        onSubmit={(e) => handleLogin(e)}
        id="login-form"
        className="w-full max-w-sm m-auto bg-[#161b22] shadow-xl rounded-lg px-8 pt-6 pb-5 mb-4 border border-[#30363d]"
      >
        <div className="mb-3">
          <label className="block text-white mb-2 ml-1">Email adress</label>
          <input
            className="shadow appearance-none border border-[#30363d] rounded-lg w-full py-2 px-3 text-white leading-tight focus:outline focus:outline-[#2f81f7] focus:shadow-outline bg-[#0d1117]"
            id="username"
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
            <NavLink to="/temp" className="absolute top-0 right-0 text-[#2f81f7]">
              Forgot Password?
            </NavLink>
          </div>
          <input
            className="shadow appearance-none border border-[#30363d] rounded-lg w-full py-2 px-3 text-white mb-3 leading-tight focus:outline focus:outline-[#2f81f7] focus:shadow-outline bg-[#0d1117]"
            id="password"
            type="password"
            placeholder="******************"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <div className="flex items-center justify-between">
          <button
            className="bg-[#238636] hover:bg-[#2EA043] text-white py-1 px-4 rounded-lg focus:outline-none focus:shadow-outline w-full block"
            type="submit"
            value="Submit"
            form="login-form"
          >
            Sign In
          </button>
        </div>
      </form>

      <ToastContainer
        position="bottom-right"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
        theme="colored"
      />
    </div>
  );
};

export default LoginPage;
