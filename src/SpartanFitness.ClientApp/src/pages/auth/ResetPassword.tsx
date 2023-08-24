import { FormEvent, useState } from "react";
import { Link, useNavigate, useSearchParams } from "react-router-dom";
import LogoSvg from "../../../../../assets/logos/svgs/logo.svg";
import LoadingIcon from "../../components/icons/LoadingIcon";
import { BsFillEyeSlashFill, BsFillEyeFill } from "react-icons/bs";
import InputField from "../../components/InputField";
import {
  validatePassword,
  validateConfirmedPassword,
} from "../../utils/StringValidations";
import axios from "axios";
import { toast } from "react-toastify";
import { MessageResponse } from "../../types/responses/MessageResponse";
import { extractErrors } from "../../utils/ExtractErrors";
import useAuth from "../../hooks/useAuth";

const RESET_PASSWORD_ENDPOINT = `${
  import.meta.env.VITE_API_BASE
}/auth/reset-password`;

const ResetPassword = () => {
  const [searchParams] = useSearchParams();
  const id = searchParams.get("id");
  const token = searchParams.get("token");

  if (id === null || token === null) {
    throw new Error("Invalid URL for the email confirmation page.");
  }

  const { auth } = useAuth();
  const navigate = useNavigate();

  const [isLoading, setIsLoading] = useState(false);
  const [serverMessage, setServerMessage] = useState<string>();
  const [errors, setErrors] = useState<string[] | null>(null);

  const [password, setPassword] = useState("");
  const [passwordError, setPasswordError] = useState<string>();
  const [passwordIsValid, setPasswordIsValid] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  const [confirmedPassword, setConfirmedPassword] = useState("");
  const [confirmedPasswordError, setConfirmedPasswordError] =
    useState<string>();
  const [confirmedPasswordIsValid, setConfirmedPasswordIsValid] =
    useState(false);
  const [showConfirmedPassword, setShowConfirmedPassword] = useState(false);

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

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    if (!(passwordIsValid && confirmedPasswordIsValid)) {
      return;
    }

    e.preventDefault();

    setIsLoading(true);
    setErrors(null);

    try {
      await axios
        .post<MessageResponse>(
          RESET_PASSWORD_ENDPOINT,
          {
            userId: id,
            password: password,
            token: token,
          },
          {
            headers: { "Content-Type": "application/json" },
          },
        )
        .then((res) => setServerMessage(res.data.message))
        .catch((err) => {
          setIsLoading(false);

          if (err.response.status === 500) {
            toast.error("Unable to reach the server", {
              toastId: err.code,
              position: "bottom-right",
              autoClose: 5000,
              hideProgressBar: false,
              closeOnClick: true,
              pauseOnHover: true,
              draggable: true,
              progress: undefined,
              theme: "colored",
            });
            setErrors([
              "The server is unreachable, please try again at a later time.",
            ]);
            return;
          }

          if (err.response.status === 400) {
            setErrors(extractErrors(err.response.data.errors));
          }
        });
    } catch {
      /* empty */
    }

    setPassword("");
    setConfirmedPassword("");
    setIsLoading(false);
  };

  return (
    <>
      <div className="flex justify-center pt-28 pb-10">
        <div className="text-center">
          <Link to={"/"}>
            <img
              src={LogoSvg}
              className="w-[10rem] h-[10rem] mx-auto hover:rotate-8 hover:transition duration-150 ease-in-out"
              alt={"SpartanFitness Logo"}
            />
          </Link>
          <p>Reset password</p>
        </div>
      </div>

      <div className="flex justify-center pb-1">
        {serverMessage ? (
          <div className="w-full max-w-sm m-auto bg-semi-black shadow-xl rounded-lg px-8 pt-6 pb-5 mb-4 border border-gray">
            <p className="mb-4">
              {serverMessage} You can continue your journey by clicking on the
              buttons below.
            </p>
            <div className="grid grid-flow-col gap-2 justify-stretch">
              <button
                className="bg-gray hover:border-hover-gray border border-[rgba(240,246,252,0.1)] rounded-lg py-1"
                onClick={() => navigate("/")}
              >
                home
              </button>
              {auth.user ? (
                <button
                  className="bg-gray hover:border-hover-gray border border-[rgba(240,246,252,0.1)] rounded-lg py-1"
                  onClick={() => navigate("/logout")}
                >
                  logout
                </button>
              ) : (
                <button
                  className="bg-gray hover:border-hover-gray border border-[rgba(240,246,252,0.1)] rounded-lg py-1"
                  onClick={() => navigate("/login")}
                >
                  login
                </button>
              )}
            </div>
          </div>
        ) : (
          <form
            id="forgot-password-form"
            className="w-full max-w-sm m-auto bg-semi-black shadow-xl rounded-lg px-8 pt-6 pb-5 mb-4 border border-gray"
            onSubmit={(e) => handleSubmit(e)}
          >
            <div className="mb-3">
              <InputField
                value={password}
                onChange={handlePasswordChange}
                placeholder="******************"
                type={showPassword ? "text" : "password"}
                label={
                  <p className="flex items-center">
                    New password *{" "}
                    <span
                      className="ml-2 cursor-pointer hover:opacity-80"
                      onClick={() => setShowPassword((prev) => !prev)}
                    >
                      {showPassword ? (
                        <BsFillEyeSlashFill />
                      ) : (
                        <BsFillEyeFill />
                      )}
                    </span>
                  </p>
                }
                error={passwordError}
              />
            </div>

            <div className="mb-4">
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

            <div className="flex items-center justify-between pb-1">
              <button
                className={`bg-dark-green hover:bg-light-green text-white py-1.5 px-4 rounded-lg focus:outline-none focus:shadow-outline w-full block ${
                  passwordIsValid && confirmedPasswordIsValid
                    ? ""
                    : "cursor-not-allowed opacity-50"
                }`}
                type="submit"
                value="Submit"
                form="forgot-password-form"
                disabled={!passwordIsValid && confirmedPasswordIsValid}
              >
                {!isLoading && <p>Submit</p>}
                {(isLoading || isLoading == undefined) && (
                  <div className="flex items-center justify-center animate-pulse">
                    <LoadingIcon classNames="mr-2 text-white fill-white w-5 h-5" />
                    <p>Processing...</p>
                  </div>
                )}
              </button>
            </div>
          </form>
        )}
      </div>

      {errors && (
        <div className="flex justify-center pb-4">
          <div className="shadow appearance-none border border-red rounded-lg w-full py-3 px-3 text-whiteas bg-black font-medium max-w-sm text-center">
            {errors.map((e, i) => (
              <p key={`error-${i}`}>{e}</p>
            ))}
          </div>
        </div>
      )}
    </>
  );
};

export default ResetPassword;
