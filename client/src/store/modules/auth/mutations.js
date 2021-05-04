export default {
  setUser(state, payload) {
    state.displayName = payload.displayName;
    state.token = payload.token;
    state.didAutoLogout = false;
  },
  setAutoLogout(state) {
    state.didAutoLogout = true;
  },
};
