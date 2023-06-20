type User = {
  id: string;
  firstName: string;
  lastName: string;
  profileImage: string;
  email: string;
  roles: Role[];
  savedExerciseIds: string[];
  savedMuscleIds: string[];
  savedMuscleGroupIds: string[];
  savedWorkoutIds: string[];
};

type Role = {
  name: string;
  id: string;
};

export default User;
