import { useState } from "react";
import { ImCancelCircle } from "react-icons/im";

type Props = {
  onSubmit: (event: React.FormEvent<HTMLFormElement>, value: string) => void;
};

const SearchBar = ({ onSubmit }: Props) => {
  const [value, setValue] = useState("");

  return (
    <form
      onSubmit={(e) => {
        onSubmit(e, value);
        setValue("");
      }}
    >
      <label
        htmlFor="default-search"
        className="mb-2 text-gray-900 sr-only dark:text-white"
      >
        Search
      </label>
      <div className="relative">
        <div className="absolute inset-y-0 left-0 flex items-center pl-3 pointer-events-none">
          <svg
            aria-hidden="true"
            className="w-5 h-5 text-gray-500 dark:text-gray-400"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth="2"
              d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
            ></path>
          </svg>
        </div>
        <input
          type="search"
          id="default-search"
          className="block w-full p-1 pl-10 rounded-lg border border-[#30363d] bg-[#262c31] hover:border-[#8B949E] hover:bg-[#30363D] focus:outline focus:outline-[#2f81f7] focus:shadow-outline search-cancel:hidden placeholder-[#7D8590] overflow-scroll"
          value={value}
          onChange={(e) => setValue(e.target.value)}
          placeholder="Search ..."
          autoComplete="off"
          required
        />
        <button
          type="reset"
          className="absolute top-0 right-0 p-[0.625rem] pl-1 text-sm font-medium text-[#8B949E] rounded-r-lg hover:text-[#2f81f7] z-3"
          onClick={() => setValue("")}
        >
          <ImCancelCircle className="bg-[#262c31]"/>
          <span className="sr-only">Cancel</span>
        </button>
      </div>
    </form>
  );
};

export default SearchBar;
