import { NavLink } from "react-router-dom";
import classes from "./ExercisesNavigation.module.css";

const ExercisesNavigation = () => {
  return (
    <header className={classes.header}>
      <nav>
        <ul className={classes.list}>
          <li>
            <NavLink
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
            </NavLink>
          </li>
        </ul>
      </nav>
    </header>
  );
};

export default ExercisesNavigation;
