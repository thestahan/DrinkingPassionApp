using Core.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Core.Entities.Identity
{
    public class AppUser : IdentityUser 
    {
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public BartenderType BartenderType { get; set; }
    }
}
