import { Tab } from "@headlessui/react";
import {
  Dispatch,
  FormEvent,
  SetStateAction,
  useEffect,
  useState,
} from "react";
import { IconType } from "react-icons";
import { AiOutlineCheck } from "react-icons/ai";
import { RiDeleteBin5Fill } from "react-icons/ri";
import { RxCross1 } from "react-icons/rx";
import { TbGhost2Filled } from "react-icons/tb";
import Exception from "../types/domain/Exception";
import Page from "../types/domain/Page";
import ListBox from "./ListBox";
import SearchBar from "./SearchBar";
import GenericCard from "./cards/GenericCard";
import LoadingIcon from "./icons/LoadingIcon";
import PageNavigation from "./navigation/PageNavigation";
import useAuth from "../hooks/useAuth";

const DEFAULT_PAGE_NUMBER = 1;
const DEFAULT_PAGE_SIZE = 8;
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

type Entity = {
  id: string;
  name: string;
  description: string;
  coachId?: string;
};

type PageArguments = {
  userId: string;
  forceRefreshValue: boolean | undefined;
  page: number | undefined;
  size: number | undefined;
  sort: string | undefined;
  order: string | undefined;
  query: string | undefined;
};

type Props = {
  usePage: ({
    page,
    size,
    sort,
    order,
    query,
    userId,
  }: PageArguments) => [
    Page<Entity> | undefined,
    Exception | undefined,
    boolean,
  ];
  handleUnsave: (
    id: string,
    userId: string,
    onSucces: () => void,
  ) => Promise<void>;
  handleUnsaveRange: (
    ids: string[],
    userId: string,
    onSucces: () => void,
  ) => Promise<void>;
  fetchAllIds: (userId: string) => Promise<string[]>;
  generateUrl: (entity: Entity) => string;
  errorMessage: string;
  iconType: IconType;
  pageState?: PageState;
  setPageState?: Dispatch<SetStateAction<PageState | undefined>>;
};

export type PageState = {
  page: number;
  size: number;
  sortName: string;
  order: string;
  query: string;
};

const UserSavesTabPanel = ({
  usePage,
  handleUnsave,
  handleUnsaveRange,
  fetchAllIds,
  errorMessage,
  generateUrl,
  iconType,
  pageState,
  setPageState,
}: Props) => {
  const { auth } = useAuth();

  const [currentPage, setCurrentPage] = useState<number>(
    pageState?.page ?? DEFAULT_PAGE_NUMBER,
  );
  const [sortName, setSortName] = useState(
    pageState?.sortName ?? SORT_OPTIONS[0].name,
  );
  const [order, setOrder] = useState(pageState?.order ?? SORT_OPTIONS[0].order);
  const [query, setQuery] = useState(pageState?.query ?? "");

  if (setPageState) {
    useEffect(() => {
      setPageState({
        page: currentPage,
        size: DEFAULT_PAGE_SIZE,
        sortName: sortName,
        order: order,
        query: query,
      });
    }, [currentPage, sortName, order, query]);
  }

  const [forceRefreshValue, setForceRefreshValue] = useState(false);

  const [page, , isLoading] = usePage({
    userId: auth.user!.id,
    page: currentPage,
    size: DEFAULT_PAGE_SIZE,
    sort: SORT_OPTIONS.find((o) => o.name == sortName)?.sort,
    order: order,
    query: query,
    forceRefreshValue: forceRefreshValue,
  });

  const handleSort = (value: string) => {
    const option = SORT_OPTIONS.find((o) => o.name == value);
    if (option) {
      setSortName(option.name);
      setOrder(option.order);
      setCurrentPage(DEFAULT_PAGE_NUMBER);
    }
  };

  const handleQuerySubmit = (
    event: FormEvent<HTMLFormElement>,
    value: string,
  ) => {
    event.preventDefault();
    setQuery(value);
    setCurrentPage(DEFAULT_PAGE_NUMBER);
  };

  const [selected, setSelected] = useState<string[]>([]);
  const toggleSelected = (toggledId: string) =>
    setSelected((prev) => {
      if (prev.includes(toggledId)) {
        return prev.filter((id) => id !== toggledId);
      }
      return [...prev, toggledId];
    });
  const isSelected = (id: string) => selected.includes(id);

  const toggleSelectAll = async () => {
    if (selected.length > 0) {
      setSelected([]);
      return;
    }

    const allIds = await fetchAllIds(auth.user!.id);
    setSelected(allIds);
  };

  const updateCurrentPage = () => {
    if (page) {
      if (page.pageNumber > 1) {
        setCurrentPage(1);
      } else {
        setForceRefreshValue((prev) => !prev);
      }
    }
  };

  const handleUnsaveObject = (objectId: string) => {
    handleUnsave(objectId, auth.user!.id, updateCurrentPage);
  };

  const handleUnsaveAll = async () => {
    const allIds = await fetchAllIds(auth.user!.id);
    handleUnsaveRange(allIds, auth.user!.id, updateCurrentPage);
  };

  const handleUnsaveSelected = () => {
    handleUnsaveRange(selected, auth.user!.id, updateCurrentPage);
    setSelected([]);
  };

  return (
    <Tab.Panel className={"pt-4"}>
      <ul
        className={`flex justify-center gap-4 items-center ${
          isLoading ? "opacity-60 animate-pulse" : ""
        }`}
      >
        <SearchBar query={query} onSubmit={handleQuerySubmit} />
        <ListBox
          selected={sortName}
          options={SORT_OPTIONS.map((o) => o.name)}
          buttonText={"Sort by:"}
          onChange={handleSort}
        />
        <button
          type="button"
          className="rounded-lg border border-[rgba(240,246,252,0.1)] bg-[#262c31] hover:border-[#8B949E] hover:bg-gray py-1 px-3 flex justify-center items-center"
          onClick={() => toggleSelectAll()}
        >
          {selected.length > 0 ? "Unselect all" : "Select all"}
        </button>
        {/* Disable button if page.values = 0 || undefined */}
        <button
          className={`text-[#e8473f] bg-gray py-1 px-3 rounded-lg hover:bg-red hover:border-[#f85149] hover:text-white border border-red flex items-center cursor-pointer`}
          type="button"
          onClick={selected.length > 0 ? handleUnsaveSelected : handleUnsaveAll}
        >
          <RiDeleteBin5Fill className="mr-1 flex" />{" "}
          {selected.length > 0 ? "Delete selected" : "Delete all"}
        </button>
      </ul>

      <ul className="relative min-h-[10rem] rounded-xl mt-6">
        <div
          className={`flex flex-wrap gap-4 justify-center mb-4 pb-2 ${
            isLoading ? "opacity-60 animate-pulse" : ""
          }`}
        >
          {page &&
            page.values.map((obj) => (
              <div key={obj.id} className="relative">
                <GenericCard
                  object={{
                    ...obj,
                    url: generateUrl(obj),
                    iconType: iconType,
                  }}
                />
                <div
                  className={`absolute left-0 top-0 w-full h-full flex items-center justify-center pointer-events-none ${
                    selected.length > 0 && !isSelected(obj.id)
                      ? "bg-semi-black opacity-80"
                      : ""
                  }`}
                />
                <button
                  className="absolute right-[3.25rem] top-3 w-8 h-8 flex items-center justify-center rounded-lg border border-[rgba(240,246,252,0.1)] bg-[#262c31] hover:border-[#8B949E] hover:bg-gray cursor-pointer"
                  type="button"
                  onClick={() => toggleSelected(obj.id)}
                >
                  {isSelected(obj.id) ? (
                    <RxCross1 className="fill-white" size={13} />
                  ) : (
                    <AiOutlineCheck className="fill-white" size={13} />
                  )}
                </button>
                <button
                  className="absolute right-3 top-3 w-8 h-8 flex items-center justify-center rounded-lg border border-[rgba(240,246,252,0.1)] bg-[#262c31] hover:border-[#8B949E] hover:bg-gray cursor-pointer"
                  type="button"
                  onClick={() => handleUnsaveObject(obj.id)}
                >
                  <RiDeleteBin5Fill className="fill-white" size={13} />
                </button>
              </div>
            ))}
        </div>

        {(isLoading || isLoading == undefined) && (
          <div role="status" className="flex justify-center items-center">
            <LoadingIcon classNames="mr-2 animate-spin fill-blue text-gray w-8 h-8" />
            <span className="sr-only">Loading...</span>
          </div>
        )}
      </ul>

      {page && page.values.length >= 1 && (
        <PageNavigation
          pageNumber={page.pageNumber}
          pageCount={page.pageCount}
          paginate={setCurrentPage}
          className={` ${isLoading ? "opacity-60 animate-pulse" : ""}`}
        />
      )}

      {page && page.values.length === 0 && (
        <p className="flex justify-center items-center">
          {errorMessage} <TbGhost2Filled className="ml-1" size={20} />
        </p>
      )}
    </Tab.Panel>
  );
};

export default UserSavesTabPanel;
