import axios, { AxiosError } from "axios";
import { useEffect, useState } from "react";
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
import useAuth from "../../hooks/useAuth";
import Muscle from "../../types/domain/Muscle";
import useMuscleGroupsByMuscleId from "../../hooks/useMuscleGroupsByMuscleId";
import { AiFillEdit } from "react-icons/ai";
import LoadingIcon from "../../components/icons/LoadingIcon";
import useUserSavedIds from "../../hooks/useUserSavedIds";

const USER_ENDPOINT = `${import.meta.env.VITE_API_BASE}/users`;

const MuscleDetailPage = () => {
  const muscle = useLoaderData() as Muscle;
  const { auth } = useAuth();
  const navigate = useNavigate();

  let [savedIds, savedIdsLoading]: [string[] | undefined, boolean] = [
    [],
    false,
  ];
  if (auth.user) {
    [savedIds, , savedIdsLoading] = useUserSavedIds(auth.user.id, "muscles");
  }

  const [saved, setSaved] = useState(savedIds?.includes(muscle.id) ?? false);
  useEffect(() => {
    if (savedIds) {
      setSaved(savedIds.includes(muscle.id));
    }
  }, [savedIds]);

  const [muscleGroups, , muscleGroupsAreLoading] = useMuscleGroupsByMuscleId(
    muscle.id,
  );

  const onError = (err: AxiosError) =>
    toast.error(
      err.code == "ERR_NETWORK"
        ? "Unable to reach the server"
        : err.response?.statusText ?? "An unexpected error occured",
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

  const handleSaving = async () => {
    setSaved((prev) => !prev);

    if (saved) {
      // Remove muscle
      await axios
        .delete(
          `${USER_ENDPOINT}/${auth.user?.id}/saved/muscles/${muscle.id}`,
          {
            headers: {
              Accept: "application/json",
              Authorization: `bearer ${localStorage.getItem("token")}`,
            },
          },
        )
        .then(() => {
          auth.user!.savedMuscleIds =
            auth.user?.savedMuscleIds.filter((id) => id !== muscle.id) ?? [];
        })
        .catch((err) => {
          onError(err);
        });
    } else {
      // Add muscle
      await axios
        .patch(
          `${USER_ENDPOINT}/${auth.user?.id}/saved/muscles`,
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
          auth.user!.savedMuscleIds.push(muscle.id);
        })
        .catch((err) => {
          onError(err);
        });
    }
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

          {auth.user && savedIdsLoading ? (
            <div
              role="status"
              className="pt-5 flex justify-center items-center"
            >
              <LoadingIcon classNames="mr-2 fill-blue text-gray w-6 h-6" />
              <span className="sr-only">Loading...</span>
            </div>
          ) : (
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
          )}

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

          <div className="mt-4">
            <div className="absolute left-0 w-[30%] flex items-start">
              <button
                type="button"
                onClick={() => navigate(-1)}
                className="bg-gray hover:border-hover-gray border border-[rgba(240,246,252,0.1)] rounded-lg py-1 px-3 mr-5"
              >
                Back
              </button>

              {auth.user?.roles.find((r) => r.name == "Administrator") && (
                <Link
                  className="bg-gray hover:border-hover-gray border border-[rgba(240,246,252,0.1)] rounded-lg px-3 h-[30px] flex items-center"
                  to={"edit"}
                >
                  <AiFillEdit className="mr-1" size={18} />
                </Link>
              )}
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
