import { ReactNode, createContext, useState } from "react";
import Authentication from "../types/authentication/Authentication";
import User from "../types/domain/User";
import AuthenticationResponse from "../types/authentication/AuthenticationResponse";

type AuthContextType = {
  auth: Authentication;
  setAuth: (response: AuthenticationResponse) => void;
  setPersist: (toggle: boolean) => void;
};

const defaultAuthentication: Authentication = {
  user: undefined,
  persist: true,
  accessToken: undefined,
  refreshToken: undefined,
};

const AuthContext = createContext<AuthContextType>({
  auth: defaultAuthentication,
  // eslint-disable-next-line @typescript-eslint/no-unused-vars,@typescript-eslint/no-empty-function
  setAuth: (_response: AuthenticationResponse) => {},
  // eslint-disable-next-line @typescript-eslint/no-unused-vars,@typescript-eslint/no-empty-function
  setPersist: (_toggle: boolean) => {},
});

type Props = {
  children: ReactNode;
};

export const AuthProvider = ({ children }: Props) => {
  const [user, setUser] = useState<User>();
  const [accessToken, setAccessToken] = useState<string>();
  const [refreshToken, setRefreshToken] = useState<string>();
  const [persist, setPersist] = useState(
    JSON.parse(localStorage.getItem("persist") ?? "false") || false,
  );

  const authContext: AuthContextType = {
    auth: {
      user: user,
      persist: persist,
      accessToken: accessToken,
      refreshToken: refreshToken,
    },
    setAuth: (response) => {
      setUser(response);
      setAccessToken(response.token);
      setRefreshToken(response.refreshToken);
    },
    setPersist: (toggle) => {
      localStorage.setItem("persist", String(toggle));
      setPersist(toggle);
    },
  };

  return (
    <AuthContext.Provider value={authContext}>{children}</AuthContext.Provider>
  );
};

export default AuthContext;
