import { Outlet } from "react-router-dom";
import MainNavigation from "../components/MainNavigation";
import Footer from "../components/Footer";

const MainLayout = () => {
  return (
    <>
      <MainNavigation />
      <main>
        <Outlet />
      </main>
      <Footer />
    </>
  );
};

export default MainLayout;
