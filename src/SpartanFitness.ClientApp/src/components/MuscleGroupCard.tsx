import { Link } from "react-router-dom";
import MuscleGroup from "../types/domain/MuscleGroup";
import { MdFitbit } from "react-icons/md";

type Props = {
  muscleGroup: MuscleGroup;
};

const MuscleGroupCard = ({ muscleGroup }: Props) => {
  return (
    <div className="px-10 pt-6 pb-6 border border-gray w-[24rem] rounded-lg">
      <Link
        to={muscleGroup.id}
        className="text-blue hover:underline hover:underline-blue font-semibold flex items-center"
      >
        <MdFitbit className="mr-1 text-light-gray" size={16} />
        {muscleGroup.name}
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
        {muscleGroup.description}
      </p>
      <Link
        to={`/muscle-groups/${muscleGroup.id}`}
        className="bg-dark-green hover:bg-light-green text-white py-1.5 px-4 rounded-lg focus:outline-none focus:shadow-outline w-full block text-center mt-3 text-sm border border-gray"
      >
        View
      </Link>
    </div>
  );
};

export default MuscleGroupCard;
