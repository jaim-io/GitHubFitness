import { useRouteError } from "react-router-dom";
import Footer from "../components/Footer";
import MainNavigation from "../components/navigation/MainNavigation";
import { AxiosError } from "axios";

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
      <h1>{title}</h1>
      <p>{message}</p>
      <Footer />
    </>
  );
};

export default ErrorPage;
