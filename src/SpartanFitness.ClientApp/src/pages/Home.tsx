import { Link } from "react-router-dom";
import useExercisesPage from "../hooks/useExercisesPage";
import LoadingIcon from "../components/icons/LoadingIcon";
import ExerciseCard from "../components/cards/ExerciseCard";
import useWorkoutsPage from "../hooks/useWorkoutsPage";
import WorkoutCard from "../components/cards/WorkoutCard";
import { TbGhost2Filled } from "react-icons/tb";

const HomePage = () => {
  const [exercisesPage, , exercisesPageIsLoading] = useExercisesPage({
    page: 1,
    size: 5,
    sort: "created",
    order: "asc",
  });

  const [workoutsPage, , workoutsPageIsLoading] = useWorkoutsPage({
    page: 1,
    size: 5,
    sort: "created",
    order: "asc",
  });

  // GetQuoteOfTheDay

  return (
    <div className="px-24 pt-6 pb-8 h-full min-h-[90vh]">
      <div className="pt-2">
        <div className="h-40 w-full border-y border-gray rounded-lg flex justify-center items-center">
          <span className="font-mono text-3xl italic">
            The body believes what the mind achieves.
          </span>
        </div>
      </div>

      <div className="pt-8 pb-1">
        <Link
          to="/coaches/all/workouts?p=1&s=created&o=desc"
          className="text-xl font-semibold text-blue hover:underline"
        >
          Newest workouts
        </Link>
        <div className="relative min-h-[10rem]">
          {workoutsPageIsLoading || workoutsPageIsLoading === undefined ? (
            <div
              role="status"
              className="py-5 flex justify-center items-center"
            >
              <LoadingIcon classNames="mr-2 fill-blue text-gray w-8 h-8" />
              <span className="sr-only">Loading...</span>
            </div>
          ) : (
            <div
              className={`flex flex-wrap gap-4 justify-center mb-4 pt-6 pb-2${
                workoutsPageIsLoading ? "opacity-60 animate-pulse" : ""
              }`}
            >
              {workoutsPage && workoutsPage.workouts.length > 0 ? (
                workoutsPage.workouts.map((w) => (
                  <WorkoutCard workout={w} key={w.id} />
                ))
              ) : (
                <p className="flex justify-center items-center">
                  No workouts found{" "}
                  <TbGhost2Filled className="ml-1" size={20} />
                </p>
              )}
            </div>
          )}
        </div>
      </div>

      <div className="bg-gray self-stretch w-full h-[1px] mb-4 mt-4" />

      <div className="pt-4">
        <Link
          to="/exercises?p=1&s=created&o=desc"
          className="text-xl font-semibold text-blue hover:underline"
        >
          Newest exercises
        </Link>

        <div className="relative min-h-[10rem]">
          {exercisesPageIsLoading || exercisesPageIsLoading === undefined ? (
            <div
              role="status"
              className="py-5 flex justify-center items-center"
            >
              <LoadingIcon classNames="mr-2 fill-blue text-gray w-8 h-8" />
              <span className="sr-only">Loading...</span>
            </div>
          ) : (
            <div
              className={`flex flex-wrap gap-4 justify-center mb-4 pt-6 pb-2${
                exercisesPageIsLoading ? "opacity-60 animate-pulse" : ""
              }`}
            >
              {exercisesPage && exercisesPage.exercises.length > 0 ? (
                exercisesPage.exercises.map((e) => (
                  <ExerciseCard exercise={e} key={e.id} />
                ))
              ) : (
                <p className="flex justify-center items-center">
                  No exercises found{" "}
                  <TbGhost2Filled className="ml-1" size={20} />
                </p>
              )}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default HomePage;
