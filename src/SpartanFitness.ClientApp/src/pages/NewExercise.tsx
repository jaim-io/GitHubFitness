import { useState } from "react";
import Select, { SelectOption } from "../components/Select";
import useMuscleGroups from "../hooks/useMuscleGroups";
import { TbGhost2Filled } from "react-icons/tb";

const muscleGroups = [
  {
    id: "1",
    name: "Chest",
  },
  {
    id: "2",
    name: "Tricep",
  },
  {
    id: "3",
    name: "Bicep",
  },
];

const NewExercisePage = () => {
  const [result, isLoading] = useMuscleGroups();
  const [_, page] = result.extract();

  const options = page?.muscleGroups.map((mg) => ({
    label: mg.name,
    value: mg.id,
  }));

  const [value, setValue] = useState<SelectOption[]>([]);

  return (
    <div className="flex justify-center px-24 pt-6 pb-8 h-full min-h-[90vh]">
      <div className="w-[42rem]">
        <h1 className="text-2xl">Create a new exercise</h1>

        <form>
          <div className="mb-4">
            <label className="block text-white mb-2 ml-1">
              Exercise name *
            </label>
            <input
              className="shadow appearance-none border border-[#30363d] rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-[#2f81f7] focus:shadow-outline bg-[#0d1117]"
              id="exercise-name"
              type="text"
              placeholder="Barbell bench press"
              required
              autoComplete="off"
            />
          </div>
          <div className="mb-4">
            <label className="flex text-white mb-2 ml-1 items-center">
              Description
              <p className="ml-1 text-[#7D8590] text-sm">(optional)</p>
            </label>
            <input
              className="shadow appearance-none border border-[#30363d] rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-[#2f81f7] focus:shadow-outline bg-[#0d1117]"
              id="description"
              type="text"
              placeholder="A chest exercise which involves chest, delts and triceps..."
              required
              autoComplete="off"
            />
          </div>
          <div>
            <label className="flex text-white mb-2 ml-1 items-center">
              Muscle groups
              <p className="ml-1 text-[#7D8590] text-sm">(optional)</p>
            </label>

            <Select
              searchBar={true}
              multiple={true}
              value={value}
              options={options ?? []}
              onChange={setValue}
              isLoading={isLoading}
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
                  <svg
                    aria-hidden="true"
                    className="w-8 h-8 mr-2 text-gray-200 animate-spin dark:text-gray-600 fill-[#2f81f7]"
                    viewBox="0 0 100 101"
                    fill="none"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z"
                      fill="currentColor"
                    />
                    <path
                      d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z"
                      fill="currentFill"
                    />
                  </svg>
                  <span className="sr-only">Loading...</span>
                </div>
              }
            />
          </div>

          {/* <div>
            <label>Image</label>
            <input />
          </div>
          <div>
            <label>Video</label>
            <input />
          </div> */}
        </form>
      </div>
    </div>
  );
};

export default NewExercisePage;
