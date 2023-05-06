import { Id } from "./Id";
import { Fun } from "./Fun";

interface EitherMethods<L, R> {
  then: <R2>(this: Either<L, R>, f: Fun<R, Either<L, R2>>) => Either<L, R2>;
  extract: (this: Either<L, R>) => [L | undefined, R | undefined];
}

const eitherMethods = <L, R>(): EitherMethods<L, R> => ({
  then: function <R2>(
    this: Either<L, R>,
    f: Fun<R, Either<L, R2>>,
  ): Either<L, R2> {
    return mapEither<L, R, L, Either<L, R2>>(Id<L>(), f).then(joinEither())(
      this,
    );
  },
  extract: function (this: Either<L, R>): [L | undefined, R | undefined] {
    const left = this.kind == "Left" ? this.value : undefined;
    const right = this.kind == "Right" ? this.value : undefined;
    return [left, right];
  },
});

export type Either<L, R> = (
  | {
      kind: "Left";
      value: L;
    }
  | {
      kind: "Right";
      value: R;
    }
) &
  EitherMethods<L, R>;

export const inl = <L, R>(): Fun<L, Either<L, R>> =>
  Fun((v) => ({ ...eitherMethods(), kind: "Left", value: v }));
export const inr = <L, R>(): Fun<R, Either<L, R>> =>
  Fun((v) => ({ ...eitherMethods(), kind: "Right", value: v }));

const mapEither = <L1, R1, L2, R2>(
  f: Fun<L1, L2>,
  g: Fun<R1, R2>,
): Fun<Either<L1, R1>, Either<L2, R2>> =>
  Fun((eith) =>
    eith.kind == "Left"
      ? f.then(inl<L2, R2>())(eith.value)
      : g.then(inr<L2, R2>())(eith.value),
  );

const joinEither = <L, R>(): Fun<Either<L, Either<L, R>>, Either<L, R>> =>
  Fun((nestedEither) =>
    nestedEither.kind == "Left"
      ? inl<L, R>()(nestedEither.value)
      : nestedEither.value,
  );

//@ts-ignore
const bindEither = <L, R, R2>(
  eith: Either<L, R>,
  f: Fun<R, Either<L, R2>>,
): Either<L, R2> =>
  mapEither<L, R, L, Either<L, R2>>(Id<L>(), f).then(joinEither())(eith);
