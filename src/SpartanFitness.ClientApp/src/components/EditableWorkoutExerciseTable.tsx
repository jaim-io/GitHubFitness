import { Combobox, Transition } from "@headlessui/react";
import { Fragment, useEffect, useState } from "react";
import { BsExclamationCircle } from "react-icons/bs";
import { HiChevronDown, HiChevronUp, HiChevronUpDown } from "react-icons/hi2";
import Exercise from "../types/domain/Exercise";
import { EXERCISE_TYPES, WorkoutExercise } from "../types/domain/Workout";
import {
  NumberValidatonProps,
  ValidationResult,
  validateMaxReps,
  validateMinReps,
  validateRepsRatio,
  validateSets,
} from "../utils/NumberValidation";

export type WorkoutExerciseWrapper = WorkoutExercise & {
  name: string;
  muscleGroupIds: string[];
  muscleIds: string[];
  isValid: boolean;
};

export const getRandomInt = (max: number): number =>
  Math.floor(Math.random() * max);

export const createDefaultValue = (
  orderNumber: number,
  exercises: Exercise[],
): WorkoutExerciseWrapper => {
  let randomExercise: Exercise;
  if (exercises.length >= orderNumber) {
    randomExercise = exercises[orderNumber - 1];
  } else {
    randomExercise = exercises[getRandomInt(exercises.length)];
  }

  const randomNumbers: number[] = [];
  for (let i = 0; i < 12; i++) {
    randomNumbers.push(getRandomInt(10));
  }
  return {
    id: `00000000-0000-0000-0000-${randomNumbers.join("")}`, // This ID doesn't matter, only used as a key for rendering.
    name: randomExercise.name,
    orderNumber: orderNumber,
    exerciseId: randomExercise.id,
    sets: 1,
    minReps: 1,
    maxReps: 1,
    exerciseType: EXERCISE_TYPES[0],
    isValid: false,
    muscleGroupIds: [],
    muscleIds: [],
  };
};

type Props = {
  exercises: Exercise[];
  workoutExercises: WorkoutExerciseWrapper[];
  setWorkoutExercises: React.Dispatch<
    React.SetStateAction<WorkoutExerciseWrapper[]>
  >;
};

const EditableWorkoutExerciseTable = ({
  exercises,
  workoutExercises,
  setWorkoutExercises,
}: Props) => {
  const onRemove = (orderNumber: number) => {
    setWorkoutExercises((prev) => {
      const result: WorkoutExerciseWrapper[] = [];
      prev.forEach((we) => {
        if (we.orderNumber === orderNumber) {
          return;
        }
        if (we.orderNumber > orderNumber) {
          we.orderNumber--;
        }
        result.push(we);
      });
      return result;
    });
  };

  const onChange = (updatedExercise: WorkoutExerciseWrapper) =>
    setWorkoutExercises((prev) =>
      prev.map((ex) =>
        ex.orderNumber === updatedExercise.orderNumber ? updatedExercise : ex,
      ),
    );

  const shiftDown = (orderNumber: number) =>
    setWorkoutExercises((prev) => {
      if (orderNumber - 1 >= 1) {
        const target = prev.find((e) => e.orderNumber === orderNumber - 1);
        const current = prev.find((e) => e.orderNumber === orderNumber);

        if (target && current) {
          target.orderNumber = target.orderNumber + 1;
          current.orderNumber = current.orderNumber - 1;

          return prev.map((ex) => {
            if (ex.orderNumber === orderNumber - 1) {
              return target;
            }

            if (ex.orderNumber === orderNumber) {
              return current;
            }

            return ex;
          });
        }
      }

      return prev;
    });

  const shiftUp = (orderNumber: number) =>
    setWorkoutExercises((prev) => {
      if (orderNumber + 1 <= Object.values(workoutExercises).length) {
        const current = prev.find((e) => e.orderNumber === orderNumber);
        const target = prev.find((e) => e.orderNumber === orderNumber + 1);

        if (current && target) {
          current.orderNumber = current.orderNumber + 1;
          target.orderNumber = target.orderNumber - 1;

          return prev.map((ex) => {
            if (ex.orderNumber === orderNumber) {
              return target;
            }

            if (ex.orderNumber === orderNumber + 1) {
              return current;
            }

            return ex;
          });
        }
      }

      return prev;
    });

  return (
    <>
      <div className="grid grid-cols-13 gap-1 ">
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
          <Row
            key={ex.id}
            workoutExercise={ex}
            onRemove={onRemove}
            onChange={onChange}
            onShiftDown={shiftDown}
            onShiftUp={shiftUp}
            exercises={exercises}
          />
        ))}
      <button
        type="button"
        className="mt-1 font-bold bg-gray hover:border-hover-gray border border-[rgba(240,246,252,0.1)] rounded-lg px-2 flex justify-center items-center"
        onClick={() =>
          setWorkoutExercises((prev) => [
            ...prev,
            createDefaultValue(workoutExercises.length + 1, exercises),
          ])
        }
      >
        &#43;
      </button>
    </>
  );
};

type RowProps = {
  exercises: Exercise[];
  workoutExercise: WorkoutExerciseWrapper;
  onRemove: (orderNumber: number) => void;
  onChange: (exercise: WorkoutExerciseWrapper) => void;
  onShiftDown: (orderNumber: number) => void;
  onShiftUp: (orderNumber: number) => void;
};

type InputError = {
  id: string;
  errorMsg: string;
};

const Row = ({
  exercises,
  workoutExercise,
  onRemove,
  onChange,
  onShiftDown,
  onShiftUp,
}: RowProps) => {
  let exercise = exercises.find((e) => e.id === workoutExercise.exerciseId);

  // The default value for an exercise name is "";
  if (workoutExercise.name === "" && !exercise) {
    exercise = exercises[getRandomInt(exercises.length)];
  } else if (!exercise) {
    // Raise error
    return <></>;
  }

  const [selectedExercise, setSelectedExercise] = useState(exercise);

  const [name, setName] = useState(workoutExercise.name);
  const [exerciseId, setExerciseId] = useState(workoutExercise.exerciseId);
  const [sets, setSets] = useState(workoutExercise.sets);
  const [minReps, setMinReps] = useState(workoutExercise.minReps);
  const [maxReps, setMaxReps] = useState(workoutExercise.maxReps);
  const [selectedType, setSelectedType] = useState(
    workoutExercise.exerciseType,
  );
  const [muscleIds, setMuscleIds] = useState<string[]>(exercise.muscleIds);
  const [muscleGroupIds, setMuscleGroupIds] = useState<string[]>(
    exercise.muscleGroupIds,
  );

  const setsValidationProps: NumberValidatonProps = {
    minValue: 1,
    maxValue: 51,
  };
  const repsValidationProps: NumberValidatonProps = {
    minValue: 1,
    maxValue: 51,
  };

  // SHOW ERROR? //val RATIo
  const [setsIsValid, setSetsIsValid] = useState(
    validateSets(sets, setsValidationProps).isValid,
  );

  const minRepsValidation = validateMinReps(minReps, repsValidationProps);
  const maxRepsvalidation = validateMaxReps(maxReps, repsValidationProps);
  const ratioValidation = validateRepsRatio(minReps, maxReps);

  const [minRepsIsValid, setMinRepsIsValid] = useState(
    minRepsValidation.isValid && ratioValidation.isValid,
  );
  const [maxRepsIsValid, setMaxRepsIsValid] = useState(
    maxRepsvalidation.isValid && ratioValidation.isValid,
  );

  const [errors, setErrors] = useState<InputError[]>([]);

  const updateErrors = (id: string, validation: ValidationResult) => {
    if (validation.isValid) {
      setErrors((prev) => prev.filter((e) => e.id !== id));
    } else {
      setErrors((prev) => {
        const error = prev.find((e) => e.id === id);
        if (!error) {
          return [...prev, { id: id, errorMsg: validation.errorMsg }];
        }
        return prev;
      });
    }
  };

  const updateMinReps = (value: number): void => {
    const minRepsValidation = validateMinReps(value, repsValidationProps);
    updateErrors("Min-Reps", minRepsValidation);

    if (minReps === maxReps) {
      const maxRepsvalidation = validateMaxReps(value, repsValidationProps);
      updateErrors("Max-Reps", maxRepsvalidation);
      setMaxRepsIsValid(maxRepsvalidation.isValid);
      setMaxReps(value);
    }

    const ratioValidation = validateRepsRatio(
      value,
      minReps === maxReps ? value : maxReps,
    );
    updateErrors("Rep-Ratio", ratioValidation);

    setMinRepsIsValid(minRepsValidation.isValid && ratioValidation.isValid);
    setMinReps(value);
  };

  const updateMaxReps = (value: number): void => {
    const maxRepsvalidation = validateMaxReps(value, repsValidationProps);
    updateErrors("Max-Reps", maxRepsvalidation);

    const ratioValidation = validateRepsRatio(minReps, value);
    updateErrors("Rep-Ratio", ratioValidation);

    setMaxRepsIsValid(maxRepsvalidation.isValid && ratioValidation.isValid);
    setMaxReps(value);
  };

  const [nameQuery, setNameQuery] = useState("");
  const filteredExercises =
    nameQuery === ""
      ? exercises
      : exercises.filter((e) =>
          e.name.toLowerCase().includes(nameQuery.toLowerCase()),
        );

  const [typeQuery, setTypeQuery] = useState("");
  const filteredTypes =
    typeQuery === ""
      ? EXERCISE_TYPES
      : EXERCISE_TYPES.filter((t) =>
          t.toLowerCase().includes(typeQuery.toLowerCase()),
        );

  useEffect(() => {
    onChange({
      ...workoutExercise,
      name: name,
      exerciseId: exerciseId,
      sets: sets,
      minReps: minReps,
      maxReps: maxReps,
      exerciseType: selectedType,
      isValid: setsIsValid && minRepsIsValid && maxRepsIsValid,
      muscleIds: muscleIds,
      muscleGroupIds: muscleGroupIds,
    });
  }, [name, exerciseId, sets, minReps, maxReps, selectedType]);

  return (
    <>
      <div className="grid grid-cols-13 py-1 gap-1">
        {/* Exercise selection */}
        <Combobox
          value={selectedExercise}
          onChange={(exercise) => {
            setSelectedExercise(exercise);
            setName(exercise.name);
            setExerciseId(exercise.id);
            setMuscleIds(exercise.muscleIds);
            setMuscleGroupIds(exercise.muscleGroupIds);
          }}
        >
          <div className="relative col-span-4">
            <div className="relative w-full cursor-default rounded-lg shadow-md">
              <Combobox.Input
                spellCheck="false"
                displayValue={(exercise: Exercise) => exercise.name}
                onChange={(e) => setNameQuery(e.target.value)}
                className="shadow appearance-none border border-gray rounded-lg w-full py-1.5 px-3 bg-black text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline"
              />
              <Combobox.Button className="absolute inset-y-0 right-0 flex items-center pr-2">
                <HiChevronUpDown
                  className="h-5 w-5 text-gray-400"
                  aria-hidden="true"
                />
              </Combobox.Button>
            </div>

            <Transition
              as={Fragment}
              leave="transition ease-in duration-100"
              leaveFrom="opacity-100"
              leaveTo="opacity-0"
              afterLeave={() => setNameQuery("")}
            >
              <Combobox.Options className="z-10 absolute mt-1 max-h-40 w-full overflow-auto bg-black py-1 px-1 text-base shadow-lg sm:text-sm border border-gray rounded-lg">
                {filteredExercises.length === 0 && typeQuery !== "" ? (
                  <div className="relative cursor-default select-none py-2 px-4 text-gray-700">
                    Nothing found.
                  </div>
                ) : (
                  filteredExercises.map((exercise) => (
                    <Combobox.Option
                      key={exercise.id}
                      className={({ active, selected }) =>
                        `flex cursor-pointer select-none py-2 pr-4 text-whiten rounded-lg pl-3 ${
                          selected ? "bg-blue" : ""
                        } ${active && !selected ? "bg-gray" : ""}`
                      }
                      value={exercise}
                    >
                      {({ selected }) => (
                        <>
                          <span
                            className={`block truncate ${
                              selected ? "font-medium" : "font-normal"
                            }`}
                          >
                            {exercise.name}
                          </span>
                        </>
                      )}
                    </Combobox.Option>
                  ))
                )}
              </Combobox.Options>
            </Transition>
          </div>
        </Combobox>

        <input
          value={sets}
          className="col-span-2 shadow appearance-none border border-gray rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black"
          onChange={(e) => {
            const value = Number(e.target.value);
            if (!isNaN(value)) {
              const validation = validateSets(value, setsValidationProps);
              updateErrors("Sets", validation);
              setSetsIsValid(validation.isValid);
              setSets(value);
            }
          }}
        />

        <input
          value={minReps}
          className="col-span-2 shadow appearance-none border border-gray rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black"
          onChange={(e) => {
            const value = Number(e.target.value);
            if (!isNaN(value)) {
              updateMinReps(value);
            }
          }}
        />
        <input
          value={maxReps}
          className="col-span-2 shadow appearance-none border border-gray rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black"
          onChange={(e) => {
            const value = Number(e.target.value);
            if (!isNaN(value)) {
              updateMaxReps(value);
            }
          }}
        />

        {/* Type selection */}
        <Combobox value={selectedType} onChange={setSelectedType}>
          <div className="relative col-span-2">
            <div className="relative w-full cursor-default rounded-lg shadow-md">
              <Combobox.Input
                spellCheck="false"
                onChange={(e) => setTypeQuery(e.target.value)}
                className="shadow appearance-none border border-gray rounded-lg w-full py-1.5 px-3 bg-black text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline"
              />
              <Combobox.Button className="absolute inset-y-0 right-0 flex items-center pr-2">
                <HiChevronUpDown
                  className="h-5 w-5 text-gray-400"
                  aria-hidden="true"
                />
              </Combobox.Button>
            </div>

            <Transition
              as={Fragment}
              leave="transition ease-in duration-100"
              leaveFrom="opacity-100"
              leaveTo="opacity-0"
              afterLeave={() => setTypeQuery("")}
            >
              <Combobox.Options className="z-10 absolute mt-1 max-h-40 w-full overflow-auto bg-black py-1 px-1 text-base shadow-lg sm:text-sm border border-gray rounded-lg">
                {filteredTypes.length === 0 && typeQuery !== "" ? (
                  <div className="relative cursor-default select-none py-2 px-4 text-gray-700">
                    Nothing found.
                  </div>
                ) : (
                  filteredTypes.map((type) => (
                    <Combobox.Option
                      key={type}
                      className={({ active, selected }) =>
                        `flex cursor-pointer select-none py-2 pr-4 text-whiten rounded-lg pl-3 ${
                          selected ? "bg-blue" : ""
                        } ${active && !selected ? "bg-gray" : ""}`
                      }
                      value={type}
                    >
                      {({ selected }) => (
                        <>
                          <span
                            className={`block truncate ${
                              selected ? "font-medium" : "font-normal"
                            }`}
                          >
                            {type}
                          </span>
                        </>
                      )}
                    </Combobox.Option>
                  ))
                )}
              </Combobox.Options>
            </Transition>
          </div>
        </Combobox>
        <div className="relative align-middle pl-1.5">
          <button
            type="button"
            className="font-bold bg-gray hover:border-hover-gray border border-[rgba(240,246,252,0.1)] rounded-lg py-1 px-2 flex justify-center items-center"
            onClick={() => onRemove(workoutExercise.orderNumber)}
          >
            &times;
          </button>
          <button
            className="absolute top-0 right-1 hover:text-blue"
            type="button"
            onClick={() => onShiftDown(workoutExercise.orderNumber)}
          >
            <HiChevronUp />
          </button>
          <button
            className="absolute bottom-0 right-1 hover:text-blue"
            type="button"
            onClick={() => onShiftUp(workoutExercise.orderNumber)}
          >
            <HiChevronDown />
          </button>
        </div>
      </div>
      {errors.length > 0 &&
        errors.map((e) => (
          <p
            key={e.id}
            className="shadow appearance-none border border-red rounded-lg w-full py-1 px-3 text-whiteas bg-black font-medium flex items-center"
          >
            <BsExclamationCircle className="text-red mr-1" size={14} />{" "}
            {e.errorMsg}
          </p>
        ))}
    </>
  );
};

export default EditableWorkoutExerciseTable;

// Change reps
// IRepetitions
//  -> StandardReps i.e. 10
//  -> MindMaxReps i.e. 10-20
//  -> DiffReps i.e. 10, 12, 20
