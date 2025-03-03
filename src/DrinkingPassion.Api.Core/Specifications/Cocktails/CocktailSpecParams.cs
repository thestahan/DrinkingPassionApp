using System.Collections.Generic;

namespace DrinkingPassion.Api.Core.Specifications.Cocktails
{
    public class CocktailSpecParams
    {
        private const int _maxPageSize = 20;
        private string _cocktailName;
        private int _pageSize = 6;
        private int _minPageIndex = 1;
        private int _pageIndex = 1;

        public int? ProductId { get; set; }
        public string Sort { get; set; }
        public string Ingredients { get; set; }
        public List<int> IngredientsList { get; set; } = new List<int>();
        public int IngredientsExactCount { get; set; }

        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = value < _minPageIndex ? _minPageIndex : value;
        }

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