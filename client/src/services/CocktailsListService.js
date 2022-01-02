import axios from "axios";

export default class CocktailsListService {
  async getCocktailsLists(token) {
    const res = await axios.get(
      `${process.env.VUE_APP_API_URL}/cocktailsLists`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return res.data;
  }
}
