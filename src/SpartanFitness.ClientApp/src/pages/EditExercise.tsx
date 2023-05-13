import { useEffect, useRef, useState } from "react";
import { BiDumbbell } from "react-icons/bi";
import { MdFitbit, MdOutlineBookmarkAdd } from "react-icons/md";
import { SiElectron } from "react-icons/si";
import { Link, useNavigate, useRouteLoaderData } from "react-router-dom";
import useAuth from "../hooks/useAuth";
import useMuscleGroupsByIds from "../hooks/useMuscleGroupsByIds";
import useMusclesByIds from "../hooks/useMusclesByIds";
import Exercise from "../types/domain/Exercise";
import Muscle from "../types/domain/Muscle";
import MuscleGroup from "../types/domain/MuscleGroup";
import { RxExit } from "react-icons/rx";
import Select, { SelectOption } from "../components/Select";
import useMuscleGroupsPage from "../hooks/useMuscleGroupsPage";
import axios from "axios";
import Draggable from "react-draggable";
import { TbGhost2Filled } from "react-icons/tb";
import LoadingIcon from "../components/Icons/LoadingIcon";

const EXERCISE_ENDPOINT = `${import.meta.env.VITE_API_BASE}/exercises/update`;
const MUSCLES_ENDPOINT = `${
  import.meta.env.VITE_API_BASE
}/muscles/muscle-group-ids`;

const createQueryString = (ids: string[]): string => {
  const params: string[] = [];

  ids.forEach((id) => params.push(`id=${id}`));

  const queryString = `?${params.join("&")}`;

  return ids.length == 0 ? "" : queryString;
};

const EditExercisePage = () => {
  const exercise = useRouteLoaderData("exercise-details") as Exercise;
  const { auth } = useAuth();
  const dragTestRef = useRef<HTMLDivElement>(null);

  const [initialMuscles, , initialMusclesAreLoading] =
    exercise.muscleIds.length != 0
      ? useMusclesByIds(exercise.muscleIds)
      : [[], undefined, false];

  const [initialMuscleGroups, , initialMuscleGroupsAreLoading] =
    exercise.muscleGroupIds.length != 0
      ? useMuscleGroupsByIds(exercise.muscleGroupIds)
      : [[], undefined, false];

  const [name, setName] = useState(exercise.name);
  const [description, setDescription] = useState(exercise.description);
  const descriptionRef = useRef<HTMLTextAreaElement>(null);
  const [selectedMuscles, setSelectedMuscles] = useState<
    SelectOption<string>[]
  >(
    initialMuscles
      ? initialMuscles?.map((m) => ({ label: m.name, value: m.id }))
      : [],
  );
  const [selectedMuscleGroups, setSelectedMuscleGroups] = useState<
    SelectOption<string>[]
  >(
    initialMuscleGroups
      ? initialMuscleGroups?.map((m) => ({ label: m.name, value: m.id }))
      : [],
  );
  const navigate = useNavigate();

  const [muscleGroupPage, , muscleGroupPageIsLoading] = useMuscleGroupsPage();
  const [muscles, setMuscles] = useState<Muscle[]>([]);
  const [musclesAreLoading, setMusclesAreLoading] = useState(false);

  const onMuscleGroupSelectionChange = async (
    selected: SelectOption<string>[],
  ) => {
    const queryString = createQueryString(selected.map((s) => s.value));

    setMusclesAreLoading(true);
    const response = await axios.get<Muscle[]>(
      `${MUSCLES_ENDPOINT}${queryString}`,
      {
        headers: {
          Accept: "application/json",
          Authorization: `bearer ${localStorage.getItem("token")}`,
        },
      },
    );

    setMusclesAreLoading(false);

    const muscles = response.data;
    setMuscles(muscles);
  };

  const muscleGroupOptions = muscleGroupPage?.muscleGroups.map((mg) => ({
    label: mg.name,
    value: mg.id,
  }));

  const musclesOptions = muscles.map((m: Muscle) => ({
    label: m.name,
    value: m.id,
  }));

  const setDescriptionAreaHeight = () => {
    descriptionRef.current!.style.height =
      descriptionRef.current!.scrollHeight + "px";
  };

  useEffect(() => {
    setDescriptionAreaHeight();
  }, []);

  return (
    <>
      <div className="flex justify-center pt-6 h-full">
        <Link
          className="bg-gray px-20 py-2 rounded-lg hover:border-hover-gray border border-[rgba(240,246,252,0.1)] flex items-center cursor-pointer"
          to={".."}
        >
          <RxExit className="mr-1" /> Leave edit mode
        </Link>
      </div>
      <div className={"flex justify-center pt-6 pb-20 h-full"}>
        <div className="mr-6 max-w-[18rem]">
          <img
            src={exercise.image}
            alt={`${exercise.name} image`}
            className="rounded-full border border-gray w-[18rem] h-[18rem] flex text-center leading-[9.5rem]"
          />

          <button
            type="button"
            className="w-full border border-[rgba(240,246,252,0.1)] rounded-lg mt-4 py-1 flex items-center justify-center hover:border-hover-gray bg-gray cursor-not-allowed opacity-50"
            disabled
          >
            <>
              <MdOutlineBookmarkAdd className="mr-1" size={16} />
              Save
            </>
          </button>

          <div className="mt-4">
            {initialMuscleGroups && (
              <div className="flex flex-wrap">
                {initialMuscleGroups.map((mg) => (
                  <button
                    type="button"
                    key={mg.id}
                    className="rounded-full border border-[rgba(240,246,252,0.1)] mr-2 px-2 py-1 mb-2 hover:border-hover-gray flex items-center"
                  >
                    <MdFitbit className="mr-1" />
                    {mg.name}{" "}
                    <span className="bg-none text-white border-none outline-none cursor-pointer text-lg ml-1">
                      &times;
                    </span>
                  </button>
                ))}
                <button
                  type="button"
                  className="rounded-full border border-[rgba(240,246,252,0.1)] mr-2 px-2 py-1 mb-2 hover:border-hover-gray flex items-center"
                >
                  <span className="">&#43;</span>
                </button>
              </div>
            )}
            {initialMuscleGroupsAreLoading && <p>Muscle groups are loading</p>}
          </div>

          {initialMuscles?.length !== 0 &&
            initialMuscleGroups?.length !== 0 && (
              <div className="self-stretch border border-gray rounded-lg my-2 h-[1px]" />
            )}

          <div className="mt-4">
            {initialMuscles && (
              <div className="flex flex-wrap">
                {initialMuscles.map((m) => (
                  <button
                    type="button"
                    key={m.id}
                    className="rounded-full border border-[rgba(240,246,252,0.1)] mr-2 px-2 py-1 mb-2 hover:border-hover-gray flex items-center"
                  >
                    <SiElectron className="mr-1" />
                    {m.name}
                    <span className="bg-none text-white border-none outline-none cursor-pointer text-lg ml-1">
                      &times;
                    </span>
                  </button>
                ))}
                <button
                  type="button"
                  className="rounded-full border border-[rgba(240,246,252,0.1)] mr-2 px-2 py-1 mb-2 hover:border-hover-gray flex items-center"
                >
                  <span className="">&#43;</span>
                </button>
              </div>
            )}
            {initialMusclesAreLoading && <p>Muscles are loading</p>}
          </div>
        </div>
        <div className="relative">
          <div className="border border-gray w-[40rem] h-fit rounded-lg px-6 py-6">
            <h1 className="text-light-gray flex items-center">
              <BiDumbbell className="mr-1" size={16} />
              Exercise<span className="mx-1">/</span>
              <span className="text-blue">
                <input
                  className={
                    "bg-transparent outline-none rounded-lg w-full focus:bg-gray hover:bg-gray"
                  }
                  value={name}
                  spellCheck={false}
                  onChange={(e) => setName(e.target.value)}
                />
              </span>
            </h1>
            <div className="self-stretch border border-gray mt-4 h-[1px] rounded-lg" />
            <p className="pt-4">
              <textarea
                ref={descriptionRef}
                className="outline-none w-full bg-transparent resize-none rounded-lg focus:bg-gray hover:bg-gray"
                value={description}
                spellCheck={false}
                onChange={(e) => {
                  setDescriptionAreaHeight();
                  setDescription(e.target.value);
                }}
              />
            </p>
          </div>
          <div className="border border-gray w-[40rem] h-fit rounded-lg px-6 py-6 mt-4">
            <iframe
              className="w-full h-[18.125rem]"
              src={exercise.video}
              title="YouTube video player"
              allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
            />
          </div>

          <div className="mt-4">
            <button
              type="button"
              className="absolute left-0 bg-gray hover:border-hover-gray border border-[rgba(240,246,252,0.1)] rounded-lg py-1 px-3 cursor-not-allowed opacity-50"
              disabled
            >
              Back
            </button>

            <div className="absolute right-0 py-1 px-3 border border-gray rounded-lg flex items-center text-light-gray ml-2 justify-center">
              Created by:{" "}
              <span className="text-blue ml-1">
                {auth.user!.firstName} {auth.user!.lastName}
              </span>
            </div>
            <Draggable nodeRef={dragTestRef}>
              <div
                className="border border-gray rounded-lg pt-2 z-10 bg-black"
                ref={dragTestRef}
              >
                <p className="text-center mb-1 cursor-move">
                  Muscle group selection
                </p>
                <Select
                  id={"muscleGroups"}
                  searchBar={true}
                  multiple={true}
                  value={selectedMuscleGroups}
                  options={muscleGroupOptions ?? []}
                  onChange={(selected) => {
                    setSelectedMuscleGroups(selected);
                    onMuscleGroupSelectionChange(selected);
                  }}
                  isLoading={muscleGroupPageIsLoading}
                  ifEmpty={
                    <p className="flex justify-center items-center py-1 cursor-default">
                      No muscle groups found{" "}
                      <TbGhost2Filled className="ml-1" size={20} />
                    </p>
                  }
                  ifLoading={
                    <div
                      role="status"
                      className="py-5 flex justify-center items-center"
                    >
                      <LoadingIcon classNames="mr-2 fill-blue text-gray w-8 h-8" />
                      <span className="sr-only">Loading...</span>
                    </div>
                  }
                />
              </div>
            </Draggable>
          </div>
        </div>
      </div>
    </>
  );
};

export default EditExercisePage;
