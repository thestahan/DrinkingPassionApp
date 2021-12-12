using Core.Entities;

namespace Core.Specifications
{
    public class ProductTypesOrderedByNameSpec : BaseSpecification<ProductType>
    {
        public ProductTypesOrderedByNameSpec() : base()
        {
            AddOrderBy(x => x.Name);
        }
    }
}
