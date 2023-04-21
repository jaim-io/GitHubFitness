import User from "./User";

type AuthenticationResponse = {
  token: string;
} & User;

export default AuthenticationResponse;
