export type StringValidatonProps = {
  minLength?: number;
  maxLength?: number;
};

export type ValidationResult =
  | { isValid: true }
  | { isValid: false; errorMsg: string };

export const validateName = (
  name: string,
  props: StringValidatonProps,
): ValidationResult => {
  let result: ValidationResult = { isValid: true };

  if (props.minLength && name.length < props.minLength) {
    result = {
      isValid: false,
      errorMsg: `Name has to be longer than ${props.minLength} characters`,
    };
  }

  if (props.maxLength && name.length > props.maxLength) {
    result = {
      isValid: false,
      errorMsg: `Name cannot be longer than ${props.maxLength} characters`,
    };
  }

  return result;
};

export const validateDescription = (
  description: string,
  props: StringValidatonProps,
): ValidationResult => {
  let result: ValidationResult = { isValid: true };

  if (props.minLength && description.length < props.minLength) {
    result = {
      isValid: false,
      errorMsg: `Description has to be longer than ${props.minLength} characters`,
    };
  }

  if (props.maxLength && description.length > props.maxLength) {
    result = {
      isValid: false,
      errorMsg: `Description cannot be longer than ${props.maxLength} characters`,
    };
  }

  return result;
};

const defaultUrlRegex = new RegExp("^https://");
export const validateDefaultUrl = (
  url: string,
  props: StringValidatonProps,
): ValidationResult => {
  let result: ValidationResult = defaultUrlRegex.test(url)
    ? { isValid: true }
    : {
        isValid: false,
        errorMsg: "URL has to start with 'https://'",
      };

  if (props.maxLength && url.length > props.maxLength) {
    result = {
      isValid: false,
      errorMsg: result.isValid
        ? `URL cannot be longer than ${props.maxLength} characters`
        : `${result.errorMsg} URL cannot be longer than ${props.maxLength} characters`,
    };
  }

  return result;
};

const youtubeUrlRegex = new RegExp("^https://www.youtube-nocookie.com/embed/");
export const validateYoutubeUrl = (
  url: string,
  props: StringValidatonProps,
): ValidationResult => {
  let result: ValidationResult = youtubeUrlRegex.test(url)
    ? { isValid: true }
    : {
        isValid: false,
        errorMsg:
          "URL has to start with 'https://www.youtube-nocookie.com/embed/'",
      };

  if (props.maxLength && url.length > props.maxLength) {
    result = {
      isValid: false,
      errorMsg: result.isValid
        ? `URL cannot be longer than ${props.maxLength} characters`
        : `${result.errorMsg} and cannot be longer than ${props.maxLength} characters`,
    };
  }

  return result;
};

const emailRegex = new RegExp(/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/);
export const validateEmail = (
  email: string,
  props: StringValidatonProps,
): ValidationResult => {
  let result: ValidationResult = emailRegex.test(email)
    ? { isValid: true }
    : {
        isValid: false,
        errorMsg: "Invalid e-mail adress",
      };

  if (props.maxLength && email.length > props.maxLength) {
    result = {
      isValid: false,
      errorMsg: result.isValid
        ? `E-mail adress cannot be longer than ${props.maxLength} characters`
        : `${result.errorMsg} and e-mail adress cannot be longer than ${props.maxLength} characters`,
    };
  }

  return result;
};

const passwordRegex = new RegExp(
  "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{0,}$",
);
export const validatePassword = (
  password: string,
  props: StringValidatonProps,
): ValidationResult => {
  let result: ValidationResult = passwordRegex.test(password)
    ? { isValid: true }
    : {
        isValid: false,
        errorMsg:
          "Password has to at least contain one uppercase letter, lowercase letter, number and special character",
      };

  if (!result.isValid) {
    return result;
  }

  if (props.minLength && password.length < props.minLength) {
    result = {
      isValid: false,
      errorMsg: `Password has to be longer than ${props.minLength} characters`,
      // errorMsg: result.isValid
      //   ? `Password has to be longer than ${props.minLength} characters`
      //   : `${result.errorMsg} and has to be longer than ${props.minLength} characters`,
    };
    return result;
  }

  if (props.maxLength && password.length > props.maxLength) {
    result = {
      isValid: false,
      errorMsg: `Password cannot be longer than ${props.maxLength} characters`,
      // errorMsg: result.isValid
      //   ? `Password cannot be longer than ${props.maxLength} characters`
      //   : `${result.errorMsg} and cannot be longer than ${props.maxLength} characters`,
    };
    return result;
  }

  return result;
};

export const validateConfirmedPassword = (
  password: string,
  confirmedPassword: string,
) => {
  return password === confirmedPassword
    ? { isValid: true }
    : {
        isValid: false,
        errorMsg: "Confirmed password does not match the password",
      };
};
