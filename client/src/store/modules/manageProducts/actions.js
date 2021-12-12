import ProductService from "../../../services/ProductSerivce";

const productService = new ProductService();

export default {
  async fetchPublicProducts(context) {
    try {
      const publicProductsData = await productService.getPublicProducts();

      context.commit("setPublicProducts", {
        publicProductsData: publicProductsData,
      });
    } catch (err) {
      console.error(err.toJSON());
    }
  },

  async fetchPrivateProducts(context, payload) {
    try {
      const privateProductsData = await productService.getPrivateProducts(
        payload.token
      );

      context.commit("setPrivateProducts", {
        privateProductsData: privateProductsData,
      });
    } catch (err) {
      console.error(err.toJSON());
    }
  },

  async fetchProductUnits(context) {
    try {
      const units = await productService.getUnits();

      context.commit("setProductUnits", { productUnits: units });
    } catch (err) {
      console.error(err.toJSON());
    }
  },

  async fetchProductTypes(context) {
    try {
      const types = await productService.getTypes();

      context.commit("setProductTypes", { productTypes: types });
    } catch (err) {
      console.error(err.toJSON());
    }
  },

  async addProduct(context, payload) {
    try {
      const createdProduct = await productService.addProduct(
        payload.token,
        payload.product
      );

      if (!payload.product.isPrivate) {
        context.commit("addPublicProduct", {
          product: createdProduct,
        });
      } else {
        context.commit("addPrivateProduct", {
          product: createdProduct,
        });
      }
    } catch (err) {
      console.warn(err.toJSON());

      return false;
    }
  },

  async updateProduct(context, payload) {
    try {
      await productService.updateProduct(payload.token, payload.product);

      if (!payload.product.isPrivate) {
        context.commit("updatePublicProduct", {
          product: payload.product,
        });
      } else {
        context.commit("updatePrivateProduct", {
          product: payload.product,
        });
      }
    } catch (err) {
      console.warn(err);

      return false;
    }
  },

  async isPartOfCocktail(_, payload) {
    try {
      const response = await productService.isPartOfCocktail(
        payload.id,
        payload.isPrivate,
        payload.token
      );

      return response;
    } catch (err) {
      const error = err.toJSON();

      console.log(error);
    }
  },

  async deleteProduct(context, payload) {
    try {
      await productService.deleteProduct(payload.id, payload.token);

      if (payload.isPrivate) {
        context.commit("deletePrivateProduct", { id: payload.id });
      } else {
        context.commit("deletePublicProduct", { id: payload.id });
      }

      return { success: true };
    } catch (err) {
      const error = err.toJSON();

      return { success: false, message: error.message };
    }
  },
};
