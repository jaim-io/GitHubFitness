import { NavLink } from "react-router-dom";

type Props = {
  path: string;
  children: string;
  border?: boolean;
  target?: string;
};

const AuthNavItem = ({
  path,
  children,
  border = false,
  target = "_self",
  ...props
}: Props) => {
  return (
    <div className={`${border ? "border border-gray rounded-lg" : ""}`}>
      <NavLink
        to={path}
        target={target}
        className={({ isActive }) =>
          `${isActive ? "text-blue" : ""} inline-block hover:text-[#FFFFFFB3] ${
            border ? "px-2 py-1.5" : ""
          }`
        }
        {...props}
        end
      >
        {children}
      </NavLink>
    </div>
  );
};

export default AuthNavItem;
