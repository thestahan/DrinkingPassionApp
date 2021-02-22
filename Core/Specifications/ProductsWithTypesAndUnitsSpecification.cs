using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndUnitsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndUnitsSpecification()
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductUnit);
        }

        public ProductsWithTypesAndUnitsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductUnit);
        }
    }
}
