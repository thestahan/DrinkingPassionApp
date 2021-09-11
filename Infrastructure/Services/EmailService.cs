using Core.Entities.Identity;
using Core.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendConfirmationEmailForRegisteredUser(IConfiguration config, AppUser user, string confirmationLink)
        {
            var client = new SmtpClient();
            await client.ConnectAsync(config["MailSenderSettings:Server"], Convert.ToInt32(config["MailSenderSettings:Port"]), MailKit.Security.SecureSocketOptions.Auto);
            await client.AuthenticateAsync(config["MailSenderSettings:User"], config["MailSenderSettings:Password"]);

            var from = new MailboxAddress("DrinkingPassion Administrator", config["MailSenderSettings:User"]);
            var to = new MailboxAddress(GetUserCredentials(user), user.Email);

            var message = new MimeMessage();
            message.From.Add(from);
            message.To.Add(to);
            message.Subject = "Rejestracja w DrinkingPassion";
            message.Body = new BodyBuilder
            {
                TextBody = "Kliknij w link, aby potwierdzić adres email: " + confirmationLink
            }.ToMessageBody();


            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            client.Dispose();
        }

        public async Task SendForgottenPasswordLink(IConfiguration config, AppUser user, string link)
        {
            var client = new SmtpClient();
            await client.ConnectAsync(config["MailSenderSettings:Server"], Convert.ToInt32(config["MailSenderSettings:Port"]), true);
            await client.AuthenticateAsync(config["MailSenderSettings:User"], config["MailSenderSettings:Password"]);

            var from = new MailboxAddress("DrinkingPassion Administrator", config["MailSenderSettings:User"]);
            var to = new MailboxAddress(GetUserCredentials(user), user.Email);

            var message = new MimeMessage();
            message.From.Add(from);
            message.To.Add(to);
            message.Subject = "Zmiana hasła w serwisie DrinkingPassion";
            message.Body = new BodyBuilder
            {
                TextBody = "Kliknij w link, aby zmienić hasło: " + link
            }.ToMessageBody();


            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            client.Dispose();
        }

        private static string GetUserCredentials(AppUser user)
        {
            if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
            {
                return $"{user.FirstName} {user.LastName}";
            }

            return user.DisplayName;
        }
    }
}
