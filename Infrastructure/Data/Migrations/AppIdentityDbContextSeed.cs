using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Migrations
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Adam",
                    Email = "adam@gmail.com",
                    UserName = "adam@gmail.com",
                    BartenderType = Core.Entities.Enums.BartenderType.Professional
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");

                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
