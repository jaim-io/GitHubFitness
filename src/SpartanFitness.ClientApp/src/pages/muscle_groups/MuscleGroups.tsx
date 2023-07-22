import { FormEvent, useState } from "react";
import { TbGhost2Filled } from "react-icons/tb";
import { Link, useSearchParams } from "react-router-dom";
import LoadingIcon from "../../components/Icons/LoadingIcon";
import ListBox from "../../components/ListBox";
import MuscleGroupCard from "../../components/MuscleGroupCard";
import PageNavigation from "../../components/PageNavigation";
import SearchBar from "../../components/SearchBar";
import useMuscleGroupsPage from "../../hooks/useMuscleGroupsPage";
import CurrentSearchParams from "../../types/CurrentSearchParams";

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

const MuscleGroupsPage = () => {
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

  const [muscleGroupsPage, , isLoading] = useMuscleGroupsPage(
    currentPage,
    pageSize,
    SORT_OPTIONS.find((o) => o.name == sortName)?.sort,
    order,
    query,
  );

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
        All muscle groups
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
        </ul>

        <ul className="relative min-h-[10rem]">
          <div
            className={`flex flex-wrap gap-4 justify-center mb-4 pt-6 pb-2${
              isLoading ? "opacity-60 animate-pulse" : ""
            }`}
          >
            {muscleGroupsPage &&
              muscleGroupsPage.muscleGroups.map((mg) => (
                <MuscleGroupCard muscleGroup={mg} key={mg.id} />
              ))}
          </div>

          {(isLoading || isLoading == undefined) && (
            <div role="status" className="flex justify-center items-center">
              <LoadingIcon classNames="mr-2 animate-spin fill-blue text-gray w-8 h-8" />
              <span className="sr-only">Loading...</span>
            </div>
          )}
        </ul>

        {muscleGroupsPage && muscleGroupsPage.muscleGroups.length >= 1 && (
          <PageNavigation
            pageNumber={muscleGroupsPage.pageNumber}
            pageCount={muscleGroupsPage.pageCount}
            paginate={paginate}
            className={` ${isLoading ? "opacity-60 animate-pulse" : ""}`}
          />
        )}

        {muscleGroupsPage && muscleGroupsPage.muscleGroups.length === 0 && (
          <p className="flex justify-center items-center">
            No muscle groups found <TbGhost2Filled className="ml-1" size={20} />
          </p>
        )}
      </div>
    </div>
  );
};

export default MuscleGroupsPage;
