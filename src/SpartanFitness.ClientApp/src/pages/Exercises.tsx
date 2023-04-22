import { ToastContainer, toast } from "react-toastify";
import ExerciseCard from "../components/ExerciseCard";
import { Link } from "react-router-dom";
import { Listbox } from "@headlessui/react";
import useExercises from "../hooks/useExercises";

const ExercisesPage = () => {
  const result = useExercises();
  const [_, exercises] = result.extract();

  return (
    <div className="px-24 pt-6">
      <Link
        to=""
        className="text-xl font-semibold text-[#2f81f7] hover:underline hover:underline-[#2f81f7] max-w-[8rem]"
      >
        All exercises
      </Link>

      <ul className="flex justify-center gap-4">
        <p>Search-balk</p>

        <p>sort-by-date</p>

        {/* <Listbox value={}> 
        <Listbox.Button></Listbox.Button>
        </Listbox> */}

        <p>new</p>
      </ul>

      <ul className="flex flex-wrap gap-4 justify-center mb-4 pt-6">
        {exercises &&
          exercises.map((e) => <ExerciseCard exercise={e} key={e.id} />)}
      </ul>

      <ToastContainer
        position="bottom-right"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
        theme="colored"
      />
    </div>
  );
};

export default ExercisesPage;
