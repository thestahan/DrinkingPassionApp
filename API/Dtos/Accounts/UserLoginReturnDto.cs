using System.Collections.Generic;

namespace API.Dtos.Accounts
{
    public class UserLoginReturnDto
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string TokenExpiration { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
