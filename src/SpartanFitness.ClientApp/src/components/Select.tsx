import { Transition } from "@headlessui/react";
import { Fragment, ReactNode, useEffect, useRef, useState } from "react";
import { BiCheck } from "react-icons/bi";
import { HiChevronDown } from "react-icons/hi";

export type SelectOption = {
  label: string;
  value: string | number;
};

type SingleSelectProps = {
  multiple?: false;
  value: SelectOption;
  onChange: (value: SelectOption | undefined) => void;
};

type MultipleSelectProps = {
  multiple: true;
  value: SelectOption[];
  onChange: (value: SelectOption[]) => void;
};

type SelectProps = {
  options: SelectOption[];
  isLoading?: boolean;
  ifEmpty?: ReactNode;
} & (SingleSelectProps | MultipleSelectProps);

const Select = ({
  multiple,
  value,
  onChange,
  options,
  isLoading = false,
  ifEmpty,
}: SelectProps) => {
  const [isOpen, setIsOpen] = useState(false);
  const [highlightedIndex, setHighlightedIndex] = useState<number | undefined>(
    undefined,
  );
  const [query, setQuery] = useState<string>("");

  const containerRef = useRef<HTMLDivElement>(null);
  const searchRef = useRef<HTMLInputElement>(null);

  const filteredOptions = options.filter((o) =>
    o.label.toLowerCase().includes(query.toLowerCase()),
  );

  const clearOptions = () => {
    multiple ? onChange([]) : onChange(undefined);
    setQuery("");
  };

  const selectOption = (option: SelectOption) => {
    if (multiple) {
      value.find((v) => v.label == option.label) != undefined
        ? onChange(value.filter((o) => o.label !== option.label))
        : onChange([...value, option]);
    } else {
      if (!isSelected(option)) {
        onChange(option);
      }
    }
  };

  const isSelected = (option: SelectOption) => {
    return multiple
      ? value.find((v) => v.label == option.label) != undefined
      : option.label === value.label;
  };

  const isHighlighted = (index: number) => index === highlightedIndex;

  useEffect(() => {
    const handler = (e: KeyboardEvent) => {
      if (e.target == searchRef.current && e.code === "ArrowDown") {
        setHighlightedIndex(0);
        containerRef.current?.focus();
      } else {
        if (e.target) if (e.target != containerRef.current) return;
        switch (e.code) {
          case "Escape":
            setIsOpen(false);
            break;

          case "Enter":
          case "Space":
            if (isOpen) {
              if (filteredOptions.length > 0) {
                selectOption(filteredOptions[highlightedIndex ?? 0]);
              }
            } else {
              setIsOpen((prev) => !prev);
            }
            break;

          case "ArrowUp":
          case "ArrowDown":
            if (!isOpen) {
              setIsOpen(true);
              break;
            }

            const newValue =
              (highlightedIndex ?? 0) + (e.code === "ArrowDown" ? 1 : -1);

            if (
              newValue >= 0 &&
              newValue < filteredOptions.length &&
              highlightedIndex != newValue
            ) {
              setHighlightedIndex(newValue);
            } else if (newValue <= 0) {
              setHighlightedIndex(undefined);
              searchRef.current?.focus();
            }
            break;
        }
      }
    };

    containerRef.current?.addEventListener("keydown", handler);

    return () => containerRef.current?.removeEventListener("keydown", handler);
  }, [isOpen, highlightedIndex, filteredOptions]);

  return (
    <>
      <div
        id="select-container"
        ref={containerRef}
        onBlur={(e) => {
          if (e.relatedTarget === null && e.target.id !== "select-container") {
            setIsOpen(false);
          } else if (e.target == document.getElementById("select-search")) {
            return;
          } else {
            setIsOpen(false);
          }
        }}
        onClick={(e) => {
          const thisElement = document.getElementById("select-container");
          const targetElement = e.target as Element;
          const isChild = thisElement?.contains(targetElement);

          if (e.target !== thisElement && !isChild) {
            return;
          } else if (
            isChild &&
            (targetElement.id === "select-search" ||
              targetElement.parentElement?.id === "select-options")
          ) {
            setIsOpen(true);
          } else {
            setIsOpen((prev) => !prev);
          }
        }}
        tabIndex={0}
        className="relative min-w-[20rem] min-h-[1.5rem] border border-[#30363d] items-center gap-2 flex p-1 rounded-lg outline-none focus:border-[#30363d] select-none cursor-pointer"
      >
        <span className="grow flex gap-2 flex-wrap min-h-[2.2rem] pl-1">
          {multiple
            ? value.map((v) => (
                <button
                  key={v.value}
                  onClick={(e) => {
                    e.stopPropagation();
                    selectOption(v);
                  }}
                  className="group flex items-center border border-[#30363d] rounded-lg py-[.15rem] px-[.5rem] gap-1 cursor-pointer bg-none outline-none hover:bg-[#30363d] focus:bg-[#30363d]"
                >
                  {v.label}
                  <span className="bg-none text-white border-none outline-none cursor-pointer group-hover:text-red group-focus:text-red text-lg">
                    &times;
                  </span>
                </button>
              ))
            : value?.label}
          <input
            ref={searchRef}
            placeholder="Search..."
            id="select-search"
            value={query}
            onFocus={(e) => {
              setIsOpen(true);
            }}
            onChange={(e) => {
              setQuery(e.target.value);
            }}
            onSubmit={(e) => e.preventDefault()}
            className="shadow appearance-none px-1 text-white leading-tight bg-[#0d1117] outline-none w-stretch"
          />
        </span>
        <div
          id="select-clear-button"
          className="self-stretch flex items-center hover:bg-[#30363d] justify-center rounded-lg cursor-pointer"
          onClick={(e) => {
            e.stopPropagation();
            clearOptions();
          }}
        >
          <span className="bg-none text-white border-none outline-none cursor-pointer text-lg px-2">
            &times;
          </span>
        </div>
        <div className="bg-[#30363d] self-stretch w-[1px]"></div>
        <div className="self-stretch flex items-center hover:bg-[#30363d] justify-center mr-1 rounded-lg cursor-pointer p-1">
          <HiChevronDown size={16} />
        </div>
        <Transition
          show={isOpen}
          as={Fragment}
          leave="transition ease-in duration-100"
          leaveFrom="opacity-100"
          leaveTo="opacity-0"
        >
          <ul
            id="select-options"
            className={`absolute m-0 p-2 list-none max-h-[15rem] overflow-y-auto border border-[#30363d] rounded-lg w-full left-0 top-[calc(100%+0.45rem)] z-[100] bg-[#0d1117]`}
          >
            {!isLoading &&
              filteredOptions.map((option, index) => (
                <li
                  onMouseEnter={() => setHighlightedIndex(index)}
                  onMouseLeave={() => setHighlightedIndex(undefined)}
                  onClick={(e) => {
                    e.stopPropagation;
                    selectOption(option);
                    if (!multiple) {
                      setIsOpen(false);
                    }
                  }}
                  key={option.value}
                  className={`py-[0.25rem] px-[.5rem] cursor-pointer flex items-center rounded-lg ${
                    isHighlighted(index) ? "bg-[#30363d]" : ""
                  }`}
                >
                  {isSelected(option) ? <BiCheck className="mr-1" /> : null}
                  {option.label}
                </li>
              ))}
            {!isLoading && filteredOptions.length === 0 && ifEmpty}
            {isLoading && (
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
            )}
          </ul>
        </Transition>
      </div>
    </>
  );
};

export default Select;
