import { useContext, useEffect, useRef, useState } from "react";
import { useNavigate } from "react-router-dom";
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
        className="w-full max-w-xs m-auto bg-white shadow-md rounded-lg px-8 pt-6 pb-8 mb-4"
      >
        <div className="mb-4">
          <label className="block text-gray-700 text-sm font-bold mb-2">
            Email
          </label>
          <input
            className="shadow appearance-none border rounded-lg w-full py-2 px-3 text-white leading-tight focus:outline-none focus:shadow-outline"
            id="username"
            type="text"
            placeholder="example@gmail.com"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            autoComplete="off"
          />
        </div>
        <div className="mb-6">
          <label className="block text-gray-700 text-sm font-bold mb-2">
            Password
          </label>
          <input
            className="shadow appearance-none border border-red-500 rounded-lg w-full py-2 px-3 text-white mb-3 leading-tight focus:outline-none focus:shadow-outline"
            id="password"
            type="password"
            placeholder="******************"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
          <p className="text-red-500 text-xs italic">
            Please choose a password.
          </p>
        </div>
        <div className="flex items-center justify-between">
          <button
            className="bg-red hover:bg-[#ff809c] text-white font-bold py-2 px-4 rounded-lg focus:outline-none focus:shadow-outline"
            type="submit"
            value="Submit"
            form="login-form"
          >
            Sign In
          </button>
          <a
            className="inline-block align-baseline font-bold text-sm text-red hover:text-[#ff809c]"
            href="#"
          >
            Forgot Password?
          </a>
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
