import { Either, inl, inr } from "../base/Either";
import { Fun } from "../base/Fun";
import Exception from "./Exception";

export type Result<T> = Either<Exception, T>;

export const createException = <T>(): Fun<Exception, Result<T>> =>
  inl<Exception, T>();
export const createValue = <T>(): Fun<T, Result<T>> => inr<Exception, T>();
