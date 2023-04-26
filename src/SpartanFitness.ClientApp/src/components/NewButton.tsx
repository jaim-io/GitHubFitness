import { Link } from "react-router-dom";
import { MdOutlineLibraryAdd } from "react-icons/md";

const NewButton = () => {
  return (
    <Link
      to="new"
      className="bg-[#238636] hover:bg-[#2EA043] border border-[#30363d] rounded-lg py-1 px-3 flex justify-center items-center"
    >
      <MdOutlineLibraryAdd className="mr-1" />
      New
    </Link>
  );
};

export default NewButton;
