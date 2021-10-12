/* eslint-disable prettier/prettier */
let timer;

export default {
  async signIn(context, payload) {
    this.isLoading = true;

    const response = await fetch(`${process.env.VUE_APP_API_URL}/accounts/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        email: payload.email,
        password: payload.password,
      }),
    });

    const responseData = await response.json();

    if (!response.ok) {
      if (responseData.message) {
        const error = new Error(responseData.message);

        throw error;
      }

      const error = new Error(
        'Dane logowania są niepoprawne. Spróbuj ponownie lub kliknij "Zapomniałeś hasła?", by zmienić hasło.'
      );
      throw error;
    }

    const expiresIn = +responseData.tokenExpiration * 1000;
    const expirationDate = new Date().getTime() + expiresIn;

    localStorage.setItem("token", responseData.token);
    localStorage.setItem("displayName", responseData.displayName);
    localStorage.setItem("tokenExpiration", expirationDate);
    localStorage.setItem("roles", responseData.roles);

    timer = setTimeout(function () {
      context.dispatch("autoLogout");
    }, expiresIn);

    context.commit("setUser", {
      displayName: responseData.displayName,
      token: responseData.token,
      roles: responseData.roles,
    });
  },

  async signUp(_, payload) {
    const response = await fetch(`${process.env.VUE_APP_API_URL}/accounts/register`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        email: payload.email,
        password: payload.password,
        displayName: payload.displayName,
        firstName: payload.firstName,
        lastName: payload.lastName,
        bartenderType: payload.bartenderType,
      }),
    });

    const responseData = await response.json();

    if (!response.ok) {
      const error = new Error(responseData.message || "Nie udało się zarejestrować konta z podanymi danymi.");
      throw error;
    }
  },

  tryLogin(context) {
    const token = localStorage.getItem("token");
    const displayName = localStorage.getItem("displayName");
    const tokenExpiration = localStorage.getItem("tokenExpiration");
    const roles = localStorage.getItem("roles");

    const expiresIn = +tokenExpiration - new Date().getTime();

    if (expiresIn < 0) {
      return;
    }

    timer = setTimeout(function () {
      context.dispatch("autoLogout");
    }, expiresIn);

    if (token && displayName && roles) {
      context.commit("setUser", {
        displayName: displayName,
        token: token,
        roles: roles,
      });
    }
  },

  logout(context) {
    localStorage.removeItem("token");
    localStorage.removeItem("displayName");
    localStorage.removeItem("tokenExpiration");
    localStorage.removeItem("roles");

    clearTimeout(timer);

    context.commit("setUser", {
      token: null,
      displayName: null,
    });
  },

  autoLogout(context) {
    context.dispatch("logout");
    context.commit("setAutoLogout");
  },
};
