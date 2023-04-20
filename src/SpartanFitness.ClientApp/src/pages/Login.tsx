import { useState } from "react";
import { useNavigate } from "react-router-dom";
import AuthenticationResponse from "../types/AuthenticationResponse";
import User from "../types/User";

const LoginPage = () => {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string>();
  const [user, setUser] = useState<User>();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleLogin = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    setIsLoading(true);

    await fetch(`${import.meta.env.VITE_API_URL}/auth/login`, {
      method: "POST",
      body: JSON.stringify({
        email: email,
        password: password,
      }),
      headers: {
        "Content-Type": "application/json",
      },
    })
      .then((res) => {
        if (!res.ok) {
          setError(res.statusText);
        }

        return res.json();
      })
      .then((res: AuthenticationResponse) => {
        localStorage.setItem("token", res.token);
        setUser(res);
      });

    setIsLoading(false);

    if (user) {
      navigate("/");
    }
  };

  return (
    <div className="flex justify-center pt-28">
      <form
        onSubmit={(e) => handleLogin(e)}
        id="login-form"
        className="w-full max-w-xs m-auto bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4"
      >
        <div className="mb-4">
          <label className="block text-gray-700 text-sm font-bold mb-2">
            Email
          </label>
          <input
            className="shadow appearance-none border rounded w-full py-2 px-3 text-white leading-tight focus:outline-none focus:shadow-outline"
            id="username"
            type="text"
            placeholder="example@gmail.com"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
        </div>
        <div className="mb-6">
          <label className="block text-gray-700 text-sm font-bold mb-2">
            Password
          </label>
          <input
            className="shadow appearance-none border border-red-500 rounded w-full py-2 px-3 text-white mb-3 leading-tight focus:outline-none focus:shadow-outline"
            id="password"
            type="password"
            placeholder="******************"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
          <p className="text-red-500 text-xs italic">
            Please choose a password.
          </p>
        </div>
        <div className="flex items-center justify-between">
          <button
            className="bg-red hover:bg-[#ff809c] text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
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
    </div>
  );
};

export default LoginPage;
