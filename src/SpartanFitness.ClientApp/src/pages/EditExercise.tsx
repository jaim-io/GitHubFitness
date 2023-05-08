import { useRouteLoaderData } from "react-router-dom";
import Exercise from "../types/domain/Exercise";

const EditExercisePage = () => {
  const exercise = useRouteLoaderData("exercise-details") as Exercise;

  return <>{exercise.name}</>;
};

export default EditExercisePage;
