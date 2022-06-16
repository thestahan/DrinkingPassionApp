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

  async getCocktailsList(userName, slug) {
    const res = await axios.get(
      `${process.env.VUE_APP_API_URL}/cocktailsLists/${userName}/${slug}`
    );

    return res.data;
  }

  async getCocktailsListDetails(id, token) {
    const res = await axios.get(
      `${process.env.VUE_APP_API_URL}/cocktailsLists/` + id,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return res.data;
  }
  async getCocktailFromList(userName, slug, cocktailId) {
    const res = await axios.get(
      `${process.env.VUE_APP_API_URL}/cocktailsLists/${userName}/${slug}}/${cocktailId}`
    );

    return res.data;
  }

  async manageCocktailsList(list, token) {
    const res = await axios.post(
      `${process.env.VUE_APP_API_URL}/cocktailsLists/`,
      list,
      {
        headers: { Authorization: `Bearer ${token}` },
      }
    );

    return res;
  }

  async deleteCocktailsList(id, token) {
    const res = await axios.delete(
      `${process.env.VUE_APP_API_URL}/cocktailsLists/` + id,
      {
        headers: { Authorization: `Bearer ${token}` },
      }
    );

    return res;
  }
}
