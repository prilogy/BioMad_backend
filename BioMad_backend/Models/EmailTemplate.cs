using BioMad_backend.Infrastructure.LocalizationResources;
using Microsoft.Extensions.Localization;

namespace BioMad_backend.Models
{
    public class EmailTemplate
    {
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}