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
    state.publicProductsData.push(payload.product);
    state.publicProductsData = state.publicProductsData.sort((a, b) =>
      a.name.localeCompare(b.name)
    );
  },
  addPrivateProduct(state, payload) {
    state.privateProductsData.push(payload.product);
    state.privateProductsData = state.privateProductsData.sort((a, b) =>
      a.name.localeCompare(b.name)
    );
  },
  updatePublicProduct(state, payload) {
    const index = state.publicProductsData.findIndex(
      (p) => p.id == payload.product.id
    );

    state.publicProductsData[index] = payload.product;
  },
  updatePrivateProduct(state, payload) {
    const index = state.privateProductsData.findIndex(
      (p) => p.id == payload.product.id
    );

    state.privateProductsData[index] = payload.product;
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
