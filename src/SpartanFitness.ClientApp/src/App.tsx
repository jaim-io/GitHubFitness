import { createBrowserRouter, RouterProvider } from "react-router-dom";
import HomePage from "./pages/Home";
import MainLayout from "./layouts/MainLayout";
import ErrorPage from "./pages/Error";
import ExercisesPage from "./pages/Exercises";
import ExerciseDetailPage from "./pages/ExerciseDetail";
import NewExercisePage from "./pages/NewExercise";
import EditExercisePage from "./pages/EditExercise";
import ExerciseLayout from "./layouts/ExerciseLayout";
import LoginPage from "./pages/Login";
import "./App.css";

const router = createBrowserRouter([
  {
    path: "/",
    element: <MainLayout />,
    errorElement: <ErrorPage />,
    children: [
      { index: true, element: <HomePage /> },
      { path: "login", element: <LoginPage /> },
      {
        path: "exercises",
        element: <ExerciseLayout />,
        children: [
          { index: true, element: <ExercisesPage /> },
          { path: "new", element: <NewExercisePage /> },
          { path: ":exerciseId", element: <ExerciseDetailPage /> },
          { path: ":exerciseId/edit", element: <EditExercisePage /> },
        ],
      },
    ],
  },
]);

const App = () => {
  return <RouterProvider router={router} />;
};

export default App;
