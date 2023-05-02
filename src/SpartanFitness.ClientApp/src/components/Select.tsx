import { Combobox, Transition } from "@headlessui/react";
import { useState, Fragment, useRef, useEffect } from "react";
import { BiCheck } from "react-icons/bi";
import { TbGhost2Filled } from "react-icons/tb";

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
} & (SingleSelectProps | MultipleSelectProps);

const Select = ({ multiple, value, onChange, options }: SelectProps) => {
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
        <span className="grow flex gap-2 flex-wrap min-h-[2.2rem]">
          {multiple
            ? value.map((v) => (
                <button
                  key={v.value}
                  onClick={(e) => {
                    e.stopPropagation();
                    selectOption(v);
                  }}
                  className="group flex items-center border border-[#30363d] rounded-lg py-[.15rem] px-[.25rem] gap-1 cursor-pointer bg-none outline-none hover:bg-[#30363d] focus:bg-[#30363d]"
                >
                  {v.label}
                  <span className="bg-none text-white border-none outline-none cursor-pointer group-hover:text-[#2f81f7] group-focus:text-[#2f81f7] text-lg">
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
            className="shadow appearance-none px-1 text-white leading-tight bg-[#0d1117] outline-none w-stretch" // border border-[#30363d] rounded-lg
          />
        </span>
        <button
          id="select-clear-button"
          type="button"
          onClick={(e) => {
            e.stopPropagation();
            clearOptions();
          }}
          className="bg-none text-white border-none outline-none cursor-pointer hover:text-[#2f81f7] focus:text-[#2f81f7]"
        >
          &times;
        </button>
        <div className="bg-[#30363d] self-stretch w-[1px]"></div>
        <div className="border-[0.25rem] border-transparent border-t-white translate-y-1/4 cursor-pointer"></div>
        <ul
          id="select-options"
          className={`absolute m-0 p-2 list-none max-h-[15rem] overflow-y-auto border border-[#30363d] rounded-lg w-full left-0 top-[calc(100%+0.45rem)] z-[100] bg-[#0d1117] ${
            isOpen ? "block" : "hidden"
          }`}
        >
          {filteredOptions.map((option, index) => (
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
              {isSelected(option) ? <BiCheck className="pr-1" /> : null}
              {option.label}
            </li>
          ))}
          {filteredOptions.length === 0 && (
            <p className="flex justify-center items-center py-1 cursor-default">
              No exercises found <TbGhost2Filled className="ml-1" size={20} />
            </p>
          )}
        </ul>
      </div>
    </>
  );
};

export default Select;
