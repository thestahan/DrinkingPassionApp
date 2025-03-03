namespace DrinkingPassion.Api.Core.Specifications.Products
{
    public class ProductsWithTypesAndUnitsSpecification : BaseSpecification<DrinkingPassion.Api.Core.Entities.Product>
    {
        public ProductsWithTypesAndUnitsSpecification(bool isPrivate, string authorId = "")
            : base(x =>
                (string.IsNullOrEmpty(authorId) || x.AuthorId == authorId) &&
                x.IsPrivate == isPrivate)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductUnit);
            AddOrderBy(x => x.Name);
        }

        public ProductsWithTypesAndUnitsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductUnit);
            AddOrderBy(x => x.Name);
        }
    }
}
