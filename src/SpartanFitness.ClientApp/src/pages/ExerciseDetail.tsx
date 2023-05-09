import axios from "axios";
import { Link, LoaderFunctionArgs, useRouteLoaderData } from "react-router-dom";
import useMuscleGroupsByIds from "../hooks/useMuscleGroupsByIds";
import useMusclesByIds from "../hooks/useMusclesByIds";
import Exercise from "../types/domain/Exercise";
import Muscle from "../types/domain/Muscle";
import MuscleGroup from "../types/domain/MuscleGroup";
import { BiDumbbell } from "react-icons/bi";
import { MdFitbit } from "react-icons/md";
import { SiElectron } from "react-icons/si";
import { MdOutlineBookmarkAdd, MdBookmarkAdded } from "react-icons/md";
import { useState } from "react";

// TODO: Add save/favorite button

const ExerciseDetailPage = () => {
  const exercise = useRouteLoaderData("exercise-details") as Exercise;

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

  const [saved, setSaved] = useState(false);
  // GetCoach

  return (
    <div className={"flex justify-center pt-6 pb-8 h-full min-h-[90vh]"}>
      <div className="mr-6 max-w-[18rem]">
        <img
          src={exercise.image}
          alt={`${exercise.name} image`}
          className="rounded-full border border-gray w-[18rem] h-[18rem] flex text-center leading-[9.5rem]"
        />
        {saved ? (
          <button
            className="w-full border border-[rgba(240,246,252,0.1)] rounded-lg mt-4 py-1 flex items-center justify-center hover:border-hover-gray bg-gray"
            onClick={() => setSaved(false)}
          >
            <MdBookmarkAdded className="mr-1 fill-[#e3b341]" size={16} />
            Saved
          </button>
        ) : (
          <button
            className="w-full border border-[rgba(240,246,252,0.1)] rounded-lg mt-4 py-1 flex items-center justify-center hover:border-hover-gray bg-gray"
            onClick={() => setSaved(true)}
          >
            <MdOutlineBookmarkAdd className="mr-1" size={16} />
            Save
          </button>
        )}

        <div className="mt-4">
          {muscleGroups && (
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
                <p>No muscle groups specified</p>
              )}
            </div>
          )}
          {muscleGroupsAreLoading && <p>Muscle groups are loading</p>}
        </div>

        <div className="self-stretch border border-gray rounded-lg my-2 h-[1px]" />

        <div className="mt-4">
          {muscles && (
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
                <p>No muscles specified</p>
              )}
            </div>
          )}
          {musclesAreLoading && <p>Muscles are loading</p>}
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
          <p className="pt-4">{exercise.description}</p>
        </div>
        <div className="border border-gray w-[40rem] h-fit rounded-lg px-6 py-6 mt-4 ">
          <iframe
            className="w-full h-[18.125rem]"
            src={exercise.video}
            title="YouTube video player"
            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
          />
        </div>

        <Link
          to=".."
          relative="path"
          className="absolute right-0 mt-4 mr-1 bg-gray hover:border-hover-gray border border-[rgba(240,246,252,0.1)] rounded-lg py-1 px-3"
        >
          Back
        </Link>
      </div>
    </div>
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
