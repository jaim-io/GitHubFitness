import { ToastContainer, toast } from "react-toastify";
import ExerciseCard from "../components/ExerciseCard";
import { Link, useParams, useSearchParams } from "react-router-dom";
import { Listbox } from "@headlessui/react";
import useExercises from "../hooks/useExercises";
import PageNavigation from "../components/PageNavigation";
import { useState } from "react";

const DEFAULT_PAGE_NUMBER = 1;
const DEFAULT_PAGE_SIZE = 1;

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

  const result = useExercises(currentPage, pageSize);
  const [_, page] = result.extract();

  return (
    <div className="px-24 pt-6 pb-8">
      <Link
        to=""
        className="text-xl font-semibold text-[#2f81f7] hover:underline hover:underline-[#2f81f7] max-w-[8rem]"
      >
        All exercises
      </Link>

      <ul className="flex justify-center gap-4">
        <p>Search-balk</p>

        <p>sort-by-date</p>

        {/* <Listbox value={}> 
        <Listbox.Button></Listbox.Button>
        </Listbox> */}

        <p>new</p>
      </ul>

      <ul className="flex flex-wrap gap-4 justify-center mb-4 pt-6">
        {page &&
          page.exercises.map((e) => <ExerciseCard exercise={e} key={e.id} />)}
      </ul>

      <div>
        {page && (
          <PageNavigation
            pageNumber={page.pageNumber}
            pageCount={page.pageCount}
            paginate={paginate}
          />
        )}
      </div>

      <ToastContainer
        position="bottom-right"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
        theme="colored"
      />
    </div>
  );
};

export default ExercisesPage;
