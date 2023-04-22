import { Outlet } from "react-router-dom";

const ExerciseLayout = () => {
  return (
    <>
      <main>
        <Outlet />
      </main>
    </>
  );
};

export default ExerciseLayout;
