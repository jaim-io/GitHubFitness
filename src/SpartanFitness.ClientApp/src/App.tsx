import { createBrowserRouter, RouterProvider } from "react-router-dom";
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

const router = createBrowserRouter([
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
]);

const App = () => {
  const { user } = useContext(AuthContext);

  useEffect(() => {
    if (user != null && user.id != "") {
      toast.success("Login succesful", {
        toastId: "user-login",
        position: "bottom-right",
        autoClose: 5000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
        theme: "colored",
      });
    }
  }, [user]);

  return (
    <>
      <RouterProvider router={router} />{" "}
    </>
  );
};

export default App;
