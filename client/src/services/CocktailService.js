import axios from "axios";

export default class CocktailService {
  async getCocktails() {
    const res = await axios.get(`${process.env.VUE_APP_API_URL}/cocktails`);
    return res.data;
  }
}
