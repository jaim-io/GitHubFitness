import { Combobox, Transition } from "@headlessui/react";
import { useState, Fragment, useRef, useEffect } from "react";
import { BiCheck } from "react-icons/bi";

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

  const containerRef = useRef<HTMLDivElement>(null);

  const clearOptions = () => (multiple ? onChange([]) : onChange(undefined));

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
      if (e.target != containerRef.current) return;
      switch (e.code) {
        case "Escape":
          setIsOpen(false);
          break;

        case "Enter":
        case "Space":
          setIsOpen((prev) => !prev);
          if (isOpen) selectOption(options[highlightedIndex ?? 0]);
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
            newValue < options.length &&
            highlightedIndex != newValue
          ) {
            setHighlightedIndex(newValue);
          }
          break;
      }
    };
    containerRef.current?.addEventListener("keydown", handler);

    return () => containerRef.current?.removeEventListener("keydown", handler);
  }, [isOpen, highlightedIndex, options]);

  return (
    <>
      <div
        ref={containerRef}
        onBlur={() => setIsOpen(false)}
        onClick={() => setIsOpen((prev) => !prev)}
        tabIndex={0}
        className="relative min-w-[20rem] min-h-[1.5rem] border border-[#30363d] items-center gap-2 flex p-2 rounded-lg outline-none focus:border-[#30363d] select-none cursor-pointer"
      >
        <span className="grow flex gap-2 flex-wrap">
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
        </span>
        <button
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
          className={`absolute m-0 p-2 list-none max-h-[15rem] overflow-y-auto border border-[#30363d] rounded-lg w-full left-0 top-[calc(100%+0.25rem)] z-[100] bg-[#0d1117] ${
            isOpen ? "block" : "hidden"
          }`}
        >
          {options.map((option, index) => (
            <li
              onMouseEnter={() => setHighlightedIndex(index)}
              onMouseLeave={() => setHighlightedIndex(undefined)}
              onClick={(e) => {
                e.stopPropagation;
                selectOption(option);
                setIsOpen(false);
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
        </ul>
      </div>
    </>
  );
};

export default Select;
