using Core.Entities;

namespace Core.Specifications.ProductUnits
{
    public class ProductUnitsOrderedByNameSpec : BaseSpecification<ProductUnit>
    {
        public ProductUnitsOrderedByNameSpec() : base()
        {
            AddOrderBy(x => x.Name);
        }
    }
}
