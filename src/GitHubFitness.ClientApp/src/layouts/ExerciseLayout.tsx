import { Outlet } from "react-router-dom";
import ExercisesNavigation from "../components/ExercisesNavigation";

const ExerciseLayout = () => {
  return (
    <>
      <main>
        <ExercisesNavigation />
        <Outlet />
      </main>
    </>
  );
};

export default ExerciseLayout;
