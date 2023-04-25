import PageNavigationItem from "./PageNavigationItem";

type Props = {
  pageNumber: number;
  pageCount: number;
  paginate: (page: number) => void;
};

const range = (start: number, end: number): number[] => {
  const length = end - start;
  return Array.from({ length }, (_, i) => i + start);
};

const paginationRange = (
  pageNumber: number,
  pageCount: number,
  siblings: number,
) => {
  const totalSiblings = siblings * 2;
  let res: number[];

  if (pageCount == 1) {
    res = [];
  } else if (pageCount == 2) {
    res = [2];
  } else if (pageNumber - siblings <= 1) {
    res = range(
      2,
      pageCount > totalSiblings + 1 ? totalSiblings + 2 + 1 : pageCount,
    );
  } else if (pageNumber + siblings >= pageCount) {
    res = range(
      pageCount - totalSiblings - 1 > 1 ? pageCount - totalSiblings - 1 : 2,
      pageCount,
    );
  } else {
    res = range(pageNumber - siblings, pageNumber + siblings + 1);
  }

  return res;
};

const PageNavigation = ({ pageNumber, pageCount, paginate }: Props) => {
  const siblings = 2;

  const pageRange = paginationRange(pageNumber, pageCount, siblings);

  if (pageNumber < 1) {
    return null;
  }

  return (
    <div className="flex justify-center">
      <button
        onClick={() => paginate(pageNumber - 1)}
        className={`${
          pageNumber - 1 <= 0 ? "cursor-not-allowed text-gray-400" : ""
        }`}
        disabled={pageNumber - 1 <= 0}
      >
        prev
      </button>

      <PageNavigationItem
        number={1}
        isActive={pageNumber == 1}
        paginate={paginate}
      />

      {!(pageRange.length == 0) && !(pageRange[0] <= 2) && (
        <p className="text-xl">...</p>
      )}

      {pageRange.map((n) => (
        <PageNavigationItem
          number={n}
          key={n}
          isActive={pageNumber == n}
          paginate={paginate}
        />
      ))}

      {!(pageRange.length == 0) &&
        !(pageRange[pageRange.length - 1] + 1 >= pageCount) && (
          <p className="text-xl">...</p>
        )}

      {pageCount > pageRange[pageRange.length - 1] && (
        <PageNavigationItem
          number={pageCount}
          key={pageCount}
          isActive={pageCount == pageNumber}
          paginate={paginate}
        />
      )}

      <button
        onClick={() => paginate(pageNumber + 1)}
        className={`${
          pageNumber + 1 > pageCount ? "cursor-not-allowed text-gray-400" : ""
        }`}
        disabled={pageNumber + 1 > pageCount}
      >
        next
      </button>
    </div>
  );
};

export default PageNavigation;
