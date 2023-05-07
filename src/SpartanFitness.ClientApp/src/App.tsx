import { createBrowserRouter, RouterProvider } from "react-router-dom";
import "./App.css";
import LoginPersistance from "./components/LoginPersistance";
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
import axios from "axios";
import Exercise from "./types/domain/Exercise";

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
                loader: async ({ params }) => {
                  try {
                    const response = await axios.get<Exercise>(
                      `${import.meta.env.VITE_API_BASE}/exercises/${
                        params.exerciseId
                      }`,
                      {
                        headers: {
                          Accept: "application/json",
                          Authorization: `bearer ${localStorage.getItem(
                            "token",
                          )}`,
                        },
                      },
                    );

                    return response.data;
                  } catch (err) {
                    if (axios.isAxiosError(err)) {
                      return err.message;
                    } else {
                      return "An unexpected error occurred";
                    }
                  }
                },
              },
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
