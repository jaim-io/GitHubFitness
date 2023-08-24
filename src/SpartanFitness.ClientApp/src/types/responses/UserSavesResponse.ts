import Exercise from "../domain/Exercise";
import Muscle from "../domain/Muscle";
import MuscleGroup from "../domain/MuscleGroup";
import Workout from "../domain/Workout";

type UserSavesResponse = {
  exercises: Exercise[];
  workouts: Workout[];
  muscles: Muscle[];
  muscleGroups: MuscleGroup[];
};

export default UserSavesResponse;
