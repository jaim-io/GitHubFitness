type Page<T> = {
  values: T[];
  pageNumber: number;
  pageCount: number;
};
export default Page;
