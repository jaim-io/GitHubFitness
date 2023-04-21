/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      colors: {
        red: "#fe0039",
        "dark-gray": "#242424",
        "default-border-color": "#30363d",
      },
    },
  },
  plugins: [],
};
