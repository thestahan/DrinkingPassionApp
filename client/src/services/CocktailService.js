import axios from "axios";

export default class CocktailService {
  async getCocktails(queryParams) {
    const res = await axios.get(
      `${process.env.VUE_APP_API_URL}/cocktails/public`,
      {
        params: queryParams,
      }
    );
    return res.data;
  }

  async getPrivateCocktails(token, queryParams) {
    const res = await axios.get(
      `${process.env.VUE_APP_API_URL}/cocktails/private`,
      {
        params: queryParams,
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return res.data;
  }

  async getAllCocktailsAvailableForUser(token) {
    const res = await axios.get(
      `${process.env.VUE_APP_API_URL}/cocktails/availableCocktailsForUser`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return res.data;
  }

  async getCocktail(id, token) {
    const res = await axios.get(
      `${process.env.VUE_APP_API_URL}/cocktails/${id}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return res.data;
  }

  async manageCocktail(formData, token) {
    const res = await axios.post(
      `${process.env.VUE_APP_API_URL}/cocktails`,
      formData,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          ContentType: "multipart/form-data",
        },
      }
    );
    return res;
  }

  async deleteCocktail(id, token) {
    const res = await axios.delete(
      `${process.env.VUE_APP_API_URL}/cocktails/${id}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return res;
  }
}
