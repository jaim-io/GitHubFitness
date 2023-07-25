import axios from "axios";
import { useEffect, useRef, useState } from "react";
import { AiFillEdit } from "react-icons/ai";
import { BiDumbbell } from "react-icons/bi";
import { BsCloudUploadFill } from "react-icons/bs";
import { MdFitbit, MdOutlineBookmarkAdd } from "react-icons/md";
import { RxExit } from "react-icons/rx";
import { useLoaderData, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import LoadingIcon from "../../components/icons/LoadingIcon";
import useAuth from "../../hooks/useAuth";
import Muscle from "../../types/domain/Muscle";
import useMuscleGroupsByMuscleId from "../../hooks/useMuscleGroupsByMuscleId";

const MUSCLE_ENDPOINT = `${import.meta.env.VITE_API_BASE}/muscles`;

const EditMusclePage = () => {
  const navigate = useNavigate();
  const muscle = useLoaderData() as Muscle;
  const { auth } = useAuth();

  const [isLoading, setIsLoading] = useState(false);

  const [name, setName] = useState(muscle.name);
  const [description, setDescription] = useState(muscle.description);
  const descriptionRef = useRef<HTMLTextAreaElement>(null);
  const [image, setImage] = useState<string>(muscle.image);
  const [previewImage, setPreviewImage] = useState<string>(muscle.image);
  const [showImageUrlInputBar, setShowImageUrlInputBar] = useState(false);

  const [muscleGroups, , muscleGroupsAreLoading] = useMuscleGroupsByMuscleId(
    muscle.id,
  );

  const setDescriptionAreaHeight = () => {
    descriptionRef.current!.style.height =
      descriptionRef.current!.scrollHeight + "px";
  };

  useEffect(() => {
    setDescriptionAreaHeight();
  }, []);

  const popupActive = () => showImageUrlInputBar;

  const handleSaveChanges = async () => {
    setIsLoading(true);

    await axios
      .put(
        `${MUSCLE_ENDPOINT}/${muscle.id}/update`,
        {
          id: muscle.id,
          name: name,
          description: description,
          image: image,
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
        toast.success("Muscle group has been updated", {
          toastId: "muscle-group-updated",
          position: "bottom-right",
          autoClose: 5000,
          hideProgressBar: false,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
          progress: undefined,
          theme: "colored",
        });
        navigate(`/muscles/${muscle.id}`);
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
            popupActive() ? "opacity-50 hover:cursor-not-allowed" : ""
          }`}
          type="button"
          onClick={() => navigate(-1)}
        >
          <RxExit className="mr-1" /> Leave edit mode
        </button>
        <button
          className={`px-20 py-2 rounded-lg bg-dark-green hover:bg-light-green text-white flex items-center cursor-pointer ${
            popupActive() ? "hover:cursor-not-allowed opacity-50" : ""
          }`}
          onClick={handleSaveChanges}
          disabled={popupActive()}
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
            onClick={() => {
              setShowImageUrlInputBar((prev) => !prev);
            }}
          >
            <img
              src={previewImage}
              alt={`${muscle.name} image`}
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
            {muscleGroups && (
              <div className="flex flex-wrap">
                {muscleGroups.map((mg) => (
                  <span
                    key={mg.id}
                    className="rounded-full border border-[rgba(240,246,252,0.1)] mr-2 px-2 py-1 mb-2 hover:border-hover-gray flex items-center cursor-not-allowed"
                  >
                    <MdFitbit className="mr-1" />
                    {mg.name}{" "}
                  </span>
                ))}
              </div>
            )}
            {muscleGroupsAreLoading && <p>Muscles are loading</p>}
          </div>
        </div>

        <div className="relative">
          <div className="border border-gray w-[40rem] h-fit rounded-lg px-6 py-6">
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
                  onChange={(e) => setName(e.target.value)}
                />
              </span>
            </h1>
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
                  setDescriptionAreaHeight();
                  setDescription(e.target.value);
                }}
              />
            </p>
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

      <div
        className={`absolute top-[40%] left-[35%] border border-blue rounded-lg pt-2 z-10 bg-black w-[40rem] ${
          showImageUrlInputBar ? "" : "hidden"
        }`}
      >
        <p className="text-center mb-1">Image URL</p>

        <button
          type="button"
          onClick={() => {
            setShowImageUrlInputBar(false);
            setImage(previewImage);
          }}
          className="absolute top-1 left-1 rounded-lg flex items-center hover:bg-gray justify-center cursor-pointer"
        >
          <span className="bg-none text-white border-none outline-none cursor-pointer text-lg px-3">
            &times;
          </span>
        </button>

        <button
          type="button"
          onClick={() => {
            setShowImageUrlInputBar(false);
            setPreviewImage(image);
          }}
          className="absolute top-1 right-1 rounded-lg flex items-center hover:bg-gray justify-center cursor-pointer"
        >
          <span className="bg-none text-white border-none outline-none cursor-pointer text-lg px-3">
            &#x2713;
          </span>
        </button>

        <div className="flex w-full items-center justify-center">
          <input
            onChange={(e) => setImage(e.target.value)}
            value={image}
            className="shadow appearance-none border border-gray rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black"
            autoComplete="off"
            placeholder="https://example.com/your-image"
          />
        </div>
      </div>
    </>
  );
};

export default EditMusclePage;
