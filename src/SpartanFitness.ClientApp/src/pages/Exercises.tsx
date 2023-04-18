import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Exercise from "../types/Exercise";

const ExercisesPage = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [exercises, setExercises] = useState<Exercise[]>();
  const [error, setError] = useState<string>();

  useEffect(() => {
    const fetchExercises = async () => {
      setIsLoading(true);

      await fetch(`${import.meta.env.VITE_API_BASE}/exercises`, {
        headers: {
          Accept: "application/json",
          Authorization: `bearer ${localStorage.getItem("token")}`,
        },
      })
        .then((res) => {
          if (!res.ok) {
            setError(res.statusText);
          }
          return res.json();
        })
        .then((res: Exercise[]) => setExercises(res));

      setIsLoading(false);
    };

    fetchExercises();
  }, []);

  return (
    <>
      <h1>The exercises page</h1>
      <ul>
        {exercises &&
          exercises.map((e) => (
            <li key={e.id}>
              <Link to={e.id}>{e.name}</Link>
            </li>
          ))}
      </ul>
    </>
  );
};

export default ExercisesPage;
