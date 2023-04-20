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
          `${isActive ? "text-red" : undefined} inline-block hover:text-red transition ease-in-out delay-150 hover:-translate-y-1 hover:scale-110 duration-300`
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
