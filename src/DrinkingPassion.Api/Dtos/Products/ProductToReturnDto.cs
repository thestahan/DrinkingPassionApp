namespace DrinkingPassion.Api.Dtos.Products
{
    public class ProductToReturnDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string ProductUnit { get; set; }
        public required string ProductUnitAbbreviation { get; set; }
        public required int ProductUnitId { get; set; }
        public required string ProductType { get; set; }
        public required int ProductTypeId { get; set; }
    }
}
