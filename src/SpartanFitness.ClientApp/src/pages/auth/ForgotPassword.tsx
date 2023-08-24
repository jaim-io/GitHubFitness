import { FormEvent, useState } from "react";
import { Link } from "react-router-dom";
import LogoSvg from "../../../../../assets/logos/svgs/logo.svg";
import InputField from "../../components/InputField";
import LoadingIcon from "../../components/icons/LoadingIcon";
import { validateEmail } from "../../utils/StringValidations";
import { MessageResponse } from "../../types/responses/MessageResponse";
import axios from "axios";
import { toast } from "react-toastify";
import { extractErrors } from "../../utils/ExtractErrors";

const FORGOT_PASSWORD_ENDPONT = `${
  import.meta.env.VITE_API_BASE
}/auth/forgot-password`;

const ForgotPasswordPage = () => {
  const [errors, setErrors] = useState<string[] | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const [email, setEmail] = useState("");
  const [emailIsValid, setEmailIsValid] = useState(false);

  const [serverMessage, setServerMessage] = useState<string>();

  const onSubmit = async (e: FormEvent<HTMLFormElement>) => {
    if (emailIsValid) {
      await handleRequest(e);
      return;
    }
  };

  const handleRequest = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setIsLoading(true);
    setErrors(null);

    try {
      await axios
        .post<MessageResponse>(
          FORGOT_PASSWORD_ENDPONT,
          {
            emailAddress: email,
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

    setEmail("");
    setIsLoading(false);
  };

  const resetData = () => {
    setEmail("");
    setServerMessage(undefined);
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
          <p>Forgot password</p>
        </div>
      </div>

      <div className="flex justify-center pb-1">
        {serverMessage ? (
          <div className="w-full max-w-sm m-auto bg-semi-black shadow-xl rounded-lg px-8 pt-6 pb-5 mb-4 border border-gray">
            <p className="mb-4">{serverMessage}</p>
            <button
              onClick={resetData}
              className="bg-dark-green hover:bg-light-green text-white py-1.5 px-4 rounded-lg focus:outline-none focus:shadow-outline w-full block"
            >
              Try again
            </button>
          </div>
        ) : (
          <form
            id="forgot-password-form"
            className="w-full max-w-sm m-auto bg-semi-black shadow-xl rounded-lg px-8 pt-6 pb-5 mb-4 border border-gray"
            onSubmit={(e) => onSubmit(e)}
          >
            <div className="mb-4">
              <InputField
                value={email}
                onChange={setEmail}
                placeholder="johndoe@gmail.com"
                label="Please enter your e-mail address"
                validator={validateEmail}
                validationProps={{ minLength: 5, maxLength: 100 }}
                setIsValid={setEmailIsValid}
              />
            </div>

            <div className="flex items-center justify-between pb-1">
              <button
                className={`bg-dark-green hover:bg-light-green text-white py-1.5 px-4 rounded-lg focus:outline-none focus:shadow-outline w-full block ${
                  emailIsValid ? "" : "cursor-not-allowed opacity-50"
                }`}
                type="submit"
                value="Submit"
                form="forgot-password-form"
                disabled={!emailIsValid}
              >
                {!isLoading && <p>Submit request</p>}
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

      <div className="flex justify-center">
        <p className="px-10 pt-5 pb-5 mb-4 border border-gray max-w-sm rounded-lg">
          Not a Spartan yet?{" "}
          <Link to="/signup" className="text-blue">
            Create an account
          </Link>
          .
        </p>
      </div>
    </>
  );
};

export default ForgotPasswordPage;
