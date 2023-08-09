import { FormEvent, useState } from "react";
import { TbGhost2Filled } from "react-icons/tb";
import { Link, useSearchParams } from "react-router-dom";
import ExerciseCard from "../../components/cards/ExerciseCard";
import LoadingIcon from "../../components/icons/LoadingIcon";
import ListBox from "../../components/ListBox";
import NewButton from "../../components/NewButton";
import PageNavigation from "../../components/navigation/PageNavigation";
import SearchBar from "../../components/SearchBar";
import useAuth from "../../hooks/useAuth";
import useExercisesPage from "../../hooks/useExercisesPage";
import CurrentSearchParams from "../../utils/CurrentSearchParams";

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

  const isCoach =
    auth.user?.roles.find((r) => r.name === "Coach") !== undefined;
  const isAdmin =
    auth.user?.roles.find((r) => r.name === "Administrator") !== undefined;

  // ---Pagination states---
  const [searchParams, setSearchParams] = useSearchParams();
  const currentParams = new CurrentSearchParams(searchParams);

  const [currentPage, setCurrentPage] = useState<number>(
    currentParams.GetPage(DEFAULT_PAGE_NUMBER),
  );
  const [pageSize] = useState<number>(currentParams.GetSize(DEFAULT_PAGE_SIZE));

  const [sortName, setSortName] = useState(
    currentParams.GetSort(SORT_OPTIONS[0].name),
  );
  const [order, setOrder] = useState(
    currentParams.GetOrder(SORT_OPTIONS[0].order),
  );

  const [query, setQuery] = useState("");
  // ------------------------

  const [exercisePage, , isLoading] = useExercisesPage({
    page: currentPage,
    size: pageSize,
    sort: SORT_OPTIONS.find((o) => o.name == sortName)?.sort,
    order: order,
    query: query,
  });

  const paginate = (page: number) => {
    setCurrentPage(page);

    const params = new URLSearchParams(location.search);
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

      const params = new URLSearchParams(location.search);
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

    const params = new URLSearchParams(location.search);
    params.set("p", DEFAULT_PAGE_NUMBER.toString());
    params.set("q", value);

    setSearchParams(params);
  };

  return (
    <div className="px-24 pt-6 pb-8 h-full min-h-[90vh]">
      <Link
        to=""
        className="text-xl font-semibold text-blue hover:underline hover:underline-blue max-w-[8rem]"
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
          {auth.user && (isCoach || isAdmin) && <NewButton />}
        </ul>

        <ul className="relative min-h-[10rem]">
          <div
            className={`flex flex-wrap gap-4 justify-center mb-4 pt-6 pb-2${
              isLoading ? "opacity-60 animate-pulse" : ""
            }`}
          >
            {exercisePage &&
              exercisePage.values.map((e) => (
                <ExerciseCard exercise={e} key={e.id} />
              ))}
          </div>

          {(isLoading || isLoading == undefined) && (
            <div role="status" className="flex justify-center items-center">
              <LoadingIcon classNames="mr-2 animate-spin fill-blue text-gray w-8 h-8" />
              <span className="sr-only">Loading...</span>
            </div>
          )}
        </ul>

        {exercisePage && exercisePage.values.length >= 1 && (
          <PageNavigation
            pageNumber={exercisePage.pageNumber}
            pageCount={exercisePage.pageCount}
            paginate={paginate}
            className={` ${isLoading ? "opacity-60 animate-pulse" : ""}`}
          />
        )}

        {exercisePage && exercisePage.values.length === 0 && (
          <p className="flex justify-center items-center">
            No exercises found <TbGhost2Filled className="ml-1" size={20} />
          </p>
        )}
      </div>
    </div>
  );
};

export default ExercisesPage;
