import CocktailService from "../../../services/CocktailService";
// import ProductService from "../../../services/ProductService";

const cocktailService = new CocktailService();
// const productService = new ProductService();

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
};
