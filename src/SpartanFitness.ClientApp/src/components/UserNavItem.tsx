import { Menu, Transition } from "@headlessui/react";
import { Fragment, useContext } from "react";
import { NavLink } from "react-router-dom";
import AuthContext from "../contexts/AuthProvider";

const UserNavItem = ({ ...props }) => {
  const { user } = useContext(AuthContext);

  const getPartOfDay = () => {
    const now = new Date().getHours();

    return now >= 5 && now < 12
      ? "morning"
      : now >= 12 && now < 17
      ? "afternoon"
      : "evening";
  };

  return (
    <Menu as="div" className="relative inline-block text-left" {...props}>
      <Menu.Button>
        <img
          className={
            "inline-block h-10 w-10 rounded-full ring-1 active:ring-red hover:ring-red"
          }
          src={
            user?.profileImage ??
            "./default-profile.svg"
          }
        />
      </Menu.Button>
      <Transition
        as={Fragment}
        enter="transition ease-out duration-100"
        enterFrom="transform opacity-0 scale-95"
        enterTo="transform opacity-100 scale-100"
        leave="transition ease-in duration-75"
        leaveFrom="transform opacity-100 scale-100"
        leaveTo="transform opacity-0 scale-95"
      >
        <Menu.Items className="absolute right-0 mt-2 w-52 origin-top-right divide-y divide-gray-100 rounded-md bg-white shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none">
          {user && (
            <div className="px-1 py-1">
              <p className="text-gray-900 px-2 py-2 text-sm">
                Good {getPartOfDay()} {user.firstName}
              </p>
            </div>
          )}
          <div className="px-1 py-1 ">
            {!user && (
              <Menu.Item>
                <NavLink
                  to="/login"
                  className={({ isActive }) =>
                    `${
                      isActive ? "bg-red text-white" : "text-gray-900"
                    } group flex w-full items-center rounded-md px-2 py-2 text-sm`
                  }
                  end
                >
                  Login
                </NavLink>
              </Menu.Item>
            )}
            <Menu.Item>
              {({ active }) => (
                <NavLink
                  to="/1"
                  className={({ isActive }) =>
                    `${
                      isActive ? "bg-red text-white" : "text-gray-900"
                    } group flex w-full items-center rounded-md px-2 py-2 text-sm`
                  }
                  end
                >
                  Duplicate
                </NavLink>
              )}
            </Menu.Item>
          </div>
          <div className="px-1 py-1">
            <Menu.Item>
              {({ active }) => (
                <NavLink
                  to="/2"
                  className={({ isActive }) =>
                    `${
                      isActive ? "bg-red text-white" : "text-gray-900"
                    } group flex w-full items-center rounded-md px-2 py-2 text-sm`
                  }
                  end
                >
                  Archive
                </NavLink>
              )}
            </Menu.Item>
            <Menu.Item>
              {({ active }) => (
                <NavLink
                  to="/3"
                  className={({ isActive }) =>
                    `${
                      isActive ? "bg-red text-white" : "text-gray-900"
                    } group flex w-full items-center rounded-md px-2 py-2 text-sm`
                  }
                  end
                >
                  Move
                </NavLink>
              )}
            </Menu.Item>
          </div>
          <div className="px-1 py-1">
            <Menu.Item>
              {({ active }) => (
                <NavLink
                  to="/4"
                  className={({ isActive }) =>
                    `${
                      isActive ? "bg-red text-white" : "text-gray-900"
                    } group flex w-full items-center rounded-md px-2 py-2 text-sm`
                  }
                  end
                >
                  Delete
                </NavLink>
              )}
            </Menu.Item>
          </div>
        </Menu.Items>
      </Transition>
    </Menu>
  );
};

export default UserNavItem;
