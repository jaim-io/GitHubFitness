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
  };

  return (
    <form onSubmit={(e) => handleLogin(e)}>
      <label>
        Email:
        <input
          type="text"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
      </label>
      <br />
      <br />
      <label>
        Password:
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
      </label>
      <br />
      <br />
      <input type="submit" value="Submit" />
    </form>
  );
};

export default LoginPage;
