export type Fun<T1, T2> = {
  (_: T1): T2;
  then: <T3>(g: Fun<T2, T3>) => Fun<T1, T3>;
  repeat: (times: number) => Fun<T1, T1>;
  repeatUntil: (condition: Fun<T1, boolean>) => Fun<T1, T1>;
};

export const Fun = function <T1, T2>(f: (_: T1) => T2): Fun<T1, T2> {
  const wrapper = f as Fun<T1, T2>;

  wrapper.then = function <T3>(this: Fun<T1, T2>, g: Fun<T2, T3>): Fun<T1, T3> {
    return Fun((x) => g(this(x)));
  };

  wrapper.repeat = function (this: Fun<T1, T1>, times: number): Fun<T1, T1> {
    return times > 0 ? this.then(this.repeat(times - 1)) : Fun((x) => x);
  };

  wrapper.repeatUntil = function (
    this: Fun<T1, T1>,
    condition: Fun<T1, boolean>,
  ): Fun<T1, T1> {
    return Fun((x) =>
      condition(x) ? x : this.then(this.repeatUntil(condition))(x),
    );
  };

  return wrapper;
};
