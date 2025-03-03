using Microsoft.AspNetCore.Identity;
using System;

namespace DrinkingPassion.Api.Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public DrinkingPassion.Api.Core.Entities.Enums.BartenderType BartenderType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}