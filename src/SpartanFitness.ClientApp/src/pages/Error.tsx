import { useRouteError } from "react-router-dom";
import Footer from "../components/Footer";
import MainNavigation from "../components/navigation/MainNavigation";
import { AxiosError } from "axios";
import { BiErrorCircle } from "react-icons/bi";

const ErrorPage = () => {
  const error = useRouteError();

  let title = "An error occured.";
  let message = "Something went wrong.";

  if (error instanceof Error) {
    message = error.message;
  } else if (error instanceof AxiosError) {
    if (error.response?.status === 400) {
      title = "Not found.";
      message = "Could not find resource or page.";
    }
  }

  return (
    <>
      <MainNavigation />
      <div className="flex justify-center py-28">
        <div className="w-full max-w-[34rem] bg-semi-black shadow-xl rounded-lg px-4 py-6 mb-4 ml-10 border border-gray relative">
          <h1 className="text-red hover:underline hover:underline-blue font-semibold flex items-center">
            <BiErrorCircle
              className="mr-1 text-light-gray fill-red"
              size={16}
            />
            {title}
          </h1>
          <div className="pb-2"></div>
          <div className="w-full border border-gray rounded-lg" />
          <p className="pt-2">
            {message}{" "}
            <span>
              Please contact the support team at{" "}
              <a
                href={`mailto:support@spartanfitness.com?subject=${message}`}
                className="underline text-blue"
              >
                support@spartanfitness.com
              </a>{" "}
              if this was an unexpected error.
            </span>
          </p>
        </div>
      </div>
      <Footer />
    </>
  );
};

export default ErrorPage;
