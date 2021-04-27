export default {
  isAuthenticated: (state) => {
    return !!state.token;
  },
  displayName: (state) => {
    return state.displayName;
  },
  token: (state) => {
    return state.token;
  },
};
