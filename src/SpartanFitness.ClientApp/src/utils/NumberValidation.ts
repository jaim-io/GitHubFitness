export type NumberValidatonProps = {
  minValue?: number;
  maxValue?: number;
};

export type ValidationResult =
  | { isValid: true }
  | { isValid: false; errorMsg: string };

export const validateSets = (
  value: number,
  props: NumberValidatonProps,
): ValidationResult => {
  let result: ValidationResult = { isValid: true };

  if (props.minValue && value < props.minValue) {
    result = {
      isValid: false,
      errorMsg: `The value of 'Sets' has to be equal to or higher than ${props.minValue}`,
    };
  }

  if (props.maxValue && value > props.maxValue) {
    result = {
      isValid: false,
      errorMsg: result.isValid
        ? `The value of 'Sets' has to be lower than ${props.maxValue}`
        : `${result.errorMsg} and lower than ${props.maxValue}`,
    };
  }

  return result;
};

export const validateRepsRatio = (
  minReps: number,
  maxReps: number,
): ValidationResult => {
  let result: ValidationResult;

  if (minReps > maxReps) {
    result = {
      isValid: false,
      errorMsg: "'Reps-min' has to be lower than or equal to 'Reps-max'",
    };
  } else {
    result = { isValid: true };
  }

  return result;
};

export const validateMinReps = (
  value: number,
  props: NumberValidatonProps,
): ValidationResult => {
  let result: ValidationResult = { isValid: true };

  if (props.minValue && value < props.minValue) {
    result = {
      isValid: false,
      errorMsg: `'Reps-min' has to be higher than ${props.minValue}`,
    };
  }

  if (props.maxValue && value > props.maxValue) {
    return {
      isValid: false,
      errorMsg: `'Reps-min' has to be lower than ${props.maxValue}`,
    };
  }

  return result;
};

export const validateMaxReps = (
  value: number,
  props: NumberValidatonProps,
): ValidationResult => {
  let result: ValidationResult = { isValid: true };

  if (props.minValue && value < props.minValue) {
    result = {
      isValid: false,
      errorMsg: `'Reps-max' has to be higher than ${props.minValue}`,
    };
  }

  if (props.maxValue && value > props.maxValue) {
    return {
      isValid: false,
      errorMsg: `'Reps-max' has to be lower than ${props.maxValue}`,
    };
  }

  return result;
};
