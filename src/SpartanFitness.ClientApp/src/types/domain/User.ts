type User = {
  id: string;
  firstName: string;
  lastName: string;
  profileImage: string;
  email: string;
  roles: Role[];
  savedExerciseIds: string[];
  savedMusclesIds: string[];
  savedMuscleGroupIds: string[];
};

type Role = {
  name: string;
  id: string;
};

export default User;
