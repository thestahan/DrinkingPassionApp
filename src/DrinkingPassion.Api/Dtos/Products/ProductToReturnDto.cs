namespace DrinkingPassion.Api.Dtos.Products
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductUnit { get; set; }
        public string ProductUnitAbbreviation { get; set; }
        public int ProductUnitId { get; set; }
        public string ProductType { get; set; }
        public int ProductTypeId { get; set; }
    }
}
