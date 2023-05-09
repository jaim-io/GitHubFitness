/** @type {import("tailwindcss").Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      colors: {
        blue: "#2f81f7",
        "light-gray": "#7D8590",
        "hover-gray": "#8b949e",
        gray: "#30363d",
        "semi-black": "#161b22",
        black: "#0d1117",
        "light-green": "#2EA043",
        "dark-green": "#238636",
      },
    },
  },
  plugins: [
    require("tailwindcss/plugin")(({ addVariant }) => {
      addVariant("search-cancel", "&::-webkit-search-cancel-button");
    }),
  ],
};
