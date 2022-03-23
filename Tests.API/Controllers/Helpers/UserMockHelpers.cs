using System.Security.Claims;

namespace Tests.API.Controllers.Helpers
{
    public static class UserMockHelpers
    {
        public static ClaimsPrincipal GetMockUser() =>
           new(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.Email, "mockEmail")
           }));
    }
}
