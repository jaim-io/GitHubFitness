import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import Exercise from "../types/Exercise";

const ExerciseDetailPage = () => {
  const params = useParams();
  const [isLoading, setIsLoading] = useState(false);
  const [exercise, setExercise] = useState<Exercise>();
  const [error, setError] = useState<string>();

  useEffect(() => {
    const fetchExercise = async () => {
      setIsLoading(true);

      await fetch(
        `${import.meta.env.VITE_API_BASE}/exercises/${params.exerciseId}`,
        {
          headers: {
            Accept: "application/json",
            Authorization: `bearer ${localStorage.getItem("token")}`,
          },
        },
      )
        .then((res) => {
          if (!res.ok) {
            setError(res.statusText);
          }
          return res.json();
        })
        .then((res: Exercise) => setExercise(res));

      setIsLoading(false);
    };

    fetchExercise();
  }, []);

  return (
    <>
      <h1>Exercise details</h1>
      {exercise && <p>{exercise.name}</p>}
      <p>
        <Link to=".." relative="path">
          Back
        </Link>
      </p>
    </>
  );
};

export default ExerciseDetailPage;
