import axios from "axios";
import { useState } from "react";
import { MdBookmarkAdded, MdOutlineBookmarkAdd } from "react-icons/md";
import { SiElectron } from "react-icons/si";
import { MdFitbit } from "react-icons/md";
import {
  Link,
  LoaderFunctionArgs,
  useLoaderData,
  useNavigate,
} from "react-router-dom";
import { toast } from "react-toastify";
import useAuth from "../hooks/useAuth";
import Muscle from "../types/domain/Muscle";
import useMuscleGroup from "../hooks/useMuscleGroup";

const USER_ENDPOINT = `${import.meta.env.VITE_API_BASE}/users`;

const MuscleDetailPage = () => {
  const muscle = useLoaderData() as Muscle;
  const { auth } = useAuth();
  const navigate = useNavigate();
  const [saved, setSaved] = useState(
    Object.values(auth.user!.savedMusclesIds ?? []).includes(muscle.id),
  );

  const [muscleGroup, , musclesAreLoading] = useMuscleGroup(
    muscle.muscleGroupId,
  );

  const handleSaving = async () => {
    setSaved((prev) => !prev);

    const action = saved ? "remove" : "add";

    await axios
      .patch(
        `${USER_ENDPOINT}/${auth.user?.id}/saved/muscles/${action}`,
        {
          muscleId: muscle.id,
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
          auth.user!.savedMusclesIds.push(muscle.id);
        } else {
          auth.user!.savedMusclesIds =
            auth.user?.savedMusclesIds.filter((id) => id !== muscle.id) ?? [];
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
            src={muscle.image}
            alt={`${muscle.name} image`}
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
            {muscleGroup && (
              <div className="flex flex-wrap">
                <Link
                  key={muscleGroup.id}
                  className="rounded-full border border-[rgba(240,246,252,0.1)] mr-2 px-2 py-1 mb-2 hover:border-hover-gray flex items-center"
                  to={`/muscles/${muscleGroup.id}`}
                >
                  <MdFitbit className="mr-1" />
                  {muscleGroup.name}
                </Link>
              </div>
            )}
            {musclesAreLoading && <p>Muscles are loading</p>}
          </div>
        </div>

        <div className="relative">
          <div className="border border-gray w-[40rem] h-fit rounded-lg px-6 py-6">
            <h1 className="text-light-gray flex items-center">
              <SiElectron className="mr-1" size={16} />
              Muscle<span className="mx-1">/</span>
              <span className="text-blue">{muscle.name}</span>
            </h1>
            <div className="self-stretch border border-gray mt-4 h-[1px] rounded-lg" />
            <p className="pt-4 whitespace-pre-line">{muscle.description}</p>
          </div>
          {/* <div className="border border-gray w-[40rem] h-fit rounded-lg px-6 py-6 mt-4 ">
            <iframe
              className="w-full h-[18.125rem]"
              src={muscle.video}
              title="YouTube video player"
              allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
            />
          </div> */}

          <div className="mt-4">
            <div className="absolute left-0 w-[30%] flex items-start">
              <button
                type="button"
                onClick={() => navigate(-1)}
                className="bg-gray hover:border-hover-gray border border-[rgba(240,246,252,0.1)] rounded-lg py-1 px-3 mr-5"
              >
                Back
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default MuscleDetailPage;

export const loader = async ({ params }: LoaderFunctionArgs) => {
  // Will raise an AxiosError if fetching fails

  const response = await axios.get<Muscle>(
    `${import.meta.env.VITE_API_BASE}/muscles/${params.muscleId}`,
    {
      headers: {
        Accept: "application/json",
        Authorization: `bearer ${localStorage.getItem("token")}`,
      },
    },
  );

  return response.data;
};
