import { Link } from "react-router-dom";
import MainNavItem from "./MainNavItem";
import UserNavItem from "./UserNavItem";
import LogoWhiteNoCircle from "../assets/logo-white-no-circle.svg";
import { useContext } from "react";
import AuthContext from "../contexts/AuthProvider";
import AuthNavItem from "./AuthNavItem";

const MainNavigation = () => {
  const { auth } = useContext(AuthContext);

  return (
    <nav className="bg-semi-black py-3 px-24">
      <div className="flex flex-wrap items-center justify-between mx-auto">
        <ul className="flex flex-col p-4 md:p-0 mt-4 md:flex-row md:space-x-4 md:mt-0 md:border-0 items-center">
          <Link to="/" className="flex items-center">
            <img
              src={LogoWhiteNoCircle}
              className="h-10 mr-3 inline-block"
              alt="SpartanFitness Logo"
            />
          </Link>
          <MainNavItem path="/">Home</MainNavItem>
          <MainNavItem path="/exercises">Exercises</MainNavItem>
          <MainNavItem path="/muscle-groups">Muscle groups</MainNavItem>
          <MainNavItem path="/muscles">Muscles</MainNavItem>
        </ul>
        <div className="w-full md:block md:w-auto">
          <ul className="flex flex-col p-4 md:p-0 mt-4 md:flex-row md:space-x-4 md:mt-0 md:border-0 items-center">
            {auth.user && <UserNavItem />}
            {!auth.user && <AuthNavItem path="/login">Sign in</AuthNavItem>}
            {!auth.user && (
              <AuthNavItem path="/register" border={true}>
                Sign up
              </AuthNavItem>
            )}
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default MainNavigation;
