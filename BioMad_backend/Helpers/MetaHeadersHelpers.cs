using BioMad_backend.Infrastructure.Constants;
using BioMad_backend.Models;
using Microsoft.AspNetCore.Http;

namespace BioMad_backend.Helpers
{
    public static class MetaHeadersHelpers
    {
        public static MetaHeaders Get(HttpContext context) => new MetaHeaders
        {
            Culture = context.Request.Headers.ContainsKey(HeaderKeys.Culture)
                ? context.Request.Headers[HeaderKeys.Culture]
                : default
        };
    }
}