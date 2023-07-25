import { NavLink } from "react-router-dom";

type Props = {
  path: string;
  children: string;
  target?: string;
};

const MainNavItem = ({ path, children, target = "_self", ...props }: Props) => {
  return (
    <NavLink
      to={path}
      target={target}
      className={({ isActive }) =>
        `${
          isActive ? "text-blue" : ""
        } inline-block hover:text-[#FFFFFFB3] py-2`
      }
      {...props}
      end
    >
      {children}
    </NavLink>
  );
};

export default MainNavItem;
