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

  GetSort = (defaultValue: string): string =>
    this.searchParams.get("s") != undefined
      ? this.searchParams.get("s")!
      : defaultValue;

  GetOrder = (defaultValue: string): string =>
    this.searchParams.get("o") != undefined
      ? this.searchParams.get("o")!
      : defaultValue;
}

export default CurrentSearchParams;
