using Core.Entities.Enums;

namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public ProductUnit Unit { get; set; }
    }
}
