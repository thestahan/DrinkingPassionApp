import axios from "axios";

export default class ProductService {
  async getPublicProducts() {
    const res = await axios.get(
      `${process.env.VUE_APP_API_URL}/products/public`
    );

    return res.data;
  }

  async getPrivateProducts(token) {
    const res = await axios.get(
      `${process.env.VUE_APP_API_URL}/products/private`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return res.data;
  }

  async getUnits() {
    const res = await axios.get(`${process.env.VUE_APP_API_URL}/productUnits`);
    return res.data;
  }

  async getTypes() {
    const res = await axios.get(`${process.env.VUE_APP_API_URL}/productTypes`);
    return res.data;
  }

  async addProduct(token, data) {
    const res = await axios.post(
      `${process.env.VUE_APP_API_URL}/products`,
      data,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return res.data;
  }

  async updateProduct(token, data) {
    const res = await axios.put(
      `${process.env.VUE_APP_API_URL}/products`,
      data,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return res.data;
  }

  async deleteProduct(token, id) {
    const res = await axios.delete(
      `${process.env.VUE_APP_API_URL}/products/${id}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return res.data;
  }
}
