import { Link } from "react-router-dom";
import Exercise from "../types/domain/Exercise";

type Props = {
  exercise: Exercise;
};

const ExerciseCard = ({ exercise }: Props) => {
  return (
    <div className="px-10 pt-6 pb-6 border border-[#30363d] w-[24rem] rounded-lg">
        <Link
          to={exercise.id}
          className="text-[#2f81f7] hover:underline hover:underline-[#2f81f7] font-semibold"
        >
          {exercise.name}
        </Link>
        <div className="pb-2"></div>
        <div className="w-full border border-[#30363d]" />
        <p
          className="pt-2 text-sm"
          style={{
            display: "-webkit-box",
            WebkitLineClamp: 6,
            WebkitBoxOrient: "vertical",
            overflow: "hidden",
            textOverflow: "ellipsis",
          }}
        >
          {exercise.description}
        </p>
        <Link
          to={exercise.id}
          className="bg-[#238636] hover:bg-[#2EA043] text-white py-1.5 px-4 rounded-lg focus:outline-none focus:shadow-outline w-full block text-center mt-3 text-sm"
        >
          View
        </Link>
    </div>
  );
};

export default ExerciseCard;
