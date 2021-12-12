using Core.Entities;

namespace Core.Specifications
{
    public class ProductUnitsOrderedByNameSpec : BaseSpecification<ProductUnit>
    {
        public ProductUnitsOrderedByNameSpec() : base()
        {
            AddOrderBy(x => x.Name);
        }
    }
}
