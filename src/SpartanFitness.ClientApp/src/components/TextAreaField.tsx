import { ReactNode, useEffect, useRef, useState } from "react";
import { BsExclamationCircle } from "react-icons/bs";
import {
  ValidationResult,
  StringValidatonProps,
} from "../utils/StringValidations";

type Props = {
  value: string;
  placeholder: string;
  label: string | ReactNode;
  onChange: (value: string) => void;
  validator: (value: string, props: StringValidatonProps) => ValidationResult;
  validationProps: StringValidatonProps;
  setIsValid: (value: boolean) => void;
};

const TexAreaField = ({
  value,
  placeholder = "",
  label,
  onChange,
  validator,
  validationProps,
  setIsValid,
}: Props) => {
  const [error, setError] = useState<string>();

  const ref = useRef<HTMLTextAreaElement>(null);

  const setTextAreaHeight = () => {
    const pixels =
      ref.current!.scrollHeight > 60 ? ref.current!.scrollHeight : 60;
    ref.current!.style.height = pixels + "px";
  };

  useEffect(() => {
    setTextAreaHeight();
  }, []);

  return (
    <>
      <label className="block text-white mb-2 ml-1">{label}</label>
      <textarea
        ref={ref}
        className="outline-none shadow appearance-none border border-gray hover:border-hover-gray rounded-lg w-full py-1.5 px-3 text-white focus:hover:border-[#408af5] focus:shadow-outline bg-black overflow-hidden min-h-[60px]"
        placeholder={placeholder}
        value={value}
        spellCheck={false}
        onChange={(e) => {
          const validation = validator(e.target.value, validationProps);

          if (validation.isValid) {
            setError(undefined);
          } else {
            setError(validation.errorMsg);
          }

          setIsValid(validation.isValid);
          setTextAreaHeight();
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

export default TexAreaField;
