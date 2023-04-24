import PageNavigationItem from "./PageNavigationItem";

type Props = {
  pageNumber: number;
  pageCount: number;
  paginate: (page: number) => void;
};

const PageNavigation = ({ pageNumber, pageCount, paginate }: Props) => {
  return (
    <div className="flex justify-center">
      <PageNavigationItem
        number={1}
        isActive={pageNumber == 1}
        paginate={paginate}
      />

      {pageCount >= 2 && pageNumber < 4 && (
        <PageNavigationItem
          number={2}
          key={2}
          isActive={pageNumber == 2}
          paginate={paginate}
        />
      )}
      {pageCount >= 3 && pageNumber < 4 && (
        <PageNavigationItem
          number={3}
          key={3}
          isActive={pageNumber == 3}
          paginate={paginate}
        />
      )}
      {pageCount >= 4 && pageNumber < 4 && (
        <PageNavigationItem
          number={4}
          key={4}
          isActive={pageNumber == 4}
          paginate={paginate}
        />
      )}
      {pageCount >= 5 && pageNumber < 4 && (
        <PageNavigationItem
          number={5}
          key={5}
          isActive={pageNumber == 5}
          paginate={paginate}
        />
      )}
      {pageCount >= 6 && pageNumber < 4 && (
        <PageNavigationItem
          number={6}
          key={6}
          isActive={pageNumber == 6}
          paginate={paginate}
        />
      )}

      {pageNumber >= 4 &&
        pageNumber <= pageCount - 4 && [
          <PageNavigationItem
            number={pageNumber - 2}
            key={pageNumber - 2}
            isActive={false}
            paginate={paginate}
          />,
          <PageNavigationItem
            number={pageNumber - 1}
            key={pageNumber - 1}
            isActive={false}
            paginate={paginate}
          />,
          <PageNavigationItem
            number={pageNumber}
            key={pageNumber}
            isActive={true}
            paginate={paginate}
          />,
          <PageNavigationItem
            number={pageNumber + 1}
            key={pageNumber + 1}
            isActive={false}
            paginate={paginate}
          />,
          <PageNavigationItem
            number={pageNumber + 2}
            key={pageNumber + 2}
            isActive={false}
            paginate={paginate}
          />,
        ]}

      {pageNumber <= pageCount - 4 && [
        <PageNavigationItem
          number={pageCount - 5}
          key={pageCount - 5}
          isActive={pageCount - 5 == pageNumber}
          paginate={paginate}
        />,
        <PageNavigationItem
          number={pageCount - 4}
          key={pageCount - 4}
          isActive={pageCount - 4 == pageNumber}
          paginate={paginate}
        />,
        <PageNavigationItem
          number={pageCount - 3}
          key={pageCount - 3}
          isActive={pageCount - 3 == pageNumber}
          paginate={paginate}
        />,
        <PageNavigationItem
          number={pageCount - 2}
          key={pageCount - 2}
          isActive={pageCount - 2 == pageNumber}
          paginate={paginate}
        />,
        <PageNavigationItem
          number={pageCount - 1}
          key={pageCount - 1}
          isActive={pageCount - 1 == pageNumber}
          paginate={paginate}
        />,
      ]}

      {pageCount >= 7 && (
        <PageNavigationItem
          number={pageCount}
          key={pageCount}
          isActive={pageCount == pageNumber}
          paginate={paginate}
        />
      )}
    </div>
  );
};

export default PageNavigation;
