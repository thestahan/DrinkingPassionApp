namespace API.Dtos.Ingredients
{
    public class IngredientToReturnDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public double Amount { get; set; }
        public string Name { get; set; }
    }
}
