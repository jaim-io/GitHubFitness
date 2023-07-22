import axios from "axios";
import { useState } from "react";
import { AiFillEdit } from "react-icons/ai";
import { BiDumbbell } from "react-icons/bi";
import {
  MdBookmarkAdded,
  MdFitbit,
  MdOutlineBookmarkAdd,
} from "react-icons/md";
import { SiElectron } from "react-icons/si";
import {
  Link,
  LoaderFunctionArgs,
  useLoaderData,
  useNavigate,
} from "react-router-dom";
import { toast } from "react-toastify";
import LoadingIcon from "../components/Icons/LoadingIcon";
import useAuth from "../hooks/useAuth";
import useCoach from "../hooks/useCoach";
import useMuscleGroupsByIds from "../hooks/useMuscleGroupsByIds";
import useMusclesByIds from "../hooks/useMusclesByIds";
import Exercise from "../types/domain/Exercise";
import Muscle from "../types/domain/Muscle";
import MuscleGroup from "../types/domain/MuscleGroup";

const USER_ENDPOINT = `${import.meta.env.VITE_API_BASE}/users`;

const ExerciseDetailPage = () => {
  const exercise = useLoaderData() as Exercise;
  const { auth } = useAuth();
  const navigate = useNavigate();
  const [saved, setSaved] = useState(
    Object.values(auth.user!.savedExerciseIds ?? []).includes(exercise.id),
  );

  let musclesAreLoading = false;
  let muscles: Muscle[] | undefined = undefined;
  if (exercise.muscleIds != undefined && exercise.muscleIds.length != 0) {
    [muscles, , musclesAreLoading] = useMusclesByIds(exercise.muscleIds);
  } else {
    muscles = [];
  }

  let muscleGroupsAreLoading = false;
  let muscleGroups: MuscleGroup[] | undefined = undefined;
  if (
    exercise.muscleGroupIds != undefined &&
    exercise.muscleGroupIds.length != 0
  ) {
    [muscleGroups, , muscleGroupsAreLoading] = useMuscleGroupsByIds(
      exercise.muscleGroupIds,
    );
  } else {
    muscleGroups = [];
  }

  const [lastUpdater] = useCoach(exercise.lastUpdaterId);

  const handleSaving = async () => {
    setSaved((prev) => !prev);

    const action = saved ? "remove" : "add";

    await axios
      .patch(
        `${USER_ENDPOINT}/${auth.user?.id}/saved/exercises/${action}`,
        {
          exerciseId: exercise.id,
        },
        {
          headers: {
            Accept: "application/json",
            Authorization: `bearer ${localStorage.getItem("token")}`,
          },
        },
      )
      .then(() => {
        if (action == "add") {
          auth.user!.savedExerciseIds.push(exercise.id);
        } else {
          auth.user!.savedExerciseIds =
            auth.user?.savedExerciseIds.filter((id) => id !== exercise.id) ??
            [];
        }
      })
      .catch((err) => {
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
      <div className={"flex justify-center pt-6 pb-20 h-full"}>
        <div className="mr-6 max-w-[18rem]">
          <img
            src={exercise.image}
            alt={`${exercise.name} image`}
            className="rounded-full border border-gray w-[18rem] h-[18rem] flex text-center leading-[9.5rem]"
          />

          <button
            className="w-full border border-[rgba(240,246,252,0.1)] rounded-lg mt-4 py-1 flex items-center justify-center hover:border-hover-gray bg-gray"
            onClick={handleSaving}
          >
            {saved ? (
              <>
                <MdBookmarkAdded className="mr-1 fill-[#e3b341]" size={16} />
                Saved
              </>
            ) : (
              <>
                <MdOutlineBookmarkAdd className="mr-1" size={16} />
                Save
              </>
            )}
          </button>

          <div className="mt-4">
            {!muscleGroupsAreLoading || muscleGroupsAreLoading === undefined ? (
              muscleGroups ? (
                <div className="flex flex-wrap">
                  {muscleGroups.length != 0 ? (
                    muscleGroups.map((mg) => (
                      <Link
                        key={mg.id}
                        className="rounded-full border border-[rgba(240,246,252,0.1)] mr-2 px-2 py-1 mb-2 hover:border-hover-gray flex items-center"
                        to={`/muscle-groups/${mg.id}`}
                      >
                        <MdFitbit className="mr-1" />
                        {mg.name}
                      </Link>
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
            {!musclesAreLoading || musclesAreLoading === undefined ? (
              muscles ? (
                <div className="flex flex-wrap">
                  {muscles.length != 0 ? (
                    muscles.map((m) => (
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
        </div>
        <div className="relative">
          <div className="border border-gray w-[40rem] h-fit rounded-lg px-6 py-6">
            <h1 className="text-light-gray flex items-center">
              <BiDumbbell className="mr-1" size={16} />
              Exercise<span className="mx-1">/</span>
              <span className="text-blue">{exercise.name}</span>
            </h1>
            <div className="self-stretch border border-gray mt-4 h-[1px] rounded-lg" />
            <p className="pt-4 whitespace-pre-line">{exercise.description}</p>
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
            <div className="absolute left-0 w-[30%] flex items-start">
              <button
                type="button"
                onClick={() => navigate(-1)}
                className="bg-gray hover:border-hover-gray border border-[rgba(240,246,252,0.1)] rounded-lg py-1 px-3 mr-5"
              >
                Back
              </button>

              {auth.user?.roles.find(
                (r) => r.name === "Coach" || r.name == "Administrator",
              ) && (
                <Link
                  className="bg-gray hover:border-hover-gray border border-[rgba(240,246,252,0.1)] rounded-lg px-3 h-[30px] flex items-center"
                  to={"edit"}
                >
                  <AiFillEdit className="mr-1" size={18} />
                </Link>
              )}
            </div>

            <div className="absolute right-0 py-1 px-3 border border-gray rounded-lg flex items-center text-light-gray ml-2 justify-center">
              {!lastUpdater ? (
                <LoadingIcon classNames="mr-2 animate-spin fill-blue text-gray w-6 h-6" />
              ) : (
                <>
                  Last updated by:{" "}
                  <Link
                    to={`/coaches/${exercise.lastUpdaterId}`}
                    className="text-blue ml-1 hover:underline"
                  >
                    {lastUpdater!.firstName} {lastUpdater!.lastName}
                  </Link>
                </>
              )}
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default ExerciseDetailPage;

export const loader = async ({ params }: LoaderFunctionArgs) => {
  // Will raise an AxiosError if fetching fails

  const response = await axios.get<Exercise>(
    `${import.meta.env.VITE_API_BASE}/exercises/${params.exerciseId}`,
    {
      headers: {
        Accept: "application/json",
        Authorization: `bearer ${localStorage.getItem("token")}`,
      },
    },
  );

  return response.data;
};
