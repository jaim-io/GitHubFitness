import { NavLink } from "react-router-dom";

const ExercisesNavigation = () => {
  return (
    <header >
      <nav>
        <ul >
          <li>
            {/* <NavLink
              to="/exercises"
              className={({ isActive }) =>
                isActive ? classes.active : undefined
              }
              end
            >
              All exercises
            </NavLink>
            <NavLink
              to="/exercises/new"
              className={({ isActive }) =>
                isActive ? classes.active : undefined
              }
              end
            >
              New exercise
            </NavLink> */}
          </li>
        </ul>
      </nav>
    </header>
  );
};

export default ExercisesNavigation;
