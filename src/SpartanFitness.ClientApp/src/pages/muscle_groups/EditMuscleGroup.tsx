import axios from "axios";
import { useEffect, useRef, useState } from "react";
import Draggable from "react-draggable";
import { AiFillEdit } from "react-icons/ai";
import { BiDumbbell } from "react-icons/bi";
import { BsCloudUploadFill, BsExclamationCircle } from "react-icons/bs";
import { MdFitbit, MdOutlineBookmarkAdd } from "react-icons/md";
import { RxExit } from "react-icons/rx";
import { TbGhost2Filled } from "react-icons/tb";
import { useLoaderData, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import LoadingIcon from "../../components/icons/LoadingIcon";
import Select, { SelectOption } from "../../components/Select";
import useAuth from "../../hooks/useAuth";
import useMusclesByMuscleGroupId from "../../hooks/useMusclesByMuscleGroupId";
import Muscle from "../../types/domain/Muscle";
import MuscleGroup from "../../types/domain/MuscleGroup";
import useMuscles from "../../hooks/useMuscles";
import {
  StringValidatonProps,
  validateDefaultUrl,
  validateDescription,
  validateName,
} from "../../utils/StringValidations";

const MUSCLEGROUP_ENDPOINT = `${import.meta.env.VITE_API_BASE}/muscle-groups`;

const EditMuscleGroupPage = () => {
  const muscleGroup = useLoaderData() as MuscleGroup;
  const { auth } = useAuth();

  const muscleSelectorRef = useRef<HTMLDivElement>(null);

  const [isLoading, setIsLoading] = useState(false);

  const [initialMuscles, , initialMusclesAreLoading] =
    useMusclesByMuscleGroupId(muscleGroup.id);

  const [name, setName] = useState(muscleGroup.name);
  const [isValidName, setIsValidName] = useState(true);
  const [nameError, setNameError] = useState<string>();
  const nameValidationProps: StringValidatonProps = {
    minLength: 5,
    maxLength: 100,
  };

  const [description, setDescription] = useState(muscleGroup.description);
  const [isValidDescription, setIsValidDescription] = useState(true);
  const descriptionRef = useRef<HTMLTextAreaElement>(null);
  const [descriptionError, setDescriptionError] = useState<string>();
  const descriptionValidationProps: StringValidatonProps = {
    maxLength: 2048,
  };

  const [image, setImage] = useState<string>(muscleGroup.image);
  const [previewImage, setPreviewImage] = useState<string>(muscleGroup.image);
  const [isValidImage, setIsValidImage] = useState(true);
  const [imageError, setImageError] = useState<string>();
  const imageValidationProps: StringValidatonProps = {
    maxLength: 2048,
  };

  const isValidForm = isValidName && isValidDescription && isValidImage;

  const [selectedMuscles, setSelectedMuscles] = useState<
    SelectOption<string>[]
  >(
    initialMuscles
      ? initialMuscles?.map((m) => ({ label: m.name, value: m.id }))
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

  const [muscles, , musclesAreLoading] = useMuscles();

  useEffect(() => {
    if (
      Object.values(selectedMuscles).length == 0 &&
      initialMuscles !== undefined
    ) {
      setSelectedMuscles(
        initialMuscles.map((m) => ({ label: m.name, value: m.id })),
      );
    }
  }, [initialMuscles]);

  const displayedMuscles = muscles?.filter((mg) =>
    Object.values(selectedMuscles).find((smg) => mg.id === smg.value),
  );

  const muscleOptions = muscles?.map((m: Muscle) => ({
    label: m.name,
    value: m.id,
  }));

  const [showMuscleSelector, setShowMuscleSelector] = useState(false);
  const [showImageUrlInputBar, setShowImageUrlInputBar] = useState(false);

  const popupActive = () => showMuscleSelector || showImageUrlInputBar;

  const handleSaveChanges = async () => {
    if (!isValidForm) {
      return;
    }

    setIsLoading(true);

    await axios
      .put(
        `${MUSCLEGROUP_ENDPOINT}/${muscleGroup.id}`,
        {
          id: muscleGroup.id,
          name: name,
          muscleIds: displayedMuscles?.map((m) => m.id),
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
        navigate(`/muscle-groups/${muscleGroup.id}`);
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
            popupActive() || !isValidForm
              ? "hover:cursor-not-allowed opacity-50"
              : ""
          }`}
          onClick={handleSaveChanges}
          disabled={popupActive() || !isValidForm}
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
              alt={`${muscleGroup.name} image`}
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
            {displayedMuscles && (
              <div className="flex flex-wrap">
                {displayedMuscles.map((mg) => (
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
            className="absolute top-1 left-1 rounded-lg flex items-center hover:bg-gray justify-center cursor-pointer"
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

export default EditMuscleGroupPage;
