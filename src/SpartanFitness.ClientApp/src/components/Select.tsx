import { Transition } from "@headlessui/react";
import { Fragment, ReactNode, useEffect, useRef, useState } from "react";
import { BiCheck } from "react-icons/bi";
import { HiChevronDown } from "react-icons/hi";

export type SelectOption<T extends string | number> = {
  label: string;
  value: T;
};

type SingleSelectProps<T extends string | number> = {
  multiple?: false;
  value: SelectOption<T>;
  onChange: (value: SelectOption<T> | undefined) => void;
};

type MultipleSelectProps<T extends string | number> = {
  multiple: true;
  value: SelectOption<T>[];
  onChange: (value: SelectOption<T>[]) => void;
};

type SelectProps<T extends string | number> = {
  id: string | number;
  options: SelectOption<T>[];
  searchBar?: boolean;
  isLoading?: boolean;
  ifEmpty?: ReactNode;
  ifLoading?: ReactNode;
} & (SingleSelectProps<T> | MultipleSelectProps<T>);

const Select = <T extends string | number>({
  id,
  multiple,
  value,
  onChange,
  options,
  searchBar = false,
  isLoading = false,
  ifEmpty,
  ifLoading,
}: SelectProps<T>) => {
  const searchId = `select-search-${id}`;
  const containerId = `select-container-${id}`;
  const optionsId = `select-options-${id}`;
  const clearButtonId = `select-clear-button-${id}`;

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

  const selectOption = (option: SelectOption<T>) => {
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

  const isSelected = (option: SelectOption<T>) => {
    return multiple
      ? value.find((v) => v.label == option.label) != undefined
      : option.label === value.label;
  };

  const isHighlighted = (index: number) => index === highlightedIndex;

  useEffect(() => {
    const handler = (e: KeyboardEvent) => {
      if (e.target == searchRef.current) {
        switch (e.code) {
          case "ArrowDown":
            setHighlightedIndex(0);
            containerRef.current?.focus();
            break;

          case "Escape":
            // Will remove focus from searchRef and trigger closeOptions() from containerRef::OnBlur()
            searchRef.current?.blur();
            break;
        }
      } else {
        if (e.target != containerRef.current) return;

        switch (e.code) {
          case "Escape":
            closeOptions();
            break;

          case "Enter":
          case "Space":
            if (isOpen && filteredOptions.length > 0) {
              selectOption(filteredOptions[highlightedIndex ?? 0]);
            } else {
              setIsOpen((prev) => !prev);
            }
            break;

          case "ArrowUp":
          case "ArrowDown": {
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
            } else if (searchBar && newValue <= 0) {
              setHighlightedIndex(undefined);
              searchRef.current?.focus();
            }
            break;
          }
        }
      }
    };

    containerRef.current?.addEventListener("keydown", handler);

    return () => containerRef.current?.removeEventListener("keydown", handler);
  }, [isOpen, highlightedIndex, filteredOptions]);

  const closeOptions = () => {
    setIsOpen(false);
    setHighlightedIndex(undefined);
  };

  return (
    <>
      <div
        id={containerId}
        ref={containerRef}
        onBlur={(e) => {
          if (!containerRef.current?.contains(e.relatedTarget)) {
            closeOptions();
          } else if (e.relatedTarget === null && e.target.id !== containerId) {
            closeOptions();
          } else if (e.target == searchRef.current) {
            return;
          } else {
            closeOptions();
          }
        }}
        onClick={(e) => {
          const targetElement = e.target as Element;
          const isChild = containerRef.current?.contains(targetElement);

          if (e.target !== containerRef.current && !isChild) {
            return;
          } else if (
            isChild &&
            (targetElement.id === searchId ||
              targetElement.parentElement?.id === optionsId)
          ) {
            setIsOpen(true);
          } else {
            setIsOpen((prev) => !prev);
          }
        }}
        tabIndex={0}
        className={`relative min-w-[20rem] min-h-[1.5rem] border ${
          isOpen ? "border-blue" : "border-gray hover:border-hover-gray"
        } items-center gap-2 flex p-1 rounded-lg outline-none focus:border-blue select-none cursor-pointer`}
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
                  className="group flex items-center border border-gray rounded-lg py-[.15rem] px-[.5rem] gap-1 cursor-pointer bg-none outline-none hover:bg-gray focus:bg-gray"
                >
                  {v.label}
                  <span className="bg-none text-white border-none outline-none cursor-pointer text-lg">
                    &times;
                  </span>
                </button>
              ))
            : value?.label}
          {searchBar && (
            <input
              ref={searchRef}
              placeholder="Search..."
              id={searchId}
              value={query}
              onFocus={() => {
                setIsOpen(true);
              }}
              onChange={(e) => {
                setQuery(e.target.value);
              }}
              onSubmit={(e) => e.preventDefault()}
              className="shadow appearance-none px-1 text-white leading-tight bg-black outline-none w-stretch"
            />
          )}
        </span>
        <button
          type="button"
          id={clearButtonId}
          className="self-stretch flex items-center hover:bg-gray justify-center rounded-lg cursor-pointer"
          onClick={(e) => {
            e.stopPropagation();
            clearOptions();
          }}
        >
          <span className="bg-none text-white border-none outline-none cursor-pointer text-lg px-2">
            &times;
          </span>
        </button>
        <div className="bg-gray self-stretch w-[1px]" />
        <button
          type="button"
          className="self-stretch flex items-center hover:bg-gray justify-center mr-1 rounded-lg cursor-pointer p-1"
          onClick={(e) => {
            e.stopPropagation();
            setIsOpen((prev) => !prev);
          }}
        >
          <HiChevronDown size={16} />
        </button>
        <Transition
          show={isOpen}
          as={Fragment}
          leave="transition ease-in duration-100"
          leaveFrom="opacity-100"
          leaveTo="opacity-0"
        >
          <ul
            id={optionsId}
            className={`absolute m-0 p-2 list-none max-h-[15rem] overflow-y-auto border border-gray rounded-lg w-full left-0 top-[calc(100%+0.45rem)] z-[5] bg-black`}
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
                      closeOptions();
                    }
                  }}
                  key={option.value}
                  className={`py-[0.25rem] px-[.5rem] cursor-pointer flex items-center rounded-lg ${
                    isHighlighted(index) ? "bg-gray" : ""
                  }`}
                >
                  {isSelected(option) ? <BiCheck className="mr-1" /> : null}
                  {option.label}
                </li>
              ))}
            {!isLoading && filteredOptions.length === 0 && ifEmpty}
            {isLoading && ifLoading}
          </ul>
        </Transition>
      </div>
    </>
  );
};

export default Select;
