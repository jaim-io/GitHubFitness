import { Fun } from "./Fun";

export type Id<T> = T;
export const Id = <T>(): Fun<T, Id<T>> => Fun((x) => x);
export const mapId = <T1, T2>(f: Fun<T1, T2>): Fun<Id<T1>, Id<T2>> =>
  f.then(Id());
export const joinId = <T>(): Fun<Id<Id<T>>, Id<T>> => Id<T>();
export const unitId = <T>(): Fun<T, Id<T>> => Id<T>();
export const bindId = <T1, T2>(i: Id<T1>, f: Fun<T1, Id<T2>>): Id<T2> =>
  f.then(Id())(i);
