import { FormEvent, useState } from "react";
import { BsFillEyeFill, BsFillEyeSlashFill } from "react-icons/bs";
import InputField from "../../components/InputField";
import {
  validateConfirmedPassword,
  validateDefaultUrl,
  validateEmail,
  validateName,
  validatePassword,
} from "../../utils/StringValidations";
import axios from "axios";
import { toast } from "react-toastify";
import AuthenticationResponse from "../../types/authentication/AuthenticationResponse";
import useAuth from "../../hooks/useAuth";
import { Link, useNavigate } from "react-router-dom";
import Exception from "../../types/domain/Exception";
import LoadingIcon from "../../components/icons/LoadingIcon";
import LogoSvg from "../../assets/logo.svg";

const REGISTER_ENDPOINT = `${import.meta.env.VITE_API_BASE}/auth/register`;

const SignUpPage = () => {
  const { setAuth } = useAuth();
  const navigate = useNavigate();

  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [image, setImage] = useState("");
  const [password, setPassword] = useState("");
  const [confirmedPassword, setConfirmedPassword] = useState("");

  const [, setError] = useState<Exception>();
  const [confirmedPasswordError, setConfirmedPasswordError] =
    useState<string>();
  const [passwordError, setPasswordError] = useState<string>();

  const [firstNameIsValid, setFirstNameIsValid] = useState(false);
  const [lastNameIsValid, setLastNameIsValid] = useState(false);
  const [emailIsValid, setEmailIsValid] = useState(false);
  const [imageIsValid, setImageIsValid] = useState(false);
  const [passwordIsValid, setPasswordIsValid] = useState(false);
  const [confirmedPasswordIsValid, setConfirmedPasswordIsValid] =
    useState(false);
  const formIsValid =
    firstNameIsValid &&
    lastNameIsValid &&
    emailIsValid &&
    imageIsValid &&
    passwordIsValid &&
    confirmedPasswordIsValid;

  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmedPassword, setShowConfirmedPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!formIsValid) {
      setError({
        message: "Invalid form",
        code: "FORM_INVALID",
      });
      return;
    }

    setIsLoading(true);

    try {
      await axios
        .post<AuthenticationResponse>(
          REGISTER_ENDPOINT,
          {
            email: email,
            password: password,
            firstName: firstName,
            lastName: lastName,
            image: image,
          },
          {
            headers: { "Content-Type": "application/json" },
          },
        )
        .then((res) => {
          if (res.data.id) {
            // Email verification & 2FA
            setAuth(res.data);
            navigate("/");
          }
        })
        .catch((err) => {
          setIsLoading(false);
          setError({
            message: err.message,
            code: err.code,
          });
          toast.error(
            err.code == "ERR_NETWORK"
              ? "Unable to reach the server"
              : err.response.status == 400
              ? "Invalid credentials."
              : err.response.statusText,
            {
              toastId: err.code,
              position: "bottom-right",
              autoClose: 5000,
              hideProgressBar: false,
              closeOnClick: true,
              pauseOnHover: true,
              draggable: true,
              progress: undefined,
              theme: "colored",
            },
          );
        });
    } catch {
      /* empty */
    }

    setPassword("");
    setConfirmedPassword("");
  };

  const handlePasswordChange = (value: string) => {
    const passwordValidation = validatePassword(value, {
      minLength: 10,
      maxLength: 50,
    });

    if (passwordValidation.isValid) {
      setPasswordError(undefined);
    } else {
      setPasswordError(passwordValidation.errorMsg);
    }
    setPasswordIsValid(passwordValidation.isValid);

    if (confirmedPassword !== "") {
      const confirmedPasswordValidation = validateConfirmedPassword(
        value,
        confirmedPassword,
      );

      if (confirmedPasswordValidation.isValid) {
        setConfirmedPasswordError(undefined);
      } else {
        setConfirmedPasswordError(confirmedPasswordValidation.errorMsg);
      }
      setConfirmedPasswordIsValid(confirmedPasswordValidation.isValid);
    }

    setPassword(value);
  };

  const handleConfirmedPasswordChange = (value: string): void => {
    const validation = validateConfirmedPassword(password, value);

    if (validation.isValid) {
      setConfirmedPasswordError(undefined);
    } else {
      setConfirmedPasswordError(validation.errorMsg);
    }
    setConfirmedPasswordIsValid(validation.isValid);

    setConfirmedPassword(value);
  };

  return (
    <div className="flex justify-center pt-28 pb-10">
      <div className="flex justify-center items-center">
        <div>
          <Link to={"/"}>
            <img
              src={LogoSvg}
              className="w-[15rem] h-[15rem] mx-auto hover:rotate-8 hover:transition duration-150 ease-in-out"
              alt={"SpartanFitness Logo"}
            />
          </Link>
          <div className="flex justify-center">
            <p className="px-10 pt-5 pb-5 mb-4 border border-gray max-w-sm rounded-lg">
              Already a Spartan?{" "}
              <Link to="/login" className="text-blue">
                Sign in
              </Link>
              .
            </p>
          </div>
        </div>
      </div>

      <form
        id="login-form"
        onSubmit={(e) => handleSubmit(e)}
        className="w-full max-w-md bg-semi-black shadow-xl rounded-lg px-4 pt-3 pb-5 mb-4 ml-10 border border-gray"
      >
        <div className="mt-3">
          <div className="px-4">
            <InputField
              value={firstName}
              onChange={setFirstName}
              placeholder="John"
              label="Firstname *"
              validator={validateName}
              validationProps={{ minLength: 2, maxLength: 100 }}
              setIsValid={setFirstNameIsValid}
            />
          </div>
        </div>

        <div className="mt-3">
          <div className="px-4">
            <InputField
              value={lastName}
              onChange={setLastName}
              placeholder="Doe"
              label="Lastname *"
              validator={validateName}
              validationProps={{ minLength: 2, maxLength: 100 }}
              setIsValid={setLastNameIsValid}
            />
          </div>
        </div>

        <div className="mt-3">
          <div className="px-4">
            <InputField
              value={email}
              onChange={setEmail}
              placeholder="johndoe@gmail.com"
              label="E-mail address *"
              validator={validateEmail}
              validationProps={{ minLength: 5, maxLength: 100 }}
              setIsValid={setEmailIsValid}
            />
          </div>
        </div>

        <div className="mt-3">
          <div className="px-4">
            <InputField
              value={image}
              onChange={setImage}
              placeholder="https://google.com/my-image"
              label="Profile image *"
              validator={validateDefaultUrl}
              validationProps={{ minLength: 10, maxLength: 100 }}
              setIsValid={setImageIsValid}
            />
          </div>
        </div>

        <div className="mt-3">
          <div className="px-4">
            <InputField
              value={password}
              onChange={handlePasswordChange}
              placeholder="******************"
              type={showPassword ? "text" : "password"}
              label={
                <p className="flex items-center">
                  Password *{" "}
                  <span
                    className="ml-2 cursor-pointer hover:opacity-80"
                    onClick={() => setShowPassword((prev) => !prev)}
                  >
                    {showPassword ? <BsFillEyeSlashFill /> : <BsFillEyeFill />}
                  </span>
                </p>
              }
              error={passwordError}
            />
          </div>
        </div>

        <div className="mt-3">
          <div className="px-4">
            <InputField
              value={confirmedPassword}
              onChange={handleConfirmedPasswordChange}
              placeholder="******************"
              type={showConfirmedPassword ? "text" : "password"}
              label={
                <p className="flex items-center">
                  Confirm password *{" "}
                  <span
                    className="ml-2 cursor-pointer hover:opacity-80"
                    onClick={() => setShowConfirmedPassword((prev) => !prev)}
                  >
                    {showConfirmedPassword ? (
                      <BsFillEyeSlashFill />
                    ) : (
                      <BsFillEyeFill />
                    )}
                  </span>
                </p>
              }
              error={confirmedPasswordError}
            />
          </div>
        </div>

        <div className="mt-4 px-4">
          <button
            className={`bg-dark-green hover:bg-light-green text-white py-1.5 px-4 rounded-lg focus:outline-none focus:shadow-outline block w-full ${
              formIsValid ? "" : "cursor-not-allowed opacity-50"
            }`}
            type="submit"
            value="Submit"
            form="login-form"
            disabled={!formIsValid}
          >
            {!isLoading && <p>Sign up</p>}
            {(isLoading || isLoading == undefined) && (
              <div className="flex items-center justify-center animate-pulse">
                <LoadingIcon classNames="mr-2 text-white fill-white w-5 h-5" />
                <p>Signing up...</p>
              </div>
            )}
          </button>
        </div>
      </form>
    </div>
  );
};

export default SignUpPage;
