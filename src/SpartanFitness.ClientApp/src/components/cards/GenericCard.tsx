import { IconType } from "react-icons/lib";
import { Link } from "react-router-dom";

type Props = {
  object: {
    id: string;
    name: string;
    description: string;
    url: string;
    iconType: IconType;
  };
};

const GenericCard = ({ object }: Props) => {
  return (
    <div className="px-10 pt-6 pb-6 border border-gray w-[24rem] rounded-lg bg-semi-black">
      <Link
        to={object.url}
        className="text-blue hover:underline hover:underline-blue font-semibold flex items-center"
      >
        <object.iconType className="mr-1 text-light-gray" size={16} />
        {object.name}
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
        {object.description}
      </p>
      <Link
        to={object.url}
        className="bg-dark-green hover:bg-light-green text-white py-1.5 px-4 rounded-lg focus:outline-none focus:shadow-outline w-full block text-center mt-3 text-sm border border-gray"
      >
        View
      </Link>
    </div>
  );
};

export default GenericCard;
