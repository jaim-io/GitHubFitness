import { useState } from "react";
import reactLogo from "../assets/react.svg";
import viteLogo from "/vite.svg";
import { useNavigate } from "react-router-dom";

const HomePage = () => {
  const [count, setCount] = useState(0);
  const navigate = useNavigate();

  const navigateHandler = () => navigate("exercises");

  return (
    <div className="App">
      <button onClick={navigateHandler}>exercises</button>
    </div>
  );
};

export default HomePage;
