import { Menu, Transition } from "@headlessui/react";
import { Fragment, useContext } from "react";
import { AiFillSetting } from "react-icons/ai";
import { FiLogOut } from "react-icons/fi";
import { HiChevronDown } from "react-icons/hi";
import { NavLink } from "react-router-dom";
import DefaultProfileSvg from "../assets/default-profile.svg";
import AuthContext from "../contexts/AuthProvider";

const UserNavItem = ({ ...props }) => {
  const { auth } = useContext(AuthContext);

  return (
    <Menu as="div" className="relative inline-block text-left" {...props}>
      {auth.user && (
        <>
          <Menu.Button
            className={({ open }) =>
              `flex h-full hover:ring-blue ${open ? "ring-blue" : ""}`
            }
          >
            <img
              className={`inline-block h-10 w-10 rounded-full ring-1 active:ring-blue`}
              src={auth.user?.profileImage ?? DefaultProfileSvg}
            />
            <div className="h-full py-1">
              <HiChevronDown className="inline-block align-bottom" />
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
              className="absolute right-0 mt-2 w-44 origin-top-right divide-y divide-gray rounded-md border border-gray bg-semi-black shadow-lg ring-1 ring-semi-black ring-opacity-5 focus:outline-none"
            >
              <p className="px-3 py-2">Signed in as {auth.user!.firstName}</p>
              <div className="py-2">
                <Menu.Item>
                  <NavLink
                    to="/user-settings"
                    className={
                      "flex w-full items-center px-3 py-1 hover:bg-blue"
                    }
                    end
                  >
                    <AiFillSetting className="mr-2" />
                    Settings
                  </NavLink>
                </Menu.Item>
              </div>
              <div className="py-2">
                <Menu.Item>
                  <NavLink
                    to="/"
                    className={
                      "flex w-full items-center px-3 py-1 hover:bg-blue"
                    }
                    end
                  >
                    <FiLogOut className="mr-2" />
                    Sign out
                  </NavLink>
                </Menu.Item>
              </div>
            </Menu.Items>
          </Transition>
        </>
      )}
    </Menu>
  );
};

export default UserNavItem;
