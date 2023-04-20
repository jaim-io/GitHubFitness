import { NavLink } from "react-router-dom";

type Props = {
  path: string;
  children: string;
  target?: string;
};

const MainNavItem = ({ path, children, target = "_self", ...props }: Props) => {
  return (
    <li>
      <NavLink
        to={path}
        target={target}
        className={({ isActive }) =>
          `${isActive ? "text-red" : undefined} hover:text-red`
        }
        {...props}
        end
      >
        {children}
      </NavLink>
    </li>
  );
};

export default MainNavItem;
