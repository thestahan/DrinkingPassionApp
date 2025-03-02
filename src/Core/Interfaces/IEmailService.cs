using Core.Entities.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationEmailForRegisteredUser(IConfiguration config, AppUser user, string confirmationLink);
        Task SendForgottenPasswordLink(IConfiguration config, AppUser user, string link);
    }
}
