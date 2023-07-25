import { NavLink, useLocation } from "react-router-dom";

type Props = {
  path: string;
  children: string;
  relatedPath?: string;
  target?: string;
};

const MainNavItem = ({
  path,
  relatedPath,
  children,
  target = "_self",
  ...props
}: Props) => {
  const location = useLocation();

  return (
    <NavLink
      to={path}
      target={target}
      className={({ isActive }) =>
        `${
          isActive || (relatedPath && location.pathname.includes(relatedPath))
            ? "text-blue hover:text-opacity-80"
            : "hover:text-[#FFFFFFB3]"
        } inline-block py-2`
      }
      {...props}
    >
      {children}
    </NavLink>
  );
};

export default MainNavItem;
