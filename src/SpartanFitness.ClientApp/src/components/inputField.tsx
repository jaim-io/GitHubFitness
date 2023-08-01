import { ChangeEvent, ReactNode, useEffect, useState } from "react";
import {
  ValidationResult,
  StringValidatonProps,
} from "../utils/StringValidations";

type Props = {
  value: string;
  valueToMatch?: string;
  placeholder: string;
  label: string | ReactNode;
  onChange: (value: string) => void;
  validator?: (value: string, props: StringValidatonProps) => ValidationResult;
  validationProps?: StringValidatonProps;
  setIsValid?: (value: boolean) => void;
  type?: string;
  error?: string;
};

const InputField = ({
  value,
  placeholder = "",
  label,
  onChange,
  validator,
  validationProps,
  setIsValid,
  type,
  error: e,
}: Props) => {
  const [error, setError] = useState<string | undefined>(e);

  useEffect(() => {
    setError(e);
  }, [e]);

  const allValidationPropsPresent =
    validator !== undefined &&
    validationProps != undefined &&
    setIsValid !== undefined;
  const noValidationPropsPresent =
    validator === undefined &&
    validationProps === undefined &&
    setIsValid === undefined;

  if (!(allValidationPropsPresent || noValidationPropsPresent)) {
    throw new Error(
      `[Inputfield](Label: ${label}) Either all of the following props should be defined or none: validator, validationProps and setIsValid`,
    );
  }

  const handleValidation = (e: ChangeEvent<HTMLInputElement>) => {
    // All undefined
    if (noValidationPropsPresent) {
      return;
    }

    // All defined
    if (allValidationPropsPresent) {
      const validation = validator(e.target.value, validationProps);

      if (validation.isValid) {
        setError(undefined);
      } else {
        setError(validation.errorMsg);
      }
      setIsValid(validation.isValid);
    }
  };

  return (
    <>
      <label className="block text-white mb-2 ml-1">{label}</label>
      <input
        className="shadow appearance-none border border-gray hover:border-hover-gray rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black"
        type={type ?? "text"}
        placeholder={placeholder}
        value={value}
        onChange={(e) => {
          handleValidation(e);
          onChange(e.target.value);
        }}
        required
        autoComplete="off"
      />
      {error && (
        <div className="pt-2">
          <p className="shadow appearance-none border border-red rounded-lg w-full py-1 px-3 text-whiteas bg-black font-medium flex items-center">
            {error}
          </p>
        </div>
      )}
    </>
  );
};

export default InputField;
