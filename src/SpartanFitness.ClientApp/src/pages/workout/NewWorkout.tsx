import axios from "axios";
import { FormEvent, useEffect, useState } from "react";
import { MdFitbit } from "react-icons/md";
import { SiElectron } from "react-icons/si";
import { Link, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import EditableWorkoutExerciseTable, {
  WorkoutExerciseWrapper,
  createDefaultValue as createDefaultWorkoutExercise,
} from "../../components/EditableWorkoutExerciseTable";
import LoadingIcon from "../../components/Icons/LoadingIcon";
import useAuth from "../../hooks/useAuth";
import useExercises from "../../hooks/useExercises";
import useMuscleGroups from "../../hooks/useMuscleGroups";
import useMuscles from "../../hooks/useMuscles";
import Exception from "../../types/domain/Exception";
import Workout, { WorkoutExercise } from "../../types/domain/Workout";
import InputField from "../../components/inputField";
import {
  validateDefaultUrl,
  validateDescription,
  validateName,
} from "../../utils/StringValidations";

const NewWorkoutPage = () => {
  const { auth } = useAuth();
  const coachRole = auth.user!.roles.find((r) => r.name === "Coach")!;

  const navigate = useNavigate();
  const [, setError] = useState<Exception>();

  const [name, setName] = useState("");
  const [isValidName, setIsValidName] = useState(false);

  const [description, setDescription] = useState("");
  const [isValidDescription, setIsValidDescription] = useState(true); // true since the field is optional

  const [image, setImage] = useState("");
  const [isValidImage, setIsValidImage] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const [exercises, , exercisesLoading] = useExercises();
  const [muscles, , musclesLoading] = useMuscles();
  const [muscleGroups, muscleGroupsLoading] = useMuscleGroups();

  const defaultOrderNumbers = Array.from({ length: 5 }, (_, i) => i + 1);
  const [workoutExercises, setWorkoutExercises] = useState<
    WorkoutExerciseWrapper[]
  >([]);

  useEffect(() => {
    if (exercises) {
      const defaultWorkoutExercises = defaultOrderNumbers.map((n) =>
        createDefaultWorkoutExercise(n, exercises),
      );
      setWorkoutExercises(defaultWorkoutExercises);
    }
  }, [exercises]);

  const activeMuscleIds = [
    ...new Set(workoutExercises.map((we) => we.muscleIds).flat()),
  ];
  const activeMuscleGroupIds = [
    ...new Set(workoutExercises.map((we) => we.muscleGroupIds).flat()),
  ];

  const activeMuscles = muscles
    ? activeMuscleIds.map((id) => muscles.find((m) => m.id === id)!)
    : undefined;
  const activeMuscleGroups = muscleGroups
    ? activeMuscleGroupIds.map((id) => muscleGroups.find((m) => m.id === id)!)
    : undefined;

  const isValidForm = isValidName && isValidDescription && isValidImage;

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!isValidForm) {
      return;
    }

    setIsLoading(true);

    try {
      await axios
        .post<Workout>(
          `${import.meta.env.VITE_API_BASE}/coaches/${
            coachRole.id
          }/workouts/create`,
          {
            name: name,
            description: description,
            workoutExercises: workoutExercises.map(
              (we) => we as WorkoutExercise,
            ),
            image: image,
          },
          {
            headers: {
              "Content-Type": "application/json",
              Authorization: `bearer ${localStorage.getItem("token")}`,
            },
          },
        )
        .then((res) => {
          toast.success("Workout has been created", {
            toastId: "workout-created",
            position: "bottom-right",
            autoClose: 5000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
            theme: "colored",
          });

          setIsLoading(false);

          navigate(`/coaches/${res.data.coachId}/workouts/${res.data.id}`);
        })
        .catch((err) => {
          setIsLoading(false);
          setError({
            message: err.message,
            code: err.code,
          });
          toast.error(
            err.code == "ERR_NETWORK"
              ? "Unable to reach the server"
              : err.response.statusText,
            {
              toastId: err.code,
              position: "bottom-right",
              autoClose: 5000,
              hideProgressBar: false,
              closeOnClick: true,
              pauseOnHover: true,
              draggable: true,
              progress: undefined,
              theme: "colored",
            },
          );
        });
    } catch {
      /* empty */
    }
  };

  return (
    <div className="flex justify-center px-24 pt-6 pb-8 h-full min-h-[90vh]">
      <div className="w-[52rem]">
        <h1 className="text-2xl mb-4">Create a new workout</h1>

        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <InputField
              value={name}
              onChange={setName}
              placeholder="Push"
              label="Exercise name *"
              validator={validateName}
              validationProps={{ minLength: 0, maxLength: 100 }}
              setIsValid={setIsValidName}
            />
          </div>

          <div className="mb-4">
            <InputField
              value={description}
              onChange={setDescription}
              placeholder="A workout which involves chest, delts and triceps..."
              label={
                <span>
                  Description
                  <span className="ml-1 text-light-gray text-sm">
                    (optional)
                  </span>
                </span>
              }
              validator={validateDescription}
              validationProps={{ maxLength: 2048 }}
              setIsValid={setIsValidDescription}
            />
          </div>

          <div className="mb-4">
            <InputField
              value={image}
              onChange={setImage}
              placeholder="https://google.com/exercise-image"
              label={"Image URL *"}
              validator={validateDefaultUrl}
              validationProps={{ maxLength: 2048 }}
              setIsValid={setIsValidImage}
            />
          </div>
          <div className="bg-gray self-stretch w-full h-[1px] mb-4 mt-6" />

          <div className="mb-4">
            <label className="block text-white mb-2 ml-1">Exercises</label>
            {exercisesLoading || exercisesLoading == undefined ? (
              <div className="flex items-center justify-center animate-pulse">
                <LoadingIcon classNames="mr-2 text-white fill-white w-5 h-5" />
                <p>Exercises are loading...</p>
              </div>
            ) : (
              <EditableWorkoutExerciseTable
                exercises={exercises ?? []}
                workoutExercises={workoutExercises}
                setWorkoutExercises={setWorkoutExercises}
              />
            )}
          </div>
          <div className="bg-gray self-stretch w-full h-[1px] mb-4 mt-6" />

          <div className="mb-4">
            <label className="block text-white mb-2 ml-1">
              Muscles used in the exercises
            </label>
            {!musclesLoading ? (
              activeMuscleGroups ? (
                <div className="flex flex-wrap">
                  {activeMuscleGroups.length > 0 ? (
                    activeMuscleGroups.map((mg) => (
                      <Link
                        key={mg.id}
                        className="rounded-full border border-[rgba(240,246,252,0.1)] mr-2 px-2 py-1 mb-2 hover:border-hover-gray flex items-center"
                        to={`/muscles/${mg.id}`}
                      >
                        <MdFitbit className="mr-1" />
                        {mg.name}
                      </Link>
                    ))
                  ) : (
                    <p className="ml-1">No muscles specified</p>
                  )}
                </div>
              ) : (
                <span className="ml-1">
                  An error occured while loading the muscles.
                </span>
              )
            ) : (
              <div
                role="status"
                className="py-2 flex justify-center items-center"
              >
                <LoadingIcon classNames="mr-2 fill-blue text-gray w-6 h-6" />
                <span className="sr-only">Loading...</span>
              </div>
            )}
          </div>
          <div className="bg-gray self-stretch w-full h-[1px] mb-4 mt-6" />

          <div className="mb-4">
            <label className="block text-white mb-2 ml-1">
              Muscle groups used in the exercises
            </label>
            {!muscleGroupsLoading ? (
              activeMuscles ? (
                <div className="flex flex-wrap">
                  {activeMuscles.length > 0 ? (
                    activeMuscles.map((m) => (
                      <Link
                        key={m.id}
                        className="rounded-full border border-[rgba(240,246,252,0.1)] mr-2 px-2 py-1 mb-2 hover:border-hover-gray flex items-center"
                        to={`/muscles/${m.id}`}
                      >
                        <SiElectron className="mr-1" />
                        {m.name}
                      </Link>
                    ))
                  ) : (
                    <p className="ml-1">No muscles specified</p>
                  )}
                </div>
              ) : (
                <span className="ml-1">
                  An error occured while loading the muscles.
                </span>
              )
            ) : (
              <div
                role="status"
                className="py-2 flex justify-center items-center"
              >
                <LoadingIcon classNames="mr-2 fill-blue text-gray w-6 h-6" />
                <span className="sr-only">Loading...</span>
              </div>
            )}
          </div>
          <div className="bg-gray self-stretch w-full h-[1px] mb-4 mt-6" />

          <div className="w-full relative mt-6">
            <Link
              className="absolute left-0 bg-gray hover:border-hover-gray border border-[rgba(240,246,252,0.1)] rounded-lg py-1 px-3 flex justify-center items-center"
              to=".."
            >
              Cancel
            </Link>
            <button
              className="absolute right-0 bg-dark-green hover:bg-light-green border border-[rgba(240,246,252,0.1)] rounded-lg py-1 px-3 flex justify-center items-center"
              type="submit"
            >
              {!isLoading && <p>Create workout</p>}
              {(isLoading || isLoading == undefined) && (
                <div className="flex items-center justify-center animate-pulse">
                  <LoadingIcon classNames="mr-2 text-white fill-white w-4 h-4" />
                  <p>Creating...</p>
                </div>
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default NewWorkoutPage;
