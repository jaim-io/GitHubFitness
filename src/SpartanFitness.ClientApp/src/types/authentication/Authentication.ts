import User from "../domain/User";

type Authentication = {
  user: User | undefined;
  accessToken: string | undefined;
  refreshToken: string | undefined;
  persist: boolean;
};

export default Authentication;
