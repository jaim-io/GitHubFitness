import { Tab } from "@headlessui/react";
import axios, { AxiosError } from "axios";
import { Dispatch, SetStateAction, useState } from "react";
import { BiDumbbell } from "react-icons/bi";
import { toast } from "react-toastify";
import UserSavesTabPanel, {
  PageState,
} from "../../components/UserSaveTabContent";
import useSavedExercisesPage from "../../hooks/useSavedExercisesPage";

const TABS = ["Muscles", "Muscle Groups", "Exercises", "Workouts"];
const USER_ENDPOINT = `${import.meta.env.VITE_API_BASE}/users`;

const handleError = (err: AxiosError) =>
  toast.error(
    err.code == "ERR_NETWORK"
      ? "Unable to reach the server"
      : err.response?.statusText,
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
const handleSucces = (message: string, toastId: string) => {
  toast.success(message, {
    toastId: toastId,
    position: "bottom-right",
    autoClose: 5000,
    hideProgressBar: false,
    closeOnClick: true,
    pauseOnHover: true,
    draggable: true,
    progress: undefined,
    theme: "colored",
  });
};
const handleUnsave = async (
  url: string,
  succesMessage: string,
  toastId: string,
  forceRefresh: Dispatch<SetStateAction<boolean>>,
) => {
  await axios
    .delete(url, {
      headers: {
        Accept: "application/json",
        Authorization: `bearer ${localStorage.getItem("token")}`,
      },
    })
    .then(() => {
      handleSucces(succesMessage, toastId);
      forceRefresh((prev) => !prev);
    })
    .catch((err) => handleError(err));
};
const createQueryString = (ids: string[]): string => {
  const params: string[] = [];

  ids.forEach((id) => params.push(`id=${id}`));

  const queryString = `?${params.join("&")}`;

  return ids.length == 0 ? "" : queryString;
};

const unSave =
  (routeArgs: RouteArguments) =>
  async (
    id: string,
    userId: string,
    forceRefresh: Dispatch<SetStateAction<boolean>>,
  ) => {
    const url = `${USER_ENDPOINT}/${userId}/saved/${routeArgs.route}/${id}`;
    handleUnsave(
      url,
      `${routeArgs.entity} deleted`,
      `${routeArgs.entity}-deleted`,
      forceRefresh,
    );
  };

const unSaveRange =
  (routeArgs: RouteArguments) =>
  async (
    ids: string[],
    userId: string,
    forceRefresh: Dispatch<SetStateAction<boolean>>,
  ) => {
    const queryString = createQueryString(ids);
    const url = `${USER_ENDPOINT}/${userId}/saved/${routeArgs.route}/ids${queryString}`;
    handleUnsave(
      url,
      `${routeArgs.entity}s deleted`,
      `${routeArgs.entity}s-deleted`,
      forceRefresh,
    );
  };

const fetchAllIds = (routeArgs: RouteArguments) => async (userId: string) => {
  try {
    const response = await axios.get<{ ids: string[] }>(
      `${USER_ENDPOINT}/${userId}/saved/${routeArgs.route}/all/ids`,
      {
        headers: {
          Accept: "application/json",
          Authorization: `bearer ${localStorage.getItem("token")}`,
        },
      },
    );

    return response.data.ids;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
  } catch (e: any) {
    if (e instanceof AxiosError) {
      handleError(e);
      return [];
    }

    // Will never happen
    return undefined!;
  }
};

type RouteArguments =
  | { route: "exercises"; entity: "Exercise" }
  | { route: "workouts"; entity: "Workout" }
  | { route: "muscles"; entity: "Muscle" }
  | { route: "muscle-groups"; entity: "Muscle group" };

const UserSavesPage = () => {
  const [musclePageState, setMusclePageState] = useState<PageState>();
  const [muscleGroupPageState, setMuscleGroupPageState] = useState<PageState>();
  const [exercisePageState, setExercisePageState] = useState<PageState>();
  const [workoutPageState, setWorkoutPageState] = useState<PageState>();

  const exerciseRouteArgs: RouteArguments = {
    route: "exercises",
    entity: "Exercise",
  };
  const unSaveExercise = unSave(exerciseRouteArgs);
  const unSaveExerciseRange = unSaveRange(exerciseRouteArgs);
  const fetchAllSavedExerciseIds = fetchAllIds(exerciseRouteArgs);

  return (
    <>
      <div className="flex justify-center pt-5 pb-10 h-full">
        <div className="w-[102rem] h-fit rounded-lg px-6 py-6">
          <Tab.Group>
            <Tab.List className="flex space-x-1 rounded-xl bg-[#262c31] p-1">
              {TABS.map((tabName, idx) => (
                <Tab
                  key={`tab-${idx}-${tabName}`}
                  className={({ selected }) =>
                    `w-full rounded-lg py-2.5 text-sm font-medium leading-5 text-blue-700 outline-none ${
                      selected
                        ? "bg-blue shadow"
                        : "text-blue-100 hover:bg-white/[0.12]"
                    }`
                  }
                >
                  {tabName}
                </Tab>
              ))}
            </Tab.List>
            <Tab.Panels className="mt-2 min-h-[42rem]">
              {/* <UserSavesTabPanel
                usePage={useMusclesPage}
                generateUrl={(m) => `/muscles/${m.id}`}
                iconType={SiElectron}
                pageState={musclePageState}
                setPageState={setMusclePageState}
              />

              <UserSavesTabPanel
                usePage={useMuscleGroupsPage}
                generateUrl={(mg) => `/muscle-groups/${mg.id}`}
                iconType={MdFitbit}
                pageState={muscleGroupPageState}
                setPageState={setMuscleGroupPageState}
              /> */}

              <UserSavesTabPanel
                usePage={useSavedExercisesPage}
                handleUnsave={unSaveExercise}
                handleUnsaveRange={unSaveExerciseRange}
                fetchAllIds={fetchAllSavedExerciseIds}
                errorMessage="You've got no exercises saved!"
                generateUrl={(e) => `/exercises/${e.id}`}
                iconType={BiDumbbell}
                pageState={exercisePageState}
                setPageState={setExercisePageState}
              />

              {/* <UserSavesTabPanel
                usePage={useWorkoutsPage}
                generateUrl={(w) => `/coaches/${w.coachId}/workouts/${w.id}`}
                iconType={IoFitnessSharp}
                pageState={workoutPageState}
                setPageState={setWorkoutPageState}
              /> */}
            </Tab.Panels>
          </Tab.Group>
        </div>
      </div>
    </>
  );
};

export default UserSavesPage;
