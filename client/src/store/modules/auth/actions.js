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
        responseData.message || "Failed to authenticate."
      );
      throw error;
    }

    context.commit("setUser", {
      displayName: responseData.displayName,
      token: responseData.token,
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
        responseData.message || "Failed to authenticate."
      );
      throw error;
    }

    context.commit("setUser", {
      displayName: responseData.displayName,
      token: responseData.token,
    });
  },
};
