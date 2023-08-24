import User from "../domain/User";

type AuthenticationResponse = {
  token: string;
  refreshToken: string;
} & User;

export default AuthenticationResponse;
