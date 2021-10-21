import axios from "axios";

export default class CocktailService {
  async getCocktails() {
    const res = await axios.get(
      `${process.env.VUE_APP_API_URL}/cocktails/public`
    );
    return res.data;
  }
  async getCocktail(id) {
    const res = await axios.get(
      `${process.env.VUE_APP_API_URL}/cocktails/${id}`
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
    console.log("got into service " + id);
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
