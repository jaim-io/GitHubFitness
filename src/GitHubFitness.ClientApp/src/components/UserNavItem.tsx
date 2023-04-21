import { Menu, Transition } from "@headlessui/react";
import { Fragment, useContext, useState } from "react";
import { NavLink } from "react-router-dom";
import AuthContext from "../contexts/AuthProvider";
import { FiLogIn } from "react-icons/fi";
import { BsPencilFill } from "react-icons/bs";
import { HiChevronDown } from "react-icons/hi";
import DefaultProfileSvg from "../assets/default-profile.svg";

const UserNavItem = ({ ...props }) => {
  const { user } = useContext(AuthContext);

  return (
    <Menu as="div" className="relative inline-block text-left" {...props}>
      <Menu.Button
        className={({ open }) =>
          `flex h-full transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-110 duration-300 hover:ring-[#2f81f7] hover:hue-rotate-90 ${
            open ? "-translate-y-1 scale-110 ring-[#2f81f7] hue-rotate-90" : ""
          }`
        }
      >
        <img
          className={`inline-block h-10 w-10 rounded-full ring-1 active:ring-[#2f81f7]`}
          src={user?.profileImage ??  DefaultProfileSvg }
        />
        <div>
          <HiChevronDown className="align-bottom inline-block" />
        </div>
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
          className="absolute right-0 mt-2 w-36 origin-top-right divide-y divide-gray-100 rounded-md bg-white shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none"
        >
          {user && (
            <div className="px-1 py-1">
              <p className="text-dark-gray px-2 py-2 text-xs">
                Signed in as {user.firstName}
              </p>
            </div>
          )}
          <div className="pt-2 pb-2">
            {!user && (
              <>
                <Menu.Item>
                  <NavLink
                    to="/login"
                    className={({ isActive }) =>
                      `${
                        isActive ? "text-[#2f81f7]" : "text-dark-gray"
                      } group flex w-full items-center px-3 py-1 text-xs 
                        hover:text-white hover:bg-[#2f81f7]
                    `
                    }
                    end
                  >
                    <FiLogIn className="mr-2 h-3 w-3" />
                    Sign in
                  </NavLink>
                </Menu.Item>
                <Menu.Item>
                  <NavLink
                    to="/register"
                    className={({ isActive }) =>
                      `${
                        isActive ? "text-[#2f81f7]" : "text-dark-gray"
                      } group flex w-full items-center  px-3 py-1 text-xs 
                          hover:text-white hover:bg-[#2f81f7]
                      `
                    }
                    end
                  >
                    <BsPencilFill className="mr-2 h-3 w-3" />
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
                        isActive ? "text-[#2f81f7]" : "text-dark-gray"
                      } group flex w-full items-center px-3 py-1 text-xs 
                      hover:text-white hover:bg-[#2f81f7]
                  `
                    }
                    end
                  >
                    <FiLogIn className="mr-2 h-3 w-3" />
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
