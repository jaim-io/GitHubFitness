import { useState } from "react";

const UserSettingsPage = () => {
  const [persist, setPersist] = useState<boolean>(
    JSON.parse(localStorage.getItem("persist") ?? "false") || false,
  );

  const togglePersist = () =>
    setPersist((prev) => {
      localStorage.setItem("persist", String(!prev));
      return !prev;
    });

  return (
    <div className="flex justify-center pt-6 pb-20 h-full">
      <div className="relative">
        <div className="border border-gray w-[40rem] h-fit rounded-lg px-6 py-6">
          <div className="flex items-center">
            <input
              type="checkbox"
              checked={persist}
              onChange={togglePersist}
              className="w-4 h-4 border-gray rounded"
            />
            <label className="ml-2 text-sm font-medium text-gray-900 dark:text-gray-300">
              Keep me signed in
            </label>
          </div>
        </div>
      </div>
    </div>
  );
};

export default UserSettingsPage;
