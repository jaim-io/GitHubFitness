type Workout = {
  id: string;
  name: string;
  description: string;
  coachId: string;
  image: string;
  muscleIds: string[];
  muscleGroupIds: string[];
  workoutExercises: WorkoutExercise[];
  createdDateTime: Date;
  updatedDateTime: Date;
};

export type WorkoutExercise = {
  id: string;
  orderNumber: number;
  exerciseId: string;
  sets: number;
  minReps: number;
  maxReps: number;
  exerciseType: string;
};

export const EXERCISE_TYPES = ["Default", "Dropset", "Superset"];

export default Workout;
