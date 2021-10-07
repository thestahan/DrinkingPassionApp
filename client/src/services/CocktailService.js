import axios from "axios";

export default class CocktailService {
  async getCocktails() {
    const res = await axios.get(`${process.env.VUE_APP_API_URL}/cocktails`);
    return res.data;
  }
  async getCocktail(id) {
    const res = await axios.get(
      `${process.env.VUE_APP_API_URL}/cocktails/${id}`
    );
    return res.data;
  }
  async addCocktail(cocktail) {
    const res = await axios.post(
      `${process.env.VUE_APP_API_URL}/cocktails`,
      cocktail
    );
    return res.data;
  }
}
