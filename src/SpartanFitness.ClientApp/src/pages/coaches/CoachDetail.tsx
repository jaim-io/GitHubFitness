import axios from "axios";
import moment from "moment";
import { AiFillEdit, AiOutlineUser } from "react-icons/ai";
import {
  Link,
  LoaderFunctionArgs,
  useLoaderData,
  useNavigate,
} from "react-router-dom";
import useAuth from "../../hooks/useAuth";
import Coach from "../../types/domain/Coach";

const dateToString = (date: Date) =>
  moment(date).format("MMMM Do YYYY, h:mm:ss a");

const CoachDetailPage = () => {
  const coach = useLoaderData() as Coach;
  const { auth } = useAuth();
  const navigate = useNavigate();

  return (
    <>
      <div className={"flex justify-center pt-6 pb-20 h-full"}>
        <div className="mr-6 max-w-[18rem]">
          <img
            src={coach.profileImage}
            alt={`${coach.firstName} ${coach.lastName}'s image`}
            className="rounded-full border border-gray w-[18rem] h-[18rem] flex text-center leading-[9.5rem]"
          />
        </div>
        <div className="relative">
          <div className="border border-gray w-[40rem] h-fit rounded-lg px-6 py-6">
            <h1 className="text-light-gray flex items-center">
              <AiOutlineUser className="mr-1" size={16} />
              Coach<span className="mx-1">/</span>
              <span className="text-blue">
                {coach.firstName} {coach.lastName}
              </span>
            </h1>
            <div className="self-stretch border border-gray mt-4 h-[1px] rounded-lg" />
            <p className="pt-4 whitespace-pre-line">TEMP</p>
          </div>

          <div className="mt-4">
            <div className="absolute left-0 w-[30%] flex items-start">
              <button
                type="button"
                onClick={() => navigate(-1)}
                className="bg-gray hover:border-hover-gray border border-[rgba(240,246,252,0.1)] rounded-lg py-1 px-3 mr-5"
              >
                Back
              </button>

              {auth.user?.roles.find(
                (r) => r.name === "Coach" || r.name == "Administrator",
              ) && (
                <Link
                  className="bg-gray hover:border-hover-gray border border-[rgba(240,246,252,0.1)] rounded-lg px-3 h-[30px] flex items-center"
                  to={"edit"}
                >
                  <AiFillEdit className="mr-1" size={18} />
                </Link>
              )}
            </div>

            <div className="absolute right-0 py-1 px-3 border border-gray rounded-lg flex items-center text-light-gray ml-2 justify-center">
              Last updated:{" "}
              <p className="text-blue ml-1 hover:underline">
                {dateToString(coach.updatedDateTime)}
              </p>
            </div>
          </div>
        </div>
      </div>
    </>
  );
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
