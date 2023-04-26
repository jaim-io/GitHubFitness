import ExerciseCard from "../components/ExerciseCard";
import { Link, useSearchParams } from "react-router-dom";
import { Listbox } from "@headlessui/react";
import useExercises from "../hooks/useExercises";
import PageNavigation from "../components/PageNavigation";
import { useState } from "react";

const DEFAULT_PAGE_NUMBER = 1;
const DEFAULT_PAGE_SIZE = 5;

const ExercisesPage = () => {
  // search ? s -> sort, o -> order, q -> query
  const [searchParams, setSearchParams] = useSearchParams();

  const getCurrentPage = (): number =>
    searchParams.get("page") != undefined
      ? +searchParams.get("page")!
      : DEFAULT_PAGE_NUMBER;

  const getPageSize = (): number =>
    searchParams.get("size") != undefined
      ? +searchParams.get("size")!
      : DEFAULT_PAGE_SIZE;

  const [currentPage, setCurrentPage] = useState<number>(getCurrentPage());
  const [pageSize, setPageSize] = useState<number>(getPageSize());

  const paginate = (page: number) => {
    setCurrentPage(page);
    setSearchParams({ page: page.toString(), size: getPageSize().toString() });
  };

  const [result, isLoading] = useExercises(currentPage, pageSize);
  const [error, page] = result.extract();
  console.log(page);

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
          className={`flex justify-center gap-4 ${
            isLoading ? "opacity-60 animate-pulse" : ""
          }`}
        >
          <p>Search-balk</p>

          <p>sort-by-date</p>

          {/* <Listbox value={}> 
        <Listbox.Button></Listbox.Button>
        </Listbox> */}

          <p>new</p>
        </ul>

        <ul className="relative min-h-[10rem]">
          <div
            className={`flex flex-wrap gap-4 justify-center mb-4 pt-6 pb-2${
              isLoading ? "opacity-60 animate-pulse" : ""
            }`}
          >
            {page &&
              page.exercises.map((e) => (
                <ExerciseCard exercise={e} key={e.id} />
              ))}
          </div>

          {!(isLoading == false) && (
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

        {page && (
          <PageNavigation
            pageNumber={page.pageNumber}
            pageCount={page.pageCount}
            paginate={paginate}
            className={` ${isLoading ? "opacity-60 animate-pulse" : ""}`}
          />
        )}
      </div>
    </div>
  );
};

export default ExercisesPage;
