import { useState } from "react";
import Select, { SelectOption } from "../components/Select";

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
  const options = muscleGroups.map((mg) => ({ label: mg.name, value: mg.id }));
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
            <label>MuscleGroups</label>

            <Select
              multiple={true}
              value={value}
              options={options}
              onChange={setValue}
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