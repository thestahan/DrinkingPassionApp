namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public ProductUnit ProductUnit { get; set; }
        public int ProductUnitId { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
    }
}