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

const NewWorkoutPage = () => {
  const { auth } = useAuth();
  const coachRole = auth.user!.roles.find((r) => r.name === "Coach")!;

  const navigate = useNavigate();
  const [, setError] = useState<Exception>();

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [image, setImage] = useState("");
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

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

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

          navigate(
            `${import.meta.env.VITE_API_BASE}/coaches/${
              res.data.coachId
            }/workouts/${res.data.id}`,
          );
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
            <label className="block text-white mb-2 ml-1">Workout name *</label>
            <input
              className="shadow appearance-none border border-gray hover:border-hover-gray rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black"
              id="workout-name"
              type="text"
              placeholder="Barbell bench press"
              value={name}
              onChange={(e) => setName(e.target.value)}
              required
              autoComplete="off"
            />
          </div>
          <div className="mb-4">
            <label className="flex text-white mb-2 ml-1 items-center">
              Description
              <p className="ml-1 text-light-gray text-sm">(optional)</p>
            </label>
            <input
              className="shadow appearance-none border border-gray hover:border-hover-gray rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black"
              id="description"
              type="text"
              placeholder="A chest workout which involves chest, delts and triceps..."
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              autoComplete="off"
            />
          </div>
          <div className="mb-4">
            <label className="block text-white mb-2 ml-1">Image URL *</label>
            <input
              className="shadow appearance-none border border-gray hover:border-hover-gray rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black"
              type="text"
              placeholder="https://google.com/workout-image"
              value={image}
              onChange={(e) => setImage(e.target.value)}
              required
              autoComplete="off"
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
