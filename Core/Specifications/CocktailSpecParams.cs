namespace Core.Specifications
{
    public class CocktailSpecParams
    {
        private const int _maxPageSize = 20;
        private string _search;
        private int _pageSize = 3;

        public int PageIndex { get; set; } = 1;
        public int? ProductId { get; set; }
        public string Sort { get; set; }
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
        }
        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}
