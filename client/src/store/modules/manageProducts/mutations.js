export default {
  setPublicProducts(state, payload) {
    state.publicProductsData = payload.publicProductsData;
  },
  setPrivateProducts(state, payload) {
    state.privateProductsData = payload.privateProductsData;
  },
  setProductUnits(state, payload) {
    state.productUnits = payload.productUnits;
  },
  setProductTypes(state, payload) {
    state.productTypes = payload.productTypes;
  },
  addPublicProduct(state, payload) {
    state.publicProductsData.push(payload.Product);
  },
  addPrivateProduct(state, payload) {
    state.privateProductsData.push(payload.Product);
  },
  deletePublicProduct(state, payload) {
    state.publicProductsData = state.publicProductsData.filter(
      (product) => product.id != payload.id
    );
  },
  deletePrivateProduct(state, payload) {
    state.privateProductsData = state.privateProductsData.filter(
      (product) => product.id != payload.id
    );
  },
};
