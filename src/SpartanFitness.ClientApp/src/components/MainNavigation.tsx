import { Link } from "react-router-dom";
import MainNavItem from "./MainNavItem";
import UserNavItem from "./UserNavItem";
import LogoWhiteNoCircle from "../assets/logo-white-no-circle.svg";
import { useContext } from "react";
import AuthContext from "../contexts/AuthProvider";
import AuthNavItem from "./AuthNavItem";

const MainNavigation = () => {
  const { user } = useContext(AuthContext);

  return (
    <nav className="backdrop-blur bg-[#161b22] py-3 px-24">
      <div className="flex flex-wrap items-center justify-between mx-auto">
        <ul className="flex flex-col p-4 md:p-0 mt-4 md:flex-row md:space-x-4 md:mt-0 md:border-0 items-center">
          <Link to="/" className="flex items-center">
            <img
              src={LogoWhiteNoCircle}
              className="h-10 mr-3 transition ease-in-out delay-150 hover:scale-105 duration-300 inline-block"
              alt="SpartanFitness Logo"
            />
          </Link>
          <MainNavItem path="/" children="Home" />
          <MainNavItem path="/exercises" children="Exercises" />
        </ul>
        <div className="w-full md:block md:w-auto">
          <ul className="flex flex-col p-4 md:p-0 mt-4 md:flex-row md:space-x-4 md:mt-0 md:border-0 items-center">
            {user && <UserNavItem />}
            {!user && <AuthNavItem path="/login" children="Sign in" />}
            {!user && (
              <AuthNavItem path="/register" children="Sign up" border={true} />
            )}
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default MainNavigation;
