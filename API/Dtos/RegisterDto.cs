using Core.Entities.Enums;

namespace API.Dtos
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public BartenderType BartenderType { get; set; }
    }
}
