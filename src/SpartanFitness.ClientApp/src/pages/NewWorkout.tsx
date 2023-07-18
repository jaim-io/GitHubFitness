import { FormEvent, useState } from "react";
import { TbGhost2Filled } from "react-icons/tb";
import { Link } from "react-router-dom";
import LoadingIcon from "../components/Icons/LoadingIcon";
import Select from "../components/Select";
import TestWorkouts from "../components/TestWorkouts";

const NewWorkoutPage = () => {
  const [isLoading, setIsLoading] = useState(false);

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [image, setImage] = useState("");

  const [muscleIds, setMuscleIds] = useState<string[]>();
  const [muscleGroupIds, setMuscleGroupIds] = useState<string[]>();

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setIsLoading(true);
  };

  return (
    <div className="flex justify-center px-24 pt-6 pb-8 h-full min-h-[90vh]">
      <div className="w-[52rem]">
        <h1 className="text-2xl mb-4">Create a new workout</h1>

        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <label className="block text-white mb-2 ml-1">Workout name *</label>
            <input
              className="shadow appearance-none border border-gray hover:border-hover-gray rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black"
              id="workout-name"
              type="text"
              placeholder="Barbell bench press"
              value={name}
              onChange={(e) => setName(e.target.value)}
              required
              autoComplete="off"
            />
          </div>
          <div className="mb-4">
            <label className="flex text-white mb-2 ml-1 items-center">
              Description
              <p className="ml-1 text-light-gray text-sm">(optional)</p>
            </label>
            <input
              className="shadow appearance-none border border-gray hover:border-hover-gray rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black"
              id="description"
              type="text"
              placeholder="A chest workout which involves chest, delts and triceps..."
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              autoComplete="off"
            />
          </div>

          <div className="mb-4">
            <label className="block text-white mb-2 ml-1">Image URL *</label>
            <input
              className="shadow appearance-none border border-gray hover:border-hover-gray rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black"
              type="text"
              placeholder="https://google.com/workout-image"
              value={image}
              onChange={(e) => setImage(e.target.value)}
              required
              autoComplete="off"
            />
          </div>
          <div className="bg-gray self-stretch w-full h-[1px] mb-4 mt-6" />

          <TestWorkouts />

          <div className="bg-gray self-stretch w-full h-[1px] mb-4 mt-6" />
          <div className="w-full relative mt-6">
            <Link
              className="absolute left-0 bg-gray hover:border-hover-gray border border-[rgba(240,246,252,0.1)] rounded-lg py-1 px-3 flex justify-center items-center"
              to=".."
            >
              Cancel
            </Link>
            <button
              className="absolute right-0 bg-dark-green hover:bg-light-green border border-[rgba(240,246,252,0.1)] rounded-lg py-1 px-3 flex justify-center items-center"
              type="submit"
            >
              {!isLoading && <p>Create workout</p>}
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

export default NewWorkoutPage;
