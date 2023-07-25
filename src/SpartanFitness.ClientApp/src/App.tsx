import { createBrowserRouter, RouterProvider } from "react-router-dom";
import "./App.css";
import LoginPersistance from "./components/LoginPersistance";
import ExerciseLayout from "./layouts/ExerciseLayout";
import LoginLayout from "./layouts/LoginLayout";
import MainLayout from "./layouts/MainLayout";
import EditExercisePage from "./pages/exercises/EditExercise";
import ErrorPage from "./pages/Error";
import ExerciseDetailPage, {
  loader as exerciseDetailLoader,
} from "./pages/exercises/ExerciseDetail";
import ExercisesPage from "./pages/exercises/Exercises";
import HomePage from "./pages/Home";
import LoginPage from "./pages/Login";
import NewExercisePage from "./pages/exercises/NewExercise";
import MuscleGroupLayout from "./layouts/MuscleGroupLayout";
import MuscleGroupsPage from "./pages/muscle_groups/MuscleGroups";
import MuscleGroupDetailPage, {
  loader as muscleGroupDetailLoader,
} from "./pages/muscle_groups/MuscleGroupDetail";
import MuscleLayout from "./layouts/MuscleLayout";
import MusclesPage from "./pages/muscles/Muscles";
import MuscleDetailPage, {
  loader as muscleDetailLoader,
} from "./pages/muscles/MuscleDetail";
import EditMuscleGroupPage from "./pages/muscle_groups/EditMuscleGroup";
import EditMusclePage from "./pages/muscles/EditMuscle";
import WorkoutLayout from "./layouts/WorkoutLayout";
import WorkoutsPage from "./pages/workout/Workouts";
import WorkoutDetailPage, {
  loader as workoutDetailLoader,
} from "./pages/workout/WorkoutDetail";
import NewWorkoutPage from "./pages/workout/NewWorkout";
import EditWorkoutPage from "./pages/workout/EditWorkout";
import UserSettingsPage from "./pages/users/UserSettings";
import UserDetailPage from "./pages/users/UserDetail";
import CoachesPage from "./pages/coaches/Coaches";
import CoachDetailPage, {
  loader as coachDetailLoader,
} from "./pages/coaches/CoachDetail";
import CoachWorkoutsPage from "./pages/coaches/CoachWorkouts";
import LoginRequired from "./components/LoginRequired";

const router = createBrowserRouter([
  {
    path: "/",
    element: <LoginPersistance />,
    errorElement: <ErrorPage />,
    children: [
      {
        path: "/",
        element: <MainLayout />,
        errorElement: <ErrorPage />,
        children: [
          { index: true, element: <HomePage /> },
          {
            path: "user",
            element: <LoginRequired />,
            children: [
              { index: true, element: <UserDetailPage /> },
              { path: "settings", element: <UserSettingsPage /> },
            ],
          },
          {
            path: "exercises",
            element: <ExerciseLayout />,
            children: [
              { index: true, element: <ExercisesPage /> },
              {
                path: "new",
                element: <LoginRequired />,
                children: [{ index: true, element: <NewExercisePage /> }],
              },
              {
                path: ":exerciseId",
                element: <ExerciseDetailPage />,
                loader: exerciseDetailLoader,
              },
              {
                path: ":exerciseId/edit",
                element: <LoginRequired />,
                children: [
                  {
                    index: true,
                    element: <EditExercisePage />,
                    loader: exerciseDetailLoader,
                  },
                ],
              },
            ],
          },
          {
            path: "muscle-groups",
            element: <MuscleGroupLayout />,
            children: [
              { index: true, element: <MuscleGroupsPage /> },
              {
                path: ":muscleGroupId",
                element: <MuscleGroupDetailPage />,
                loader: muscleGroupDetailLoader,
              },
              {
                path: ":muscleGroupId/edit",
                element: <LoginRequired />,
                children: [
                  {
                    index: true,
                    element: <EditMuscleGroupPage />,
                    loader: muscleGroupDetailLoader,
                  },
                ],
              },
            ],
          },
          {
            path: "muscles",
            element: <MuscleLayout />,
            children: [
              { index: true, element: <MusclesPage /> },
              {
                path: ":muscleId",
                element: <MuscleDetailPage />,
                loader: muscleDetailLoader,
              },
              {
                path: ":muscleId/edit",
                element: <LoginRequired />,
                children: [
                  {
                    index: true,
                    element: <EditMusclePage />,
                    loader: muscleDetailLoader,
                  },
                ],
              },
            ],
          },
          {
            path: "coaches",
            children: [
              { index: true, element: <CoachesPage /> },
              {
                path: "all",
                children: [
                  { index: true, element: <CoachesPage /> },
                  {
                    path: "workouts",
                    element: <WorkoutLayout />,
                    children: [{ index: true, element: <WorkoutsPage /> }],
                  },
                ],
              },
              {
                path: ":coachId",
                children: [
                  {
                    index: true,
                    element: <CoachDetailPage />,
                    loader: coachDetailLoader,
                  },
                  {
                    path: "workouts",
                    element: <WorkoutLayout />,
                    children: [
                      { index: true, element: <CoachWorkoutsPage /> },
                      {
                        path: "new",
                        element: <LoginRequired />,
                        children: [
                          { index: true, element: <NewWorkoutPage /> },
                        ],
                      },
                      {
                        path: ":workoutId",
                        element: <WorkoutDetailPage />,
                        loader: workoutDetailLoader,
                      },
                      {
                        path: ":workoutId/edit",
                        element: <LoginRequired />,
                        children: [
                          {
                            index: true,
                            element: <EditWorkoutPage />,
                            loader: workoutDetailLoader,
                          },
                        ],
                      },
                    ],
                  },
                ],
              },
            ],
          },
        ],
      },
      {
        path: "/",
        element: <LoginLayout />,
        errorElement: <ErrorPage />,
        children: [{ path: "login", element: <LoginPage /> }],
      },
    ],
  },
]);

const App = () => {
  // TODO: Create a popup where the user can choose to perist their login or not
  // TODO: Loading bar

  return (
    <>
      <RouterProvider router={router} />{" "}
    </>
  );
};

export default App;
