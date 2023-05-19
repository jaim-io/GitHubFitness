import { Outlet } from "react-router-dom";

const MuscleLayout = () => {
  return (
    <>
      <main>
        <Outlet />
      </main>
    </>
  );
};

export default MuscleLayout;
