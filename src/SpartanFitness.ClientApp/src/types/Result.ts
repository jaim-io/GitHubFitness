import Exception from "./Exception";
import { Either, inl as eithInl, inr as eithInr } from "./base/Either";
import { Fun } from "./base/Fun";

export type Result<T> = Either<Exception, T>;

export const inl = <T>(): Fun<Exception, Result<T>> =>
  eithInl<Exception, T>();
export const inr = <T>(): Fun<T, Result<T>> =>
  eithInr<Exception, T>();
