namespace API.Dtos.Cocktails
{
    public class CocktailToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string BaseIngredient { get; set; }
        public int IngredientsCount { get; set; }
    }
}
