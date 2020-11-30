using System;
using System.Globalization;
using BioMad_backend.Entities;
using BioMad_backend.Helpers;
using BioMad_backend.Infrastructure.LocalizationResources;
using BioMad_backend.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace BioMad_backend.Helpers
{
    public class EmailTemplates
    {
        private readonly IStringLocalizer<EmailResources> _localizer;
        private readonly AppSettings _appSettings;

        public EmailTemplates(IStringLocalizer<EmailResources> localizer, IOptions<AppSettings> options)
        {
            _localizer = localizer;
            _appSettings = options.Value;
        }

        public EmailTemplate PasswordReset(string code, Culture culture)
        {
            ChangeCulture(culture);
            
            return new EmailTemplate
            {
                Subject = _localizer["ResetPasswordSubject"],
                Body = WithCode(_localizer["ResetPasswordBody1"], _localizer["ResetPasswordBody2"], code)
            };
        }
        
        public EmailTemplate EmailConfirmation(string code, Culture culture)
        {
            ChangeCulture(culture);

            return new EmailTemplate
            {
                Subject = _localizer["EmailConfirmationSubject"],
                Body = WithCode(_localizer["EmailConfirmationBody1"], _localizer["EmailConfirmationBody2"], code)
            };
        }

        private void ChangeCulture(Culture culture)
        {
            CultureInfo.CurrentCulture = culture.Info;
            CultureInfo.CurrentUICulture = culture.Info;
        }

        private string WithCode(string beforeCode, string afterCode, string code) =>
            AppendCopyright($"<h3 style=\"font-weight: 300;\">{beforeCode}:</h3> " +
                            $"<h1 style=\"font-weight: 300;\">{code}</h1> " +
                            $"<h3 style=\"font-weight: 300;\">{afterCode}.</h3>");

        private string AppendCopyright(string body)
            => body + $"<hr/> " +
               $"<h3>© {DateTime.UtcNow.Year} {_appSettings.EmailSenderName}</h3>" +
               "<style>* {font-weight: 300 !important}</style>";
    }
}