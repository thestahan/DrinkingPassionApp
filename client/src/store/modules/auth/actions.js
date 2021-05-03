export default {
  async signIn(context, payload) {
    this.isLoading = true;
    console.log(this.isLoading);
    const response = await fetch("https://localhost:5001/api/accounts/login", {
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
      const error = new Error(
        'Dane logowania są niepoprawne. Spróbuj ponownie lub kliknij "Zapomniałeś hasła?", by zmienić hasło.'
      );
      throw error;
    }

    localStorage.setItem("token", responseData.token);
    localStorage.setItem("displayName", responseData.displayName);
    localStorage.setItem("tokenExpiration", responseData.tokenExpiration);

    context.commit("setUser", {
      displayName: responseData.displayName,
      token: responseData.token,
      tokenExpiration: responseData.tokenExpiration,
    });
  },

  async signUp(context, payload) {
    const response = await fetch(
      "https://localhost:5001/api/accounts/register",
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email: payload.email,
          password: payload.password,
          displayName: payload.displayName,
        }),
      }
    );

    const responseData = await response.json();

    if (!response.ok) {
      const error = new Error(
        responseData.message ||
          "Nie udało się zarejestrować konta z podanymi danymi."
      );
      throw error;
    }
  },

  tryLogin(context) {
    const token = localStorage.getItem("token");
    const displayName = localStorage.getItem("displayName");

    if (token && displayName) {
      context.commit("setUser", {
        displayName: displayName,
        token: token,
      });
    }
  },

  logout(context) {
    localStorage.removeItem("token");
    localStorage.removeItem("displayName");
    localStorage.removeItem("tokenExpiration");

    context.commit("setUser", {
      token: null,
      displayName: null,
      isAuthenticated: false,
    });
  },
};
