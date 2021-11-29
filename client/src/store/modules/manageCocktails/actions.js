import CocktailService from "../../../services/CocktailService";
import ProductService from "../../../services/ProductSerivce";

const cocktailService = new CocktailService();
const productService = new ProductService();

export default {
  async fetchPublicCocktails(context) {
    try {
      const publicCocktailsData = await cocktailService.getCocktails();

      context.commit("setPublicCocktails", {
        publicCocktailsData: publicCocktailsData,
      });
    } catch (err) {
      console.error(err.toJSON());
    }
  },
  async fetchPrivateCocktails(context, payload) {
    try {
      const privateCocktailsData = await cocktailService.getPrivateCocktails(
        payload.token
      );

      context.commit("setPrivateCocktails", {
        privateCocktailsData: privateCocktailsData,
      });
    } catch (err) {
      console.error(err.toJSON());
    }
  },
  async deleteCocktail(context, payload) {
    try {
      await cocktailService.deleteCocktail(payload.id, payload.token);

      if (payload.isPrivate) {
        context.commit("deletePrivateCocktail", { id: payload.id });
      } else {
        context.commit("deletePublicCocktail", { id: payload.id });
      }

      return true;
    } catch (err) {
      console.warn(err.toJSON());

      return false;
    }
  },
  async fetchProducts(context) {
    try {
      const products = await productService.getProducts();

      context.commit("setProducts", {
        products: products,
      });
    } catch (err) {
      console.error(err.toJSON());
    }
  },
};
