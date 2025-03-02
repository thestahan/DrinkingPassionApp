using Core.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System;

namespace Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public BartenderType BartenderType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}