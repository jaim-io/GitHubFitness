import { Link } from "react-router-dom";
import MainNavItem from "./MainNavItem";
import UserNavItem from "./UserNavItem";
import LogoNoCircle from "../assets/logo-no-circle.svg"

const MainNavigation = () => {
  return (
    <nav className="flex flex-wrap items-center justify-between mx-auto p-4 backdrop-blur">
      <Link to="/" className="flex items-center">
        <img
          src={LogoNoCircle}
          className="h-10 mr-3 transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-110 duration-300 inline-block"
          alt="SpartanFitness Logo"
        />
      </Link>
      <div className="hidden w-full md:block md:w-auto">
        <ul className="flex flex-col p-4 md:p-0 mt-4 md:flex-row md:space-x-8 md:mt-0 md:border-0 items-center">
          <MainNavItem path="/" children="Home" />
          <MainNavItem path="/exercises" children="Exercises" />
          <UserNavItem />
        </ul>
      </div>
    </nav>
  );
};

export default MainNavigation;
