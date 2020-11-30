using System.Threading.Tasks;
using BioMad_backend.Data;
using BioMad_backend.Helpers;
using BioMad_backend.Infrastructure.LocalizationResources;
using BioMad_backend.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BioMad_backend.Services
{
    public class EmailService
    {
        private readonly AppSettings _appSettings;
        private readonly ApplicationContext _db;

        public readonly EmailTemplates Templates;

        public EmailService(IOptions<AppSettings> appSettings, ApplicationContext db, IStringLocalizer<EmailResources> emailLocalizer)
        {
            _appSettings = appSettings.Value;
            _db = db;
            Templates = new EmailTemplates(emailLocalizer, appSettings);
        }
        
        public async Task SendEmailAsync(string email, EmailTemplate emailTemplate)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_appSettings.EmailSenderName, _appSettings.EmailSenderLogin));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = emailTemplate.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailTemplate.Body
            };
      
            await Task.Run(async () =>
            {
                using var client = new SmtpClient();
        
                await client.ConnectAsync(_appSettings.EmailSenderSmtp, 25, false);
                await client.AuthenticateAsync(_appSettings.EmailSenderLogin, _appSettings.EmailSenderPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            });
        }
    }
}