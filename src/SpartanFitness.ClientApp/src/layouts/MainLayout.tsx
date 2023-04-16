import { Outlet } from "react-router-dom";
import MainNavigation from "../components/MainNavigation";

const MainLayout = () => {
  return (
    <>
      <MainNavigation />
      <main>
        <Outlet />
      </main>
    </>
  );
};

export default MainLayout;
