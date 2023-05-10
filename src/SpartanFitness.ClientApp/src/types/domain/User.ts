type User = {
  id: string;
  firstName: string;
  lastName: string;
  profileImage: string;
  email: string;
  savedExerciseIds: string[];
  savedMusclesIds: string[];
  savedMuscleGroupIds: string[];
};

export default User;
