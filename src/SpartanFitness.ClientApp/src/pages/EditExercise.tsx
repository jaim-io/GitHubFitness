import axios from "axios";
import { useEffect, useRef, useState } from "react";
import Draggable from "react-draggable";
import { AiFillEdit } from "react-icons/ai";
import { BiDumbbell } from "react-icons/bi";
import { BsCloudUploadFill } from "react-icons/bs";
import { MdFitbit, MdOutlineBookmarkAdd } from "react-icons/md";
import { RxExit } from "react-icons/rx";
import { SiElectron } from "react-icons/si";
import { TbGhost2Filled } from "react-icons/tb";
import { Link, useLoaderData, useNavigate } from "react-router-dom";
import LoadingIcon from "../components/Icons/LoadingIcon";
import Select, { SelectOption } from "../components/Select";
import useAuth from "../hooks/useAuth";
import useMuscleGroupsByIds from "../hooks/useMuscleGroupsByIds";
import useMuscleGroupsPage from "../hooks/useMuscleGroupsPage";
import useMusclesByIds from "../hooks/useMusclesByIds";
import Exercise from "../types/domain/Exercise";
import Muscle from "../types/domain/Muscle";
import MuscleGroup from "../types/domain/MuscleGroup";
import { toast } from "react-toastify";

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
  const exercise = useLoaderData() as Exercise;
  const { auth } = useAuth();
  const muscleGroupSelectorRef = useRef<HTMLDivElement>(null);
  const muscleSelectorRef = useRef<HTMLDivElement>(null);

  const [isLoading, setIsLoading] = useState(false);

  const [initialMuscles, , initialMusclesAreLoading] =
    exercise.muscleIds.length != 0
      ? useMusclesByIds(exercise.muscleIds)
      : [[], undefined, false];

  const [initialMuscleGroups, , initialMuscleGroupsAreLoading] =
    exercise.muscleGroupIds.length != 0
      ? useMuscleGroupsByIds(exercise.muscleGroupIds)
      : [[], undefined, false];

  // getImage

  const [name, setName] = useState(exercise.name);
  const [description, setDescription] = useState(exercise.description);
  const descriptionRef = useRef<HTMLTextAreaElement>(null);
  const [image, setImage] = useState<File>();
  const [video, setVideo] = useState<string>(exercise.video);

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

  const setDescriptionAreaHeight = () => {
    descriptionRef.current!.style.height =
      descriptionRef.current!.scrollHeight + "px";
  };

  useEffect(() => {
    setDescriptionAreaHeight();
  }, []);

  const navigate = useNavigate();

  const [muscleGroupPage, , muscleGroupPageIsLoading] = useMuscleGroupsPage();
  const [muscles, setMuscles] = useState<Muscle[]>(initialMuscles ?? []);
  const [musclesAreLoading, setMusclesAreLoading] = useState(false);

  useEffect(() => {
    if (
      Object.values(selectedMuscles).length == 0 &&
      initialMuscles !== undefined
    ) {
      setSelectedMuscles(
        initialMuscles.map((m) => ({ label: m.name, value: m.id })),
      );
      setMuscles(initialMuscles);
    }
  }, [initialMuscles]);

  useEffect(() => {
    if (
      Object.values(selectedMuscleGroups).length == 0 &&
      initialMuscleGroups !== undefined
    ) {
      setSelectedMuscleGroups(
        initialMuscleGroups.map((m) => ({ label: m.name, value: m.id })),
      );
    }
  }, [initialMuscleGroups]);

  /**
   * Removes selected muscles where selectedMuscle.muscleGroupId is not Selected (in selectedMuscleGroups)
   */
  const updateMuscleSelection = (
    changedMuscleGroups: SelectOption<string>[],
  ) => {
    // MuscleGroups
    const mgs: MuscleGroup[] = [];
    Object.values(changedMuscleGroups).forEach((smg) => {
      const muscleGroup = muscleGroupPage?.muscleGroups.find(
        (mg) => mg.id === smg.value,
      );
      if (muscleGroup) mgs.push(muscleGroup);
    });

    // Muscles
    const ms: Muscle[] = [];
    Object.values(selectedMuscles).forEach((sm) => {
      const muscle = muscles.find((m) => m.id === sm.value);
      if (muscle) ms.push(muscle);
    });

    setSelectedMuscles((prev) => {
      const selection: SelectOption<string>[] = [];
      prev.forEach((sm) => {
        const muscle = ms.find((m) => m.id === sm.value);
        if (muscle) {
          if (mgs.find((mg) => mg.id === muscle.muscleGroupId)) {
            selection.push(sm);
          }
        }
      });
      return selection;
    });
  };

  const onMuscleGroupSelectionChange = async (
    selected: SelectOption<string>[],
  ) => {
    updateMuscleSelection(selected);

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

  const displayedMuscleGroups = muscleGroupPage?.muscleGroups.filter((mg) =>
    Object.values(selectedMuscleGroups).find((smg) => mg.id === smg.value),
  );

  const displayedMuscles = muscles?.filter((mg) =>
    Object.values(selectedMuscles).find((smg) => mg.id === smg.value),
  );

  const muscleOptions = muscles.map((m: Muscle) => ({
    label: m.name,
    value: m.id,
  }));

  const [showMuscleSelector, setShowMuscleSelector] = useState(false);
  const [showMuscleGroupSelector, setShowMuscleGroupSelector] = useState(false);

  const handleSaveChanges = async () => {
    setIsLoading(true);

    const formData = new FormData();
    formData.append("Id", exercise.id);
    formData.append("Name", name);
    formData.append("Description", description);
    formData.append("Image", image!);
    formData.append("Video", video);

    displayedMuscleGroups?.forEach((mg, i) =>
      formData.append(`muscleGroupIds[${i}]`, mg.id),
    );
    displayedMuscles?.forEach((m, i) =>
      formData.append(`muscleIds[${i}]`, m.id),
    );

    await axios
      .put(`${EXERCISE_ENDPOINT}/${exercise.id}`, formData, {
        headers: {
          Accept: "multipart/form-data",
          Authorization: `bearer ${localStorage.getItem("token")}`,
        },
      })
      .then(() => {
        setIsLoading(false);
        toast.success("Exercise has been updated", {
          toastId: "exercise-updated",
          position: "bottom-right",
          autoClose: 5000,
          hideProgressBar: false,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
          progress: undefined,
          theme: "colored",
        });
        navigate(`/exercises/${exercise.id}`);
      })
      .catch((err) => {
        setIsLoading(false);
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
  };

  return (
    <>
      <div className="flex justify-center pt-6 h-full">
        <Link
          className="bg-gray px-20 py-2 rounded-lg hover:border-hover-gray border border-[rgba(240,246,252,0.1)] flex items-center cursor-pointer mr-3"
          to={`/exercises/${exercise.id}`}
        >
          <RxExit className="mr-1" /> Leave edit mode
        </Link>
        <button
          className="px-20 py-2 rounded-lg bg-dark-green hover:bg-light-green text-white flex items-center cursor-pointer"
          onClick={handleSaveChanges}
        >
          {isLoading || isLoading == undefined ? (
            <div className="flex items-center justify-center animate-pulse">
              <LoadingIcon classNames="mr-2 text-white fill-white w-5 h-5" />
              <p>Saving...</p>
            </div>
          ) : (
            <>
              {" "}
              <BsCloudUploadFill className="mr-1" />
              Save changes
            </>
          )}
        </button>
      </div>
      <div className={"flex justify-center pt-6 pb-20 h-full"}>
        <div className="mr-6 max-w-[18rem]">
          <input
            type="file"
            accept="image/*"
            onChange={(e) => {
              if (e.target.files && e.target.files[0] !== null)
                setImage(e.target.files[0]);
            }}
          />
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
            {displayedMuscleGroups && (
              <div className="flex flex-wrap">
                {displayedMuscleGroups.map((mg) => (
                  <span
                    key={mg.id}
                    className="rounded-full border border-[rgba(240,246,252,0.1)] mr-2 px-2 py-1 mb-2 hover:border-hover-gray flex items-center cursor-not-allowed"
                  >
                    <MdFitbit className="mr-1" />
                    {mg.name}{" "}
                  </span>
                ))}
                <button
                  type="button"
                  className="rounded-full border border-[rgba(240,246,252,0.1)] mr-2 px-2 py-2 mb-2 hover:border-hover-gray flex items-center"
                  onClick={() => setShowMuscleGroupSelector((prev) => !prev)}
                >
                  <AiFillEdit size={16} />
                </button>
              </div>
            )}
            {initialMuscleGroupsAreLoading && <p>Muscle groups are loading</p>}
          </div>

          <div className="self-stretch border border-gray rounded-lg my-2 h-[1px]" />

          <div className="mt-4">
            {displayedMuscles && (
              <div className="flex flex-wrap">
                {displayedMuscles.map((m) => (
                  <span
                    key={m.id}
                    className="rounded-full border border-[rgba(240,246,252,0.1)] mr-2 px-2 py-1 mb-2 hover:border-hover-gray flex items-center cursor-not-allowed"
                  >
                    <SiElectron className="mr-1" />
                    {m.name}
                  </span>
                ))}
                <button
                  type="button"
                  className="rounded-full border border-[rgba(240,246,252,0.1)] mr-2 px-2 py-2 mb-2 hover:border-hover-gray flex items-center"
                  onClick={() => setShowMuscleSelector((prev) => !prev)}
                >
                  <AiFillEdit size={16} />
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
                    "bg-transparent outline-none rounded-lg w-[30rem] focus:bg-gray hover:bg-gray"
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
              Last updated by:{" "}
              <span className="text-blue ml-1 hover:underline cursor-not-allowed">
                {auth.user!.firstName} {auth.user!.lastName}
              </span>
            </div>
          </div>
        </div>
      </div>

      <Draggable
        nodeRef={muscleGroupSelectorRef}
        bounds={{ left: -550, right: 550, top: -300, bottom: 450 }}
      >
        <div
          className={`absolute top-[40%] left-[35%] border border-blue rounded-lg pt-2 z-10 bg-black w-[40rem] ${
            showMuscleGroupSelector ? "" : "hidden"
          }`}
          ref={muscleGroupSelectorRef}
        >
          <p className="text-center mb-1 cursor-move">Muscle group selection</p>
          <button
            type="button"
            onClick={() => setShowMuscleGroupSelector(false)}
            className="absolute top-1 right-1 rounded-lg flex items-center hover:bg-gray justify-center cursor-pointer"
          >
            <span className="bg-none text-white border-none outline-none cursor-pointer text-lg px-3">
              &times;
            </span>
          </button>
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

      <Draggable nodeRef={muscleSelectorRef}>
        <div
          className={`absolute top-[40%] left-[35%] border border-blue rounded-lg pt-2 z-10 bg-black w-[40rem] ${
            showMuscleSelector ? "" : "hidden"
          }`}
          ref={muscleSelectorRef}
        >
          <p className="text-center mb-1 cursor-move">Muscle selection</p>
          <button
            type="button"
            onClick={() => setShowMuscleSelector(false)}
            className="absolute top-1 right-1 rounded-lg flex items-center hover:bg-gray justify-center cursor-pointer"
          >
            <span className="bg-none text-white border-none outline-none cursor-pointer text-lg px-3">
              &times;
            </span>
          </button>
          <Select
            id={"muscles"}
            searchBar={true}
            multiple={true}
            value={selectedMuscles}
            options={muscleOptions ?? []}
            onChange={(selected) => {
              setSelectedMuscles(selected);
            }}
            isLoading={musclesAreLoading}
            ifEmpty={
              <p className="flex justify-center items-center py-1 cursor-default">
                No muscles found <TbGhost2Filled className="ml-1" size={20} />
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
    </>
  );
};

export default EditExercisePage;
