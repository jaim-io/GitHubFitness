import { Link, useParams } from "react-router-dom";

const ExerciseDetailPage = () => {
  const params = useParams();

  return (
    <>
      <h1>Exercise details</h1>
      <p>{params.exerciseId}</p>
      <p><Link to=".." relative="path">Back</Link></p>
    </>
  );
};

export default ExerciseDetailPage;
