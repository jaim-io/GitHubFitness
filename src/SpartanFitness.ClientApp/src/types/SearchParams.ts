class CurrentSearchParams {
  searchParams: URLSearchParams;

  constructor(searchParams: URLSearchParams) {
    this.searchParams = searchParams;
  }

  GetPage = (defaultValue: number): number =>
    this.searchParams.get("p") != undefined
      ? +this.searchParams.get("p")!
      : defaultValue;

  GetSize = (defaultValue: number): number =>
    this.searchParams.get("ls") != undefined
      ? +this.searchParams.get("ls")!
      : defaultValue;
}

export default CurrentSearchParams;
