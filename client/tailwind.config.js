module.exports = {
  purge: [
    "./src/App.vue",
    "./src/components/pages/*.{vue,js,ts,jsx,tsx}",
    "./src/components/pages/**/*.{vue,js,ts,jsx,tsx}",
  ],
  darkMode: false, // or 'media' or 'class'
  theme: {
    extend: {
      container: {
        padding: "2rem",
      },
    },
  },
  variants: {
    extend: {},
  },
  plugins: [require("@tailwindcss/forms")],
};
