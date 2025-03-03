namespace DrinkingPassion.Api.Core.Specifications.ProductTypes
{
    public class ProductTypesOrderedByNameSpec : BaseSpecification<DrinkingPassion.Api.Core.Entities.ProductType>
    {
        public ProductTypesOrderedByNameSpec() : base()
        {
            AddOrderBy(x => x.Name);
        }
    }
}
