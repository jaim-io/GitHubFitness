import { createBrowserRouter, RouterProvider } from "react-router-dom";
import "./App.css";
import LoginPersistance from "./components/LoginPersistance";
import ExerciseLayout from "./layouts/ExerciseLayout";
import LoginLayout from "./layouts/LoginLayout";
import MainLayout from "./layouts/MainLayout";
import EditExercisePage from "./pages/EditExercise";
import ErrorPage from "./pages/Error";
import ExerciseDetailPage, {
  loader as exerciseDetailLoader,
} from "./pages/ExerciseDetail";
import ExercisesPage from "./pages/Exercises";
import HomePage from "./pages/Home";
import LoginPage from "./pages/Login";
import NewExercisePage from "./pages/NewExercise";
import MuscleGroupLayout from "./layouts/MuscleGroupLayout";
import MuscleGroupsPage from "./pages/MuscleGroups";
import MuscleGroupDetailPage, {
  loader as muscleGroupDetailLoader,
} from "./pages/MuscleGroupDetail";
import MuscleLayout from "./layouts/MuscleLayout";
import MusclesPage from "./pages/Muscles";
import MuscleDetailPage, {
  loader as muscleDetailLoader,
} from "./pages/MuscleDetail";

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
            path: "exercises",
            element: <ExerciseLayout />,
            children: [
              { index: true, element: <ExercisesPage /> },
              { path: "new", element: <NewExercisePage /> },
              {
                path: ":exerciseId",
                element: <ExerciseDetailPage />,
                loader: exerciseDetailLoader,
              },
              {
                path: ":exerciseId/edit",
                element: <EditExercisePage />,
                loader: exerciseDetailLoader,
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
  localStorage.setItem("persist", "true");

  return (
    <>
      <RouterProvider router={router} />{" "}
    </>
  );
};

export default App;
