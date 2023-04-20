import { Menu, Transition } from "@headlessui/react";
import { Fragment, useContext, useState } from "react";
import { NavLink } from "react-router-dom";
import AuthContext from "../contexts/AuthProvider";
import { FiLogIn } from "react-icons/fi";
import { BsPencilFill } from "react-icons/bs";

const UserNavItem = ({ ...props }) => {
  const { user } = useContext(AuthContext);

  return (
    <Menu as="div" className="relative inline-block text-left" {...props}>
      <Menu.Button>
        <img
          className={`inline-block h-10 w-10 rounded-full ring-1 active:ring-red hover:ring-red transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-110 duration-300 hover:hue-rotate-90 ${
            user?.profileImage == null ? "grayscale hover:grayscale-0" : ""
          }`}
          src={user?.profileImage ?? "./default-profile.svg"}
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
        <Menu.Items
          static
          className="absolute right-0 mt-2 w-32 origin-top-right divide-y divide-gray-100 rounded-md bg-white shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none"
        >
          {user && (
            <div className="px-1 py-1">
              <p className="text-gray-900 px-2 py-2 text-sm">
                Signed in as {user.firstName} {user.lastName}
              </p>
            </div>
          )}
          <div className="px-1 py-1 ">
            {!user && (
              <>
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
                    <FiLogIn className="mr-2 h-5 w-5" />
                    Sign in
                  </NavLink>
                </Menu.Item>
                <Menu.Item>
                  <NavLink
                    to="/register"
                    className={({ isActive }) =>
                      `${
                        isActive ? "bg-red text-white" : "text-gray-900"
                      } group flex w-full items-center rounded-md px-2 py-2 text-sm`
                    }
                    end
                  >
                    <BsPencilFill className="mr-2 h-5 w-5" />
                    Sign up
                  </NavLink>
                </Menu.Item>
              </>
            )}
            {user && (
              <>
                <Menu.Item>
                  <NavLink
                    to="/user-settings"
                    className={({ isActive }) =>
                      `${
                        isActive ? "bg-red text-white" : "text-gray-900"
                      } group flex w-full items-center rounded-md px-2 py-2 text-sm`
                    }
                    end
                  >
                    <FiLogIn className="mr-2 h-5 w-5" />
                    Settings
                  </NavLink>
                </Menu.Item>
              </>
            )}
          </div>
        </Menu.Items>
      </Transition>
    </Menu>
  );
};

export default UserNavItem;
