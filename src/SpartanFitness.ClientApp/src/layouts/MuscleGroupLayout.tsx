import { Outlet } from "react-router-dom";

const MuscleGroupLayout = () => {
  return (
    <>
      <main>
        <Outlet />
      </main>
    </>
  );
};

export default MuscleGroupLayout;
