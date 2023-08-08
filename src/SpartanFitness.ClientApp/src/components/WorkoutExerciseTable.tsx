import { WorkoutExercise } from "../types/domain/Workout";

export type WorkoutExerciseWrapper = WorkoutExercise & {
  name: string;
};

type Props = {
  workoutExercises: WorkoutExerciseWrapper[];
};

const WorkoutExerciseTable = ({ workoutExercises }: Props) => {
  return (
    <>
      <div className="grid grid-cols-12 gap-1 ">
        <span className="border border-x-0 border-t-0 border-blue col-span-4 mx-1.5">
          Exercise
        </span>
        <span className="border border-x-0 border-t-0 border-blue col-span-2 mx-1.5">
          Sets
        </span>
        <span className="border border-x-0 border-t-0 border-blue col-span-2 mx-1.5">
          Reps-min
        </span>
        <span className="border border-x-0 border-t-0 border-blue col-span-2 mx-1.5">
          Reps-max
        </span>
        <span className="border border-x-0 border-t-0 border-blue col-span-2 mx-1.5">
          Type
        </span>
        <></>
      </div>

      {workoutExercises
        .sort((e1, e2) => (e1.orderNumber < e2.orderNumber ? -1 : 1))
        .map((ex) => (
          <Row key={ex.id} workoutExercise={ex} />
        ))}
    </>
  );
};

type RowProps = {
  workoutExercise: WorkoutExerciseWrapper;
};

const Row = ({ workoutExercise }: RowProps) => {
  return (
    <div className="grid grid-cols-12 py-[0.125rem] gap-1">
      <span className="col-span-4 shadow appearance-none border border-gray rounded-lg w-full py-1.5 px-3 bg-black text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline cursor-default">
        {workoutExercise.name}
      </span>

      <span className="col-span-2 shadow appearance-none border border-gray rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black">
        {workoutExercise.sets}
      </span>

      <span className="col-span-2 shadow appearance-none border border-gray rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black">
        {workoutExercise.minReps}
      </span>

      <span className="col-span-2 shadow appearance-none border border-gray rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black">
        {workoutExercise.maxReps}
      </span>

      <span className="col-span-2 shadow appearance-none border border-gray rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black">
        {workoutExercise.exerciseType}
      </span>
    </div>
  );
};

export default WorkoutExerciseTable;

// Change reps
// IRepetitions
//  -> StandardReps i.e. 10
//  -> MindMaxReps i.e. 10-20
//  -> DiffReps i.e. 10, 12, 20
