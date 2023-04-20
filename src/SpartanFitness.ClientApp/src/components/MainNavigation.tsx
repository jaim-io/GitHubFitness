import { Link } from "react-router-dom";
import MainNavItem from "./MainNavItem";
import UserNavItem from "./UserNavItem";

const MainNavigation = () => {
  return (
    <nav>
      <div className="flex flex-wrap items-center justify-between mx-auto p-4 backdrop-blur">
        <Link to="/" className="flex items-center">
          <img
            src="./logo-no-circle.svg"
            className="h-10 mr-3"
            alt="Flowbite Logo"
          />
        </Link>
        <div className="hidden w-full md:block md:w-auto">
          <ul className="flex flex-col p-4 md:p-0 mt-4 md:flex-row md:space-x-8 md:mt-0 md:border-0 items-center">
            <MainNavItem path="/" children="Home" />
            <MainNavItem path="/exercises" children="Exercises" />
            <UserNavItem />
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default MainNavigation;
