import { Tab } from "@headlessui/react";
import { BiDumbbell } from "react-icons/bi";
import { IoFitnessSharp } from "react-icons/io5";
import { MdFitbit } from "react-icons/md";
import { SiElectron } from "react-icons/si";
import UserSavesTabPanel, {
  PageState,
} from "../../components/UserSaveTabContent";
import useExercisesPage from "../../hooks/useExercisesPage";
import useMuscleGroupsPage from "../../hooks/useMuscleGroupsPage";
import useMusclesPage from "../../hooks/useMusclesPage";
import useWorkoutsPage from "../../hooks/useWorkoutsPage";
import { useState } from "react";

const TABS = ["Muscles", "Muscle Groups", "Exercises", "Workouts"];

const UserSavesPage = () => {
  const [musclePageState, setMusclePageState] = useState<PageState>();
  const [muscleGroupPageState, setMuscleGroupPageState] = useState<PageState>();
  const [exercisePageState, setExercisePageState] = useState<PageState>();
  const [workoutPageState, setWorkoutPageState] = useState<PageState>();

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
              <UserSavesTabPanel
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
              />

              <UserSavesTabPanel
                usePage={useExercisesPage}
                generateUrl={(e) => `/exercises/${e.id}`}
                iconType={BiDumbbell}
                pageState={exercisePageState}
                setPageState={setExercisePageState}
              />

              <UserSavesTabPanel
                usePage={useWorkoutsPage}
                generateUrl={(w) => `/coaches/${w.coachId}/workouts/${w.id}`}
                iconType={IoFitnessSharp}
                pageState={workoutPageState}
                setPageState={setWorkoutPageState}
              />
            </Tab.Panels>
          </Tab.Group>
        </div>
      </div>
    </>
  );
};

export default UserSavesPage;
