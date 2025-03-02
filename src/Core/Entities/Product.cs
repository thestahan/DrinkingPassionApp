using Core.Entities.Identity;

namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public AppUser Author { get; set; }
        public string AuthorId { get; set; }
        public ProductUnit ProductUnit { get; set; }
        public int ProductUnitId { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
    }
}