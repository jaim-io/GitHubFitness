import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import { AuthProvider } from "./contexts/AuthProvider";

console.log(
  "Note: Remember to disable React.StrictMode when actually using / testing the application.",
  "React.StrictMode will sometimes call functions twice.",
);
console.log(
  "1: Since ShiftUp, ShiftDown and OnRemove are not pure functions the functionality will break.",
);
console.log(
  "2: Since since the email confirmation is not a pure action the functionality will break.",
);

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <AuthProvider>
      <App />
    </AuthProvider>
  </React.StrictMode>,
);
