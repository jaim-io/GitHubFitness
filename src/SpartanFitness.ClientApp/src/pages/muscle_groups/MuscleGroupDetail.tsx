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
import useMusclesByMuscleGroupId from "../../hooks/useMusclesByMuscleGroupId";
import MuscleGroup from "../../types/domain/MuscleGroup";
import { AiFillEdit } from "react-icons/ai";
import LoadingIcon from "../../components/icons/LoadingIcon";
import useUserSavedIds from "../../hooks/useUserSavedIds";

const USER_ENDPOINT = `${import.meta.env.VITE_API_BASE}/users`;

const MuscleGroupDetailPage = () => {
  const muscleGroup = useLoaderData() as MuscleGroup;
  const { auth } = useAuth();
  const navigate = useNavigate();

  let [savedIds, savedIdsLoading]: [string[] | undefined, boolean] = [
    [],
    false,
  ];
  if (auth.user) {
    [savedIds, , savedIdsLoading] = useUserSavedIds(
      auth.user.id,
      "muscle-groups",
    );
  }

  const [saved, setSaved] = useState(
    savedIds?.includes(muscleGroup.id) ?? false,
  );
  useEffect(() => {
    if (savedIds) {
      setSaved(savedIds.includes(muscleGroup.id));
    }
  }, [savedIds]);

  const [muscles, , musclesAreLoading] = useMusclesByMuscleGroupId(
    muscleGroup.id,
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
      // Remove MuscleGroup
      await axios
        .delete(
          `${USER_ENDPOINT}/${auth.user?.id}/saved/muscle-groups/${muscleGroup.id}`,
          {
            headers: {
              Accept: "application/json",
              Authorization: `bearer ${localStorage.getItem("token")}`,
            },
          },
        )
        .then(() => {
          auth.user!.savedMuscleGroupIds =
            auth.user?.savedMuscleGroupIds.filter(
              (id) => id !== muscleGroup.id,
            ) ?? [];
        })
        .catch((err) => {
          onError(err);
        });
    } else {
      // Add MuscleGroup
      await axios
        .patch(
          `${USER_ENDPOINT}/${auth.user?.id}/saved/muscle-groups`,
          {
            muscleGroupId: muscleGroup.id,
          },
          {
            headers: {
              Accept: "application/json",
              Authorization: `bearer ${localStorage.getItem("token")}`,
            },
          },
        )
        .then(() => {
          auth.user!.savedMuscleGroupIds.push(muscleGroup.id);
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
            src={muscleGroup.image}
            alt={`${muscleGroup.name} image`}
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
              <MdFitbit className="mr-1" size={16} />
              Muscle group<span className="mx-1">/</span>
              <span className="text-blue">{muscleGroup.name}</span>
            </h1>
            <div className="self-stretch border border-gray mt-4 h-[1px] rounded-lg" />
            <p className="pt-4 whitespace-pre-line">
              {muscleGroup.description}
            </p>
          </div>
          {/* <div className="border border-gray w-[40rem] h-fit rounded-lg px-6 py-6 mt-4 ">
            <iframe
              className="w-full h-[18.125rem]"
              src={muscleGroup.video}
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

export default MuscleGroupDetailPage;

export const loader = async ({ params }: LoaderFunctionArgs) => {
  // Will raise an AxiosError if fetching fails

  const response = await axios.get<MuscleGroup>(
    `${import.meta.env.VITE_API_BASE}/muscle-groups/${params.muscleGroupId}`,
    {
      headers: {
        Accept: "application/json",
        Authorization: `bearer ${localStorage.getItem("token")}`,
      },
    },
  );

  return response.data;
};
