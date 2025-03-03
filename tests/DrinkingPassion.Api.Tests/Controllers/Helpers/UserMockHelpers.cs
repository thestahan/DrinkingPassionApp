using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DrinkingPassion.Api.Tests.Controllers.Helpers
{
    public static class UserMockHelpers
    {
        public static ControllerContext GetControllerContextWithMockedUser() =>
            new()
            {
                HttpContext = new DefaultHttpContext() { User = GetMockUser() }
            };
        
        public static ClaimsPrincipal GetMockUser() =>
           new(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.Email, "mockEmail")
           }));
    }
}
