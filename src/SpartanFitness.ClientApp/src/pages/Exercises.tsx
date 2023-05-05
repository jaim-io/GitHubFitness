import React, { FormEvent, useState } from "react";
import { Link, useSearchParams } from "react-router-dom";
import ExerciseCard from "../components/ExerciseCard";
import ListBox from "../components/ListBox";
import PageNavigation from "../components/PageNavigation";
import useExercises from "../hooks/useExercises";
import CurrentSearchParams from "../types/CurrentSearchParams";
import SearchBar from "../components/SearchBar";
import NewButton from "../components/NewButton";
import useAuth from "../hooks/useAuth";
import { TbGhost2Filled } from "react-icons/tb";

const DEFAULT_PAGE_NUMBER = 1;
const DEFAULT_PAGE_SIZE = 5;
const SORT_OPTIONS = [
  {
    name: "Newest",
    sort: "created",
    order: "desc",
  },
  {
    name: "Oldest",
    sort: "created",
    order: "asc",
  },
  {
    name: "Name (a-z)",
    sort: "name",
    order: "asc",
  },
  {
    name: "Name (z-a)",
    sort: "name",
    order: "desc",
  },
];

const ExercisesPage = () => {
  const { auth } = useAuth();

  // ---Pagination states---
  const [searchParams, setSearchParams] = useSearchParams();
  const currentParams = new CurrentSearchParams(searchParams);

  const [currentPage, setCurrentPage] = useState<number>(
    currentParams.GetPage(DEFAULT_PAGE_NUMBER),
  );
  const [pageSize, setPageSize] = useState<number>(
    currentParams.GetSize(DEFAULT_PAGE_SIZE),
  );

  const [sortName, setSortName] = useState(
    currentParams.GetSort(SORT_OPTIONS[0].name),
  );
  const [order, setOrder] = useState(
    currentParams.GetOrder(SORT_OPTIONS[0].order),
  );
  
  const [query, setQuery] = useState("");
  // ------------------------

  const [result, isLoading] = useExercises(
    currentPage,
    pageSize,
    SORT_OPTIONS.find((o) => o.name == sortName)?.sort,
    order,
    query,
  );
  const [error, exercisePage] = result.extract();

  const paginate = (page: number) => {
    setCurrentPage(page);

    let params = new URLSearchParams(location.search);
    params.set("p", page.toString());
    params.set("ls", currentParams.GetSize(DEFAULT_PAGE_SIZE).toString());

    setSearchParams(params);
  };

  const handleSort = (value: string) => {
    const option = SORT_OPTIONS.find((o) => o.name == value);
    if (option) {
      setSortName(option.name);
      setOrder(option.order);
      setCurrentPage(DEFAULT_PAGE_NUMBER);

      let params = new URLSearchParams(location.search);
      params.set("p", DEFAULT_PAGE_NUMBER.toString());
      params.set("s", option.sort);
      params.set("o", option.order);

      setSearchParams(params);
    }
  };

  const handleQuerySubmit = (
    event: FormEvent<HTMLFormElement>,
    value: string,
  ) => {
    event.preventDefault();
    setQuery(value);
    setCurrentPage(DEFAULT_PAGE_NUMBER);

    let params = new URLSearchParams(location.search);
    params.set("p", DEFAULT_PAGE_NUMBER.toString());
    params.set("q", value);

    setSearchParams(params);
  };

  return (
    <div className="px-24 pt-6 pb-8 h-full min-h-[90vh]">
      <Link
        to=""
        className="text-xl font-semibold text-[#2f81f7] hover:underline hover:underline-[#2f81f7] max-w-[8rem]"
      >
        All exercises
      </Link>

      <div>
        <ul
          className={`flex justify-center gap-4 items-center ${
            isLoading ? "opacity-60 animate-pulse" : ""
          }`}
        >
          <SearchBar onSubmit={handleQuerySubmit} />
          <ListBox
            selected={sortName}
            options={SORT_OPTIONS.map((o) => o.name)}
            buttonText={"Sort by:"}
            onChange={handleSort}
          />
          {auth.user && <NewButton />}
        </ul>

        <ul className="relative min-h-[10rem]">
          <div
            className={`flex flex-wrap gap-4 justify-center mb-4 pt-6 pb-2${
              isLoading ? "opacity-60 animate-pulse" : ""
            }`}
          >
            {exercisePage &&
              exercisePage.exercises.map((e) => (
                <ExerciseCard exercise={e} key={e.id} />
              ))}
          </div>

          {(isLoading || isLoading == undefined) && (
            <div
              role="status"
              className="absolute -translate-x-1/2 -translate-y-1/2 top-2/4 left-1/2"
            >
              <svg
                aria-hidden="true"
                className="w-8 h-8 mr-2 text-gray-200 animate-spin dark:text-gray-600 fill-[#2f81f7]"
                viewBox="0 0 100 101"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z"
                  fill="currentColor"
                />
                <path
                  d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z"
                  fill="currentFill"
                />
              </svg>
              <span className="sr-only">Loading...</span>
            </div>
          )}
        </ul>

        {exercisePage && exercisePage.exercises.length >= 1 && (
          <PageNavigation
            pageNumber={exercisePage.pageNumber}
            pageCount={exercisePage.pageCount}
            paginate={paginate}
            className={` ${isLoading ? "opacity-60 animate-pulse" : ""}`}
          />
        )}

        {exercisePage && exercisePage.exercises.length === 0 && (
          <p className="flex justify-center items-center">
            No exercises found <TbGhost2Filled className="ml-1" size={20}/>
          </p>
        )}
      </div>
    </div>
  );
};

export default ExercisesPage;
