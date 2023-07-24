import { ReactNode, useState } from "react";
import { BsExclamationCircle } from "react-icons/bs";
import { ValidationResult } from "../utils/Validations";

type Props = {
  value: string;
  placeholder: string;
  label: string | ReactNode;
  onChange: (value: string) => void;
  validator: (
    value: string,
    minLength?: number,
    maxLength?: number,
  ) => ValidationResult;
  validatorProps: ValidorProps;
  setIsValid: (value: boolean) => void;
};

type ValidorProps = {
  minLength?: number;
  maxLength?: number;
};

const inputField = ({
  value,
  placeholder = "",
  label,
  onChange,
  validator,
  validatorProps,
  setIsValid,
}: Props) => {
  const [error, setError] = useState<string>();

  return (
    <>
      <label className="block text-white mb-2 ml-1">{label}</label>
      <input
        className="shadow appearance-none border border-gray hover:border-hover-gray rounded-lg w-full py-1.5 px-3 text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline bg-black"
        type="text"
        placeholder={placeholder}
        value={value}
        onChange={(e) => {
          const validation = validator(
            e.target.value,
            validatorProps.minLength,
            validatorProps.maxLength,
          );

          if (validation.isValid) {
            setError(undefined);
          } else {
            setError(validation.errorMsg);
          }
          setIsValid(validation.isValid);
          onChange(e.target.value);
        }}
        required
        autoComplete="off"
      />
      {error && (
        <div className="pt-2">
          <p className="shadow appearance-none border border-red rounded-lg w-full py-1 px-3 text-whiteas bg-black font-medium flex items-center">
            <BsExclamationCircle className="text-red mr-1" size={14} /> {error}
          </p>
        </div>
      )}
    </>
  );
};

export default inputField;
