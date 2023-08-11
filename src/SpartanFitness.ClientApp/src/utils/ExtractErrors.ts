// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const extractErrors = (object: any): any[] => {
  return Object.entries(object)
    .map(([, value]) => {
      if (value instanceof Array) {
        return value;
      }
      return [value];
    })
    .flat();
};
