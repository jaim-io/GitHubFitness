import axios from "axios";
import { FormEvent, useState } from "react";
import { TbGhost2Filled } from "react-icons/tb";
import { Link, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import LoadingIcon from "../../components/icons/LoadingIcon";
import Select, { SelectOption } from "../../components/Select";
import useMuscleGroupsPage from "../../hooks/useMuscleGroupsPage";
import Exception from "../../types/domain/Exception";
import Exercise from "../../types/domain/Exercise";
import Muscle from "../../types/domain/Muscle";
import MuscleGroup from "../../types/domain/MuscleGroup";
import {
  validateDefaultUrl,
  validateDescription,
  validateName,
  validateYoutubeUrl,
} from "../../utils/StringValidations";
import InputField from "../../components/InputField";
import TexAreaField from "../../components/TextAreaField";

const EXERCISE_ENDPOINT = `${import.meta.env.VITE_API_BASE}/exercises/create`;
const MUSCLES_ENDPOINT = `${
  import.meta.env.VITE_API_BASE
}/muscles/muscle-group-ids`;

const createQueryString = (ids: string[]): string => {
  const params: string[] = [];

  ids.forEach((id) => params.push(`id=${id}`));

  const queryString = `?${params.join("&")}`;

  return ids.length == 0 ? "" : queryString;
};

const NewExercisePage = () => {
  const [name, setName] = useState("");
  const [isValidName, setIsValidName] = useState(false);

  const [description, setDescription] = useState("");
  const [isValidDescription, setIsValidDescription] = useState(true); // true since the field is optional

  const [selectedMuscleGroups, setSelectedMuscleGroups] = useState<
    SelectOption<string>[]
  >([]);
  const [selectedMuscles, setSelectedMuscles] = useState<
    SelectOption<string>[]
  >([]);

  const [image, setImage] = useState("");
  const [isValidImage, setIsValidImage] = useState(false);
  const [video, setVideo] = useState("");
  const [isValidVideo, setIsValidVideo] = useState(false);

  const [isLoading, setIsLoading] = useState(false);
  const [, setError] = useState<Exception>();
  const navigate = useNavigate();

  const [muscleGroupPage, , muscleGroupPageIsLoading] = useMuscleGroupsPage();
  const [muscles, setMuscles] = useState<Muscle[]>([]);
  const [musclesAreLoading, setMusclesAreLoading] = useState(false);

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
          if (mgs.find((mg) => mg.muscleIds.includes(muscle.id))) {
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

  const musclesOptions = muscles.map((m: Muscle) => ({
    label: m.name,
    value: m.id,
  }));

  const isValidForm =
    isValidName && isValidDescription && isValidImage && isValidVideo;

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!isValidForm) {
      return;
    }

    setIsLoading(true);

    try {
      await axios
        .post<Exercise>(
          EXERCISE_ENDPOINT,
          {
            name: name,
            description: description,
            muscleGroupIds: selectedMuscleGroups.map((mg) => mg.value),
            muscleIds: selectedMuscles.map((m) => m.value),
            image: image,
            video: video,
          },
          {
            headers: {
              "Content-Type": "application/json",
              Authorization: `bearer ${localStorage.getItem("token")}`,
            },
          },
        )
        .then((res) => {
          toast.success("Exercise has been created", {
            toastId: "exercise-created",
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

          navigate(`/exercises/${res.data.id}`);
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
      <div className="w-[42rem]">
        <h1 className="text-2xl mb-4">Create a new exercise</h1>

        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <InputField
              value={name}
              onChange={setName}
              placeholder="Barbell bench press"
              label="Exercise name *"
              validator={validateName}
              validationProps={{ minLength: 5, maxLength: 100 }}
              setIsValid={setIsValidName}
            />
          </div>

          <div className="mb-4">
            <TexAreaField
              value={description}
              onChange={setDescription}
              placeholder="A chest exercise which involves chest, delts and triceps..."
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
            <label className="flex text-white mb-2 ml-1 items-center">
              Muscle groups
              <p className="ml-1 text-light-gray text-sm">(optional)</p>
            </label>

            <Select<string>
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
          <div className="bg-gray self-stretch w-full h-[1px] mb-4 mt-6" />

          <div className="mb-4">
            <label className="flex text-white mb-2 ml-1 items-center">
              Muscles
              <p className="ml-1 text-light-gray text-sm">(optional)</p>
            </label>

            <Select<string>
              id={"muscles"}
              searchBar={true}
              multiple={true}
              value={selectedMuscles}
              options={musclesOptions ?? []}
              onChange={setSelectedMuscles}
              isLoading={musclesAreLoading}
              ifEmpty={
                <p className="flex justify-center items-center py-1 cursor-default">
                  No muscle found matching the muscle groups{" "}
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
          <div className="bg-gray self-stretch w-full h-[1px] mb-4 mt-6" />

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

          <div className="mb-4">
            <InputField
              value={video}
              onChange={setVideo}
              placeholder="https://www.youtube-nocookie.com/embed/exercise-video"
              label={"Video URL *"}
              validator={validateYoutubeUrl}
              validationProps={{ maxLength: 2048 }}
              setIsValid={setIsValidVideo}
            />
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
              className={`absolute right-0 bg-dark-green hover:bg-light-green border border-[rgba(240,246,252,0.1)] rounded-lg py-1 px-3 flex justify-center items-center ${
                !isValidForm ? "cursor-not-allowed opacity-50" : ""
              }`}
              type="submit"
              disabled={!isValidForm}
            >
              {!isLoading && <p>Create exercise</p>}
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

export default NewExercisePage;
