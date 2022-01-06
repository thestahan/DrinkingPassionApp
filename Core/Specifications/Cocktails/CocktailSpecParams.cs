using System.Collections.Generic;

namespace Core.Specifications.Cocktails
{
    public class CocktailSpecParams
    {
        private const int _maxPageSize = 20;
        private string _cocktailName;
        private int _pageSize = 3;

        public int PageIndex { get; set; } = 1;
        public int? ProductId { get; set; }
        public string Sort { get; set; }
        public string Ingredients { get; set; }
        public List<int> IngredientsList { get; set; } = new List<int>();
        public int IngredientsExactCount { get; set; }
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > _maxPageSize ? _maxPageSize : value;
        }
        public string CocktailName
        {
            get => _cocktailName;
            set => _cocktailName = value.ToLower();
        }
    }
}
