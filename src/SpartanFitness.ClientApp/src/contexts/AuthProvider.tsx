import { ReactNode, createContext, useState } from "react";
import User from "../types/User";

type AuthContextType = {
  user: User | undefined;
  setAuth: (user: User) => void;
};

const AuthContext = createContext<AuthContextType>({
  user: undefined,
  setAuth: () => {},
});

type Props = {
  children: ReactNode;
};

export const AuthProvider = ({ children }: Props) => {
  const [user, setUser] = useState<User>();
  const authContext: AuthContextType = {
    user: user,
    setAuth: (user) => {
      setUser(user);
    },
  };

  return (
    <AuthContext.Provider value={authContext}>{children}</AuthContext.Provider>
  );
};

export default AuthContext;
