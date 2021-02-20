using Core.Entities.Enums;

namespace API.Dtos
{
    public class ProductToAddDto
    {
        public string Name { get; set; }
        public ProductUnit Unit { get; set; }
    }
}
