import { Link, useNavigate, useSearchParams } from "react-router-dom";
import LogoSvg from "../../../../../assets/logos/svgs/logo.svg";
import LoadingIcon from "../../components/icons/LoadingIcon";
import { ReactNode, useEffect, useState } from "react";
import axios from "axios";
import { MessageResponse } from "../../types/responses/MessageResponse";
import { toast } from "react-toastify";
import useAuth from "../../hooks/useAuth";

const CONFIRM_EMAIL_ENDPOINT = `${
  import.meta.env.VITE_API_BASE
}/auth/confirm-email`;

const ConfirmEmailPage = () => {
  const { auth } = useAuth();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const id = searchParams.get("id");
  const token = searchParams.get("token");

  if (id === null || token === null) {
    throw new Error("Invalid URL for the email confirmation page.");
  }

  const [isLoading, setIsLoading] = useState(true);
  const [serverMessage, setServerMessage] = useState<string>();
  const [error, setError] = useState<string | ReactNode | undefined>();

  useEffect(() => {
    const confirmEmail = async () => {
      setIsLoading(true);

      await axios
        .get<MessageResponse>(
          `${CONFIRM_EMAIL_ENDPOINT}?id=${id}&token=${token}`,
          {
            headers: {
              Accept: "application/json",
            },
          },
        )
        .then((res) => {
          setIsLoading(false);
          setServerMessage(res.data.message);
        })
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
            setError(
              "The server is unreachable, please try again at a later time.",
            );
            return;
          }

          if (err.response.status === 400) {
            setError(
              <span>
                There seems to be an invalid URL for the email confirmation
                page. Please contact the support team at{" "}
                <a
                  href="mailto:support@spartanfitness.com?subject=Invalid URL error while confirming e-mail address"
                  className="underline text-blue"
                >
                  support@spartanfitness.com
                </a>
                .
              </span>,
            );
          }

          throw new Error("An unexpected error occured.");
        });
    };

    if (auth.user) {
      navigate("/");
    }

    confirmEmail();
  }, []);

  return (
    <div className="flex justify-center pt-28 pb-2">
      <div className="flex justify-center items-center mb-4">
        <div>
          <Link to={"/"}>
            <img
              src={LogoSvg}
              className="w-[15rem] h-[15rem] mx-auto hover:rotate-8 hover:transition duration-150 ease-in-out"
              alt={"SpartanFitness Logo"}
            />
          </Link>
          {serverMessage !== undefined && (
            <div className="flex justify-center">
              <p className="px-10 pt-5 pb-5 border border-gray max-w-sm rounded-lg">
                <>
                  Sign in{" "}
                  <Link to="/login" className="text-blue">
                    here
                  </Link>
                  .
                </>
              </p>
            </div>
          )}
        </div>

        <div className="w-full max-w-[34rem] bg-semi-black shadow-xl rounded-lg px-4 py-4 ml-10 border border-gray relative">
          Hi new Spartan,
          <br />
          <br />
          {isLoading ? (
            <>
              Thank your for signing up at Spartan Fitness! Please standby while
              we are confirming your e-mail address.
              <LoadingIcon classNames="mr-2 animate-spin fill-blue text-gray w-8 h-8 absolute right-0 bottom-2" />
            </>
          ) : (
            <span>{serverMessage || error}</span>
          )}
          <br />
          <br />
          Kind regards,
          <br />
          <br />
          <span className="flex items-center">
            The Spartan Fitness team
            <img
              src={LogoSvg}
              className="w-[1rem] h-[1rem] ml-1"
              alt={"SpartanFitness Logo"}
            />
          </span>
        </div>
      </div>
    </div>
  );
};
export default ConfirmEmailPage;
