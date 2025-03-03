namespace DrinkingPassion.Api.Core.Specifications.ProductUnits
{
    public class ProductUnitsOrderedByNameSpec : BaseSpecification<DrinkingPassion.Api.Core.Entities.ProductUnit>
    {
        public ProductUnitsOrderedByNameSpec() : base()
        {
            AddOrderBy(x => x.Name);
        }
    }
}
