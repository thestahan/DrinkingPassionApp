import axios from "axios";

export default class ProductService {
  async getProducts() {
    const res = await axios.get(`${process.env.VUE_APP_API_URL}/products`);
    return res.data;
  }
}
