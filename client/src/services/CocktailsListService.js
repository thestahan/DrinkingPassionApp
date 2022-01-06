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

  async getCocktailsList(id) {
    const res = await axios.get(
      `${process.env.VUE_APP_API_URL}/cocktailsLists/` + id + "/guestview"
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
  async getCocktailFromList(uniqueLink, cocktailId) {
    const res = await axios.get(
      `${process.env.VUE_APP_API_URL}/cocktailsLists/` +
        uniqueLink +
        "/guestview/" +
        cocktailId,
      {
        params: {
          uniqueLink: uniqueLink,
          cocktailId: +cocktailId,
        },
      }
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
