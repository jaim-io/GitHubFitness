class SearchParamsFactory {
  static Create = (
    page: number | undefined,
    size: number | undefined,
    sort?: string | undefined,
    order?: string | undefined,
    query?: string | undefined,
  ): URLSearchParams => {
    const params = new URLSearchParams({});

    if (page) {
      params.set("p", page.toString());
    }

    if (size) {
      params.set("ls", size.toString());
    }

    if (sort) {
      params.set("s", sort!);
    }

    if (order) {
      params.set("o", order!);
    }

    if (query) {
      params.set("q", query!);
    }

    return params;
  };
}

export default SearchParamsFactory;
