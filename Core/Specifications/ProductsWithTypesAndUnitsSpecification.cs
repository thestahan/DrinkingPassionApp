using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndUnitsSpecification : BaseSpecification<Product>
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
