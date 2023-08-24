import { ReactNode, createContext, useState } from "react";
import Authentication from "../types/authentication/Authentication";
import User from "../types/domain/User";
import AuthenticationResponse from "../types/responses/AuthenticationResponse";

type AuthContextType = {
  auth: Authentication;
  setAuth: (response: AuthenticationResponse) => void;
  logout: () => void;
};

const defaultAuthentication: Authentication = {
  user: undefined,
  accessToken: undefined,
  refreshToken: undefined,
};

const AuthContext = createContext<AuthContextType>({
  auth: defaultAuthentication,
  // eslint-disable-next-line @typescript-eslint/no-unused-vars,@typescript-eslint/no-empty-function
  setAuth: (_response: AuthenticationResponse) => {},
  // eslint-disable-next-line @typescript-eslint/no-unused-vars,@typescript-eslint/no-empty-function
  logout: () => {},
});

type Props = {
  children: ReactNode;
};

export const AuthProvider = ({ children }: Props) => {
  const [user, setUser] = useState<User>();
  const [accessToken, setAccessToken] = useState<string>();
  const [refreshToken, setRefreshToken] = useState<string>();

  const authContext: AuthContextType = {
    auth: {
      user: user,
      accessToken: accessToken,
      refreshToken: refreshToken,
    },
    setAuth: (response) => {
      setUser(response);
      setAccessToken(response.token);
      setRefreshToken(response.refreshToken);
      localStorage.setItem("token", response.token);
      localStorage.setItem("refreshToken", response.refreshToken);
      localStorage.setItem("uid", response.id);
    },
    logout: () => {
      setUser(undefined);
      setAccessToken("");
      setRefreshToken("");
      localStorage.removeItem("token");
      localStorage.removeItem("refreshToken");
      localStorage.removeItem("uid");
      localStorage.removeItem("persist");
    },
  };

  return (
    <AuthContext.Provider value={authContext}>{children}</AuthContext.Provider>
  );
};

export default AuthContext;
