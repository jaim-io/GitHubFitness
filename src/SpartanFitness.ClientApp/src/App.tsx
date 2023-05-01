import {
  createBrowserRouter,
  RouterProvider,
  useNavigate,
} from "react-router-dom";
import "./App.css";
import ExerciseLayout from "./layouts/ExerciseLayout";
import LoginLayout from "./layouts/LoginLayout";
import MainLayout from "./layouts/MainLayout";
import EditExercisePage from "./pages/EditExercise";
import ErrorPage from "./pages/Error";
import ExerciseDetailPage from "./pages/ExerciseDetail";
import ExercisesPage from "./pages/Exercises";
import HomePage from "./pages/Home";
import LoginPage from "./pages/Login";
import NewExercisePage from "./pages/NewExercise";
import { useContext, useEffect } from "react";
import { toast } from "react-toastify";
import AuthContext from "./contexts/AuthProvider";
import LoginPersistance from "./components/LoginPersistance";

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
              { path: ":exerciseId", element: <ExerciseDetailPage /> },
              { path: ":exerciseId/edit", element: <EditExercisePage /> },
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
  localStorage.setItem("persist", "true");

  return (
    <>
      <RouterProvider router={router} />{" "}
    </>
  );
};

export default App;
