import { NavLink } from "react-router-dom";

type Props = {
  number: number;
  isActive: boolean;
  paginate: (page: number) => void;
};

const PageNavigationItem = ({ number, isActive, paginate }: Props) => {
  return (
    <button
      onClick={() => paginate(number)}
      className={`px-[calc(0.8rem)] py-[0.3rem] rounded-lg mr-1 ${
        isActive ? "bg-blue" : ""
      }`}
    >
      {number}
    </button>
  );
};

export default PageNavigationItem;
