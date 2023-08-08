import Exercise from "../domain/Exercise";
import Muscle from "../domain/Muscle";
import MuscleGroup from "../domain/MuscleGroup";
import Workout from "../domain/Workout";

type UserSavesResult = {
  exercises: Exercise[];
  workouts: Workout[];
  muscles: Muscle[];
  muscleGroups: MuscleGroup[];
};

export default UserSavesResult;
