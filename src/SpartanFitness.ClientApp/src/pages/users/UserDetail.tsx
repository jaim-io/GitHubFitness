import moment from "moment";
import { AiOutlineUser } from "react-icons/ai";
import { useNavigate } from "react-router-dom";
import LoadingIcon from "../../components/icons/LoadingIcon";
import useAdmin from "../../hooks/useAdmin";
import useAuth from "../../hooks/useAuth";
import useCoach from "../../hooks/useCoach";
import useUserSaves from "../../hooks/useUserSaves";
import Administrator from "../../types/domain/Administrator";
import Coach from "../../types/domain/Coach";
import UserSavesResult from "../../types/results/UserSavesResult";

const dateToString = (date: Date) =>
  moment(date).format("MMMM Do YYYY, h:mm:ss a");

type Role = {
  type: "User" | "Coach" | "Administrator";
  createdDateTime: Date;
};

const UserDetailPage = () => {
  const { auth } = useAuth();
  if (!auth.user) {
    throw new Error("User undefined");
  }

  const navigate = useNavigate();
  const rolesToShow: Role[] = [];
  rolesToShow.push({ type: "User", ...auth.user });

  const coachRole = auth.user.roles.find((r) => r.name === "Coach");
  let [coach, coachLoading]: [Coach | undefined, boolean] = [undefined, false];
  if (coachRole) {
    [coach, , coachLoading] = useCoach(coachRole.id);

    if (coach) {
      rolesToShow.push({ type: "Coach", ...coach });
    }
  }

  const adminRole = auth.user.roles.find((r) => r.name === "Administrator");
  let [admin, adminLoading]: [Administrator | undefined, boolean] = [
    undefined,
    false,
  ];
  if (adminRole) {
    [admin, , adminLoading] = useAdmin(adminRole.id);

    if (admin) {
      rolesToShow.push({ type: "Administrator", ...admin });
    }
  }

  let [userSaves, userSavesLoading]: [UserSavesResult | undefined, boolean] = [
    undefined,
    false,
  ];
  if (adminRole) {
    [userSaves, , userSavesLoading] = useUserSaves(auth.user.id);
  }

  return (
    <>
      <div className={"flex justify-center pt-6 pb-20 h-full"}>
        <div className="mr-6 max-w-[18rem]">
          <img
            src={auth.user.profileImage}
            alt={`${auth.user.firstName} ${auth.user.lastName}'s image`}
            className="rounded-full border border-gray w-[18rem] h-[18rem] flex text-center leading-[9.5rem]"
          />
        </div>

        <div className="relative">
          <div className="border border-gray w-[40rem] h-fit rounded-lg px-6 py-6">
            <h1 className="text-light-gray flex items-center">
              <AiOutlineUser className="mr-1" size={16} />
              User<span className="mx-1">/</span>
              <span className="text-blue">
                {auth.user.firstName} {auth.user.lastName}
              </span>
            </h1>

            <div className="self-stretch border border-gray mt-4 h-[1px] rounded-lg" />

            <div className="pt-4 whitespace-pre-line">
              <div className="grid grid-cols-10 gap-1 ">
                <span className="border border-x-0 border-t-0 border-blue col-span-4 mx-1.5">
                  Role
                </span>
                <span className="border border-x-0 border-t-0 border-blue col-span-6 mx-1.5">
                  Since
                </span>
              </div>

              {coachLoading || adminLoading ? (
                <div className="grid grid-cols-10 py-[0.125rem] gap-1">
                  <div
                    role="status"
                    className="py-5 flex justify-center items-center col-span-4"
                  >
                    <LoadingIcon classNames="mr-2 fill-blue text-gray w-6 h-6" />
                    <span className="sr-only">Loading...</span>
                  </div>

                  <div
                    role="status"
                    className="py-5 flex justify-center items-center col-span-6"
                  >
                    <LoadingIcon classNames="mr-2 fill-blue text-gray w-6 h-6" />
                    <span className="sr-only">Loading...</span>
                  </div>
                </div>
              ) : (
                rolesToShow.length > 0 &&
                rolesToShow.map((role) => (
                  <div
                    className="grid grid-cols-10 py-[0.125rem] gap-1"
                    key={`${role.type}`}
                  >
                    <span className="col-span-4 shadow appearance-none border border-gray rounded-lg w-full py-1.5 px-3 bg-black text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline cursor-default">
                      {role.type}
                    </span>
                    <span className="col-span-6 shadow appearance-none border border-gray rounded-lg w-full py-1.5 px-3 bg-black text-white leading-tight focus:outline focus:outline-blue focus:shadow-outline cursor-default">
                      {dateToString(role.createdDateTime)}
                    </span>
                  </div>
                ))
              )}
            </div>
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
            </div>

            <div className="absolute right-0 py-1 px-3 border border-gray rounded-lg flex items-center text-light-gray ml-2 justify-center">
              Last updated:{" "}
              <span className="text-blue ml-1">
                {dateToString(auth.user.createdDateTime)}
              </span>
            </div>
          </div>
        </div>
      </div>
      <div className="flex justify-center pt-6 pb-20 h-full">
        <div className="border border-gray w-[60rem] h-fit rounded-lg px-6 py-6"></div>
      </div>
    </>
  );
};

export default UserDetailPage;
