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
        <button
          className="absolute inset-y-0 left-0 flex items-center pl-3 z-3 text-gray-500 hover:text-blue"
          type="submit"
        >
          <svg
            aria-hidden="true"
            className="w-5 h-5 "
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
        </button>
        <input
          type="search"
          id="default-search"
          className="block w-full p-1 pl-10 pr-7 rounded-lg border border-[rgba(240,246,252,0.1)] bg-[#262c31] hover:border-[#8B949E] hover:bg-gray focus:outline focus:outline-blue focus:shadow-outline search-cancel:hidden placeholder-light-gray overflow-scroll z-1"
          value={value}
          onChange={(e) => setValue(e.target.value)}
          placeholder="Search ..."
          autoComplete="off"
          required
        />
        <button
          type="reset"
          className="absolute top-0 right-[0.125rem] p-[0.625rem] pl-1 text-sm font-medium text-[#8B949E] rounded-r-lg hover:text-blue z-3"
          onClick={() => setValue("")}
        >
          <ImCancelCircle className="bg-[#262c31]" size={14} />
          <span className="sr-only">Cancel</span>
        </button>
      </div>
    </form>
  );
};

export default SearchBar;
