import { Outlet } from "react-router-dom";

const WorkoutLayout = () => {
  return (
    <>
      <main>
        <Outlet />
      </main>
    </>
  );
};

export default WorkoutLayout;
