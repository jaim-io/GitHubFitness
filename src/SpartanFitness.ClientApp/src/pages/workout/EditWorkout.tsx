import axios from "axios";
import { useEffect, useRef, useState } from "react";
import { AiFillEdit } from "react-icons/ai";
import { BiDumbbell } from "react-icons/bi";
import { BsCloudUploadFill, BsExclamationCircle } from "react-icons/bs";
import { MdFitbit, MdOutlineBookmarkAdd } from "react-icons/md";
import { RxExit } from "react-icons/rx";
import { useLoaderData, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import EditableWorkoutExerciseTable, {
  WorkoutExerciseWrapper,
} from "../../components/EditableWorkoutExerciseTable";
import LoadingIcon from "../../components/icons/LoadingIcon";
import useAuth from "../../hooks/useAuth";
import useExercises from "../../hooks/useExercises";
import Workout, { WorkoutExercise } from "../../types/domain/Workout";
import useMuscleGroups from "../../hooks/useMuscleGroups";
import useMuscles from "../../hooks/useMuscles";
import { SiElectron } from "react-icons/si";
import {
  StringValidatonProps,
  validateDefaultUrl,
  validateDescription,
  validateName,
} from "../../utils/StringValidations";

const EditWorkoutPage = () => {
  const { auth } = useAuth();
  const coachRole = auth.user!.roles.find((r) => r.name === "Coach")!;

  const workout = useLoaderData() as Workout;

  const [isLoading, setIsLoading] = useState(false);

  const [name, setName] = useState(workout.name);
  const [isValidName, setIsValidName] = useState(true);
  const [nameError, setNameError] = useState<string>();
  const nameValidationProps: StringValidatonProps = {
    minLength: 5,
    maxLength: 100,
  };

  const [description, setDescription] = useState(workout.description);
  const [isValidDescription, setIsValidDescription] = useState(true);
  const descriptionRef = useRef<HTMLTextAreaElement>(null);
  const [descriptionError, setDescriptionError] = useState<string>();
  const descriptionValidationProps: StringValidatonProps = {
    maxLength: 2048,
  };

  const [image, setImage] = useState<string>(workout.image);
  const [isValidImage, setIsValidImage] = useState(true);
  const [previewImage, setPreviewImage] = useState<string>(workout.image);
  const [imageError, setImageError] = useState<string>();
  const imageValidationProps: StringValidatonProps = {
    maxLength: 2048,
  };

  const navigate = useNavigate();
  const [showImageUrlInputBar, setShowImageUrlInputBar] = useState(false);

  const [exercises, , exercisesLoading] = useExercises();
  const [muscles, , musclesLoading] = useMuscles();
  const [muscleGroups, muscleGroupsLoading] = useMuscleGroups();
  const [workoutExercises, setWorkoutExercises] = useState<
    WorkoutExerciseWrapper[]
  >([]);

  const validExercises = !workoutExercises
    .map((we) => we.isValid)
    .includes(false);
  const isValidForm =
    isValidName && isValidDescription && isValidImage && validExercises;

  useEffect(() => {
    if (exercises) {
      const initialWorkoutExercises: WorkoutExerciseWrapper[] = [];
      workout.workoutExercises.forEach((we) => {
        const exercise = exercises?.find((e) => e.id === we.exerciseId);
        if (exercise) {
          initialWorkoutExercises!.push({
            ...we,
            ...exercise,
            isValid: true,
            id: we.id,
            exerciseId: exercise.id,
          });
        }
      });

      setWorkoutExercises(initialWorkoutExercises);
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

  const setDescriptionAreaHeight = () => {
    descriptionRef.current!.style.height =
      descriptionRef.current!.scrollHeight + "px";
  };

  useEffect(() => {
    setDescriptionAreaHeight();
  }, []);

  const handleSaveChanges = async () => {
    if (!isValidForm) {
      return;
    }

    setIsLoading(true);

    await axios
      .put(
        `${import.meta.env.VITE_API_BASE}/coaches/${coachRole.id}/workouts/${
          workout.id
        }/update`,
        {
          id: workout.id,
          name: name,
          description: description,
          image: image,
          workoutExercises: workoutExercises.map((we) => we as WorkoutExercise),
        },
        {
          headers: {
            Accept: "application/json",
            Authorization: `bearer ${localStorage.getItem("token")}`,
          },
        },
      )
      .then(() => {
        setIsLoading(false);
        toast.success("Workout has been updated", {
          toastId: "Workout-updated",
          position: "bottom-right",
          autoClose: 5000,
          hideProgressBar: false,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
          progress: undefined,
          theme: "colored",
        });
        navigate(`/coaches/${workout.coachId}/workouts/${workout.id}`);
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
        <button
          className={`bg-gray px-20 py-2 rounded-lg hover:border-hover-gray border border-[rgba(240,246,252,0.1)] flex items-center cursor-pointer mr-3 ${
            showImageUrlInputBar ? "opacity-50 hover:cursor-not-allowed" : ""
          }`}
          type="button"
          onClick={() => navigate(-1)}
        >
          <RxExit className="mr-1" /> Leave edit mode
        </button>
        <button
          className={`px-20 py-2 rounded-lg bg-dark-green hover:bg-light-green text-white flex items-center cursor-pointer ${
            showImageUrlInputBar || !isValidForm
              ? "hover:cursor-not-allowed opacity-50"
              : ""
          }`}
          onClick={handleSaveChanges}
          disabled={showImageUrlInputBar || !isValidForm}
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
          <div
            className={`relative cursor-pointer`}
            onClick={() => setShowImageUrlInputBar((prev) => !prev)}
          >
            <img
              src={previewImage}
              alt={`${workout.name} image`}
              className="rounded-full border border-gray w-[18rem] h-[18rem] flex text-center leading-[9.5rem]"
            />
            <div
              className={`absolute left-0 top-0 w-full h-full items-center rounded-full ease-in-out duration-200 hover:backdrop-blur-sm ${
                showImageUrlInputBar ? "backdrop-blur-sm" : ""
              }`}
            >
              <div className="relative flex justify-center top-[48%]">
                <AiFillEdit size={18} />
              </div>
            </div>
          </div>

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
            {!muscleGroupsLoading || muscleGroupsLoading === undefined ? (
              activeMuscleGroups ? (
                <div className="flex flex-wrap">
                  {activeMuscleGroups.length > 0 ? (
                    activeMuscleGroups.map((mg) => (
                      <span
                        key={mg.id}
                        className="rounded-full border border-[rgba(240,246,252,0.1)] mr-2 px-2 py-1 mb-2 hover:border-hover-gray flex items-center cursor-not-allowed"
                      >
                        <MdFitbit className="mr-1" />
                        {mg.name}
                      </span>
                    ))
                  ) : (
                    <p className="ml-1">No muscle groups specified</p>
                  )}
                </div>
              ) : (
                <span className="ml-1">
                  An error occured while loading the muscle groups.
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

          {muscles?.length !== 0 && muscleGroups?.length !== 0 && (
            <div className="self-stretch border border-gray rounded-lg my-2 h-[1px]" />
          )}

          <div className="mt-4">
            {!musclesLoading || musclesLoading === undefined ? (
              activeMuscles ? (
                <div className="flex flex-wrap">
                  {activeMuscles.length > 0 ? (
                    activeMuscles.map((m) => (
                      <span
                        key={m.id}
                        className="rounded-full border border-[rgba(240,246,252,0.1)] mr-2 px-2 py-1 mb-2 hover:border-hover-gray flex items-center cursor-not-allowed"
                      >
                        <SiElectron className="mr-1" />
                        {m.name}
                      </span>
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
        </div>

        <div className="relative">
          <div className="border border-gray w-[45rem] h-fit rounded-lg px-6 py-6">
            <h1 className="text-light-gray flex items-center">
              <BiDumbbell className="mr-1" size={16} />
              Exercise<span className="mx-1">/</span>
              <span className="text-blue">
                <input
                  className={`bg-transparent outline-none rounded-lg w-[30rem] focus:bg-gray hover:bg-gray ${
                    showImageUrlInputBar ? "pointer-events-none" : ""
                  }`}
                  value={name}
                  spellCheck={false}
                  onChange={(e) => {
                    const validation = validateName(
                      e.target.value,
                      nameValidationProps,
                    );

                    if (validation.isValid) {
                      setNameError(undefined);
                    } else {
                      setNameError(validation.errorMsg);
                    }
                    setIsValidName(validation.isValid);
                    setName(e.target.value);
                  }}
                />
              </span>
            </h1>
            {nameError && (
              <div className="pt-2">
                <p className="shadow appearance-none border border-red rounded-lg w-full py-1 px-3 text-whiteas bg-black font-medium flex items-center">
                  <BsExclamationCircle className="text-red mr-1" size={14} />{" "}
                  {nameError}
                </p>
              </div>
            )}

            <div className="self-stretch border border-gray mt-4 h-[1px] rounded-lg" />
            <p className="pt-4">
              <textarea
                ref={descriptionRef}
                className={`outline-none w-full bg-transparent rounded-lg focus:bg-gray hover:bg-gray ${
                  showImageUrlInputBar ? "pointer-events-none" : ""
                }`}
                value={description}
                spellCheck={false}
                onChange={(e) => {
                  const validation = validateDescription(
                    e.target.value,
                    descriptionValidationProps,
                  );

                  if (validation.isValid) {
                    setDescriptionError(undefined);
                  } else {
                    setDescriptionError(validation.errorMsg);
                  }
                  setIsValidDescription(validation.isValid);

                  setDescriptionAreaHeight();
                  setDescription(e.target.value);
                }}
              />
            </p>

            {descriptionError && (
              <div className="pt-2">
                <p className="shadow appearance-none border border-red rounded-lg w-full py-1 px-3 text-whiteas bg-black font-medium flex items-center">
                  <BsExclamationCircle className="text-red mr-1" size={14} />{" "}
                  {descriptionError}
                </p>
              </div>
            )}
          </div>

          <div className="border border-gray w-[55rem] h-fit rounded-lg px-6 py-6  mt-4">
            {!exercisesLoading || exercisesLoading === undefined ? (
              exercises ? (
                <EditableWorkoutExerciseTable
                  workoutExercises={workoutExercises}
                  exercises={exercises}
                  setWorkoutExercises={setWorkoutExercises}
                />
              ) : (
                <span className="ml-1">
                  An error occured while loading the exercises.
                </span>
              )
            ) : (
              <div
                role="status"
                className="py-5 flex justify-center items-center"
              >
                <LoadingIcon classNames="mr-2 fill-blue text-gray w-6 h-6" />
                <span className="sr-only">Loading...</span>
              </div>
            )}
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
              Last updated:{" "}
              <span className="text-blue ml-1">To be assigned</span>
            </div>
          </div>
        </div>
      </div>

      <div
        className={`absolute top-[40%] left-[35%] border border-blue rounded-lg pt-2 z-10 bg-black w-[40rem] ${
          showImageUrlInputBar ? "" : "hidden"
        }`}
      >
        <p className="text-center mb-1">Image URL</p>

        <button
          type="button"
          onClick={() => {
            // Resets to the previous value
            const validation = validateDefaultUrl(
              previewImage,
              imageValidationProps,
            );
            if (validation.isValid) {
              setImageError(undefined);
            } else {
              setImageError(validation.errorMsg);
            }

            setIsValidImage(validation.isValid);
            setShowImageUrlInputBar(false);
            setImage(previewImage);
          }}
          className="absolute top-1 left-1 rounded-lg flex items-center hover:bg-gray justify-center cursor-pointer"
        >
          <span className="bg-none text-white border-none outline-none text-lg px-3">
            &times;
          </span>
        </button>

        <button
          type="button"
          onClick={() => {
            if (isValidImage) {
              setShowImageUrlInputBar(false);
              setPreviewImage(image);
            }
          }}
          disabled={!isValidImage}
          className={`absolute top-1 right-1 rounded-lg flex items-center hover:bg-gray justify-center text-white ${
            !isValidImage ? "cursor-not-allowed opacity-50" : "cursor-pointer"
          }`}
        >
          <span className="bg-none border-none outline-none text-lg px-3">
            &#x2713;
          </span>
        </button>

        <div className="flex w-full items-center justify-center">
          <input
            onChange={(e) => {
              const validation = validateDefaultUrl(
                e.target.value,
                imageValidationProps,
              );

              if (validation.isValid) {
                setImageError(undefined);
              } else {
                setImageError(validation.errorMsg);
              }
              setIsValidImage(validation.isValid);
              setImage(e.target.value);
            }}
            value={image}
            className="shadow appearance-none border border-gray rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black"
            autoComplete="off"
            placeholder="https://example.com/your-image"
          />
        </div>

        {imageError && (
          <div className="pt-2">
            <p className="shadow appearance-none border border-red rounded-lg w-full py-1 px-3 text-whiteas bg-black font-medium flex items-center">
              <BsExclamationCircle className="text-red mr-1" size={14} />{" "}
              {imageError}
            </p>
          </div>
        )}
      </div>
    </>
  );
};

export default EditWorkoutPage;
