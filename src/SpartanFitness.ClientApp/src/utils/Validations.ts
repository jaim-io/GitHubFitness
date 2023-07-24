export type ValidatonProps = {
  maxLength?: number;
};

export type ValidationResult =
  | { isValid: true }
  | { isValid: false; errorMsg: string };

export const validateName = (
  name: string,
  minLength?: number,
  maxLength?: number,
): ValidationResult => {
  let result: ValidationResult = { isValid: true };

  if (minLength && name.length < minLength) {
    result = {
      isValid: false,
      errorMsg: `Name has to be longer then ${minLength} characters.`,
    };
  }

  if (maxLength && name.length > maxLength) {
    result = {
      isValid: false,
      errorMsg: `Name cannot be longer then ${maxLength} characters.`,
    };
  }

  return result;
};

export const validateDescription = (
  description: string,
  minLength?: number,
  maxLength?: number,
): ValidationResult => {
  let result: ValidationResult = { isValid: true };

  if (minLength && description.length < minLength) {
    result = {
      isValid: false,
      errorMsg: `Description has to be longer then ${minLength} characters.`,
    };
  }

  if (maxLength && description.length > maxLength) {
    result = {
      isValid: false,
      errorMsg: `Description cannot be longer then ${maxLength} characters.`,
    };
  }

  return result;
};

const defaultUrlRegex = new RegExp("^https://");
export const validateDefaultUrl = (
  url: string,
  maxLength?: number,
): ValidationResult => {
  let result: ValidationResult = defaultUrlRegex.test(url)
    ? { isValid: true }
    : {
        isValid: false,
        errorMsg: "URL has to start with 'https://'.",
      };

  if (maxLength && url.length > maxLength) {
    result = {
      isValid: false,
      errorMsg: result.isValid
        ? `URL cannot be longer then ${maxLength} characters.`
        : `${result.errorMsg} URL cannot be longer then ${maxLength} characters.`,
    };
  }

  return result;
};

const youtubeUrlRegex = new RegExp("^https://www.youtube-nocookie.com/embed/");
export const validateYoutubeUrl = (
  url: string,
  maxLength?: number,
): ValidationResult => {
  let result: ValidationResult = youtubeUrlRegex.test(url)
    ? { isValid: true }
    : {
        isValid: false,
        errorMsg:
          "URL has to start with 'https://www.youtube-nocookie.com/embed/'.",
      };

  if (maxLength && url.length > maxLength) {
    result = {
      isValid: false,
      errorMsg: result.isValid
        ? `URL cannot be longer then ${maxLength} characters.`
        : `${result.errorMsg} URL cannot be longer then ${maxLength} characters.`,
    };
  }

  return result;
};
