import { Link } from "react-router-dom";
import { MdOutlineLibraryAdd } from "react-icons/md";

const NewButton = () => {
  return (
    <Link
      to="new"
      className="bg-dark-green hover:bg-light-green border border-gray rounded-lg py-1 px-3 flex justify-center items-center"
    >
      <MdOutlineLibraryAdd className="mr-1" />
      New
    </Link>
  );
};

export default NewButton;
