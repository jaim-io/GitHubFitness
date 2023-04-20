import { Menu, Transition } from "@headlessui/react";
import { Fragment } from "react";
import { NavLink } from "react-router-dom";

const UserNavItem = ({ ...props }) => {
  return (
    <Menu as="div" className="relative inline-block text-left" {...props}>
      <Menu.Button>
        <img
          className={
            "inline-block h-10 w-10 rounded-full ring-1 active:ring-red hover:ring-red"
          }
          src="https://images.unsplash.com/photo-1491528323818-fdd1faba62cc?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80"
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
        <Menu.Items className="absolute right-0 mt-2 w-36 origin-top-right divide-y divide-gray-100 rounded-md bg-white shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none">
          <div className="px-1 py-1 ">
            <Menu.Item>
              {({ active }) => (
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
              )}
            </Menu.Item>
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
