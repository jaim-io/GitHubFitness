import { Listbox, Transition } from "@headlessui/react";
import { Fragment } from "react";
import { BiChevronDown, BiCheck } from "react-icons/bi";

type Props = {
  selected: string;
  options: string[];
  buttonText: string;
  onChange: (value: string) => void;
};

const ListBox = ({ selected, options, buttonText, onChange }: Props) => {
  return (
    <Listbox value={selected} onChange={onChange}>
      <div className="relative">
        <Listbox.Button className="relative w-full cursor-default rounded-lg border border-gray bg-[#262c31] hover:border-[#8B949E] hover:bg-gray py-[0.25rem] pl-3 pr-8 text-left shadow  min-w-full">
          <p className="block truncate">
            <span className="text-light-gray">{buttonText} </span>
            {selected}
          </p>
          <span className="pointer-events-none absolute inset-y-0 right-0 flex items-center pr-2">
            <BiChevronDown
              className="h-5 w-5 text-light-gray"
              aria-hidden="true"
            />
          </span>
        </Listbox.Button>
        <Transition
          as={Fragment}
          leave="transition ease-in duration-100"
          leaveFrom="opacity-100"
          leaveTo="opacity-0"
        >
          <Listbox.Options className="absolute mt-1 max-h-60 w-full min-w-[10rem] overflow-auto rounded-xl border border-gray bg-semi-black py-1 text-base shadow-lg focus:outline-none sm:text-sm z-10">
            <div className="my-1">
              {options.map((option, optionIdx) => (
                <Listbox.Option
                  key={optionIdx}
                  className={({ active }) =>
                    `relative cursor-default select-none py-[0.4rem] mx-2 pl-10 pr-4 text-white rounded-lg ${
                      active ? "bg-[#262c31]" : ""
                    }`
                  }
                  value={option}
                >
                  {({ selected }) => (
                    <>
                      <span
                        className={`block truncate ${
                          selected ? "font-medium" : "font-normal"
                        }`}
                      >
                        {option}
                      </span>
                      {selected ? (
                        <span className="absolute inset-y-0 left-0 bottom-1 flex items-center pl-3 text-white">
                          <BiCheck className="h-5 w-5" aria-hidden="true" />
                        </span>
                      ) : null}
                    </>
                  )}
                </Listbox.Option>
              ))}
            </div>
          </Listbox.Options>
        </Transition>
      </div>
    </Listbox>
  );
};

export default ListBox;
