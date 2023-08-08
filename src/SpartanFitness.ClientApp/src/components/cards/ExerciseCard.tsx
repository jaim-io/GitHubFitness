import { Link } from "react-router-dom";
import Exercise from "../../types/domain/Exercise";
import { BiDumbbell } from "react-icons/bi";

type Props = {
  exercise: Exercise;
};

const ExerciseCard = ({ exercise }: Props) => {
  return (
    <div className="px-10 pt-6 pb-6 border border-gray w-[24rem] rounded-lg">
      <Link
        to={`/exercises/${exercise.id}`}
        className="text-blue hover:underline hover:underline-blue font-semibold flex items-center"
      >
        <BiDumbbell className="mr-1 text-light-gray" size={16} />
        {exercise.name}
      </Link>
      <div className="pb-2"></div>
      <div className="w-full border border-gray rounded-lg" />
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
        to={`/exercises/${exercise.id}`}
        className="bg-dark-green hover:bg-light-green text-white py-1.5 px-4 rounded-lg focus:outline-none focus:shadow-outline w-full block text-center mt-3 text-sm border border-gray"
      >
        View
      </Link>
    </div>
  );
};

export default ExerciseCard;
