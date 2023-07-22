import { LoaderFunctionArgs } from "react-router-dom";
import Coach from "../../types/domain/Coach";
import axios from "axios";

const CoachDetailPage = () => {
  return <></>;
};

export default CoachDetailPage;

export const loader = async ({ params }: LoaderFunctionArgs) => {
  // Will raise an AxiosError if fetching fails

  const response = await axios.get<Coach>(
    `${import.meta.env.VITE_API_BASE}/coaches/${params.coachId}`,
    {
      headers: {
        Accept: "application/json",
        Authorization: `bearer ${localStorage.getItem("token")}`,
      },
    },
  );

  return response.data;
};
