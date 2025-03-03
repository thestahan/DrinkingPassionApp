using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace DrinkingPassion.Api.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationEmailForRegisteredUser(IConfiguration config, DrinkingPassion.Api.Core.Entities.Identity.AppUser user, string confirmationLink);
        Task SendForgottenPasswordLink(IConfiguration config, DrinkingPassion.Api.Core.Entities.Identity.AppUser user, string link);
    }
}
