using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Helpers;
using BioMad_backend.Infrastructure.AbstractClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using SocialAuthenticationCore;
using SocialAuthenticationCore.Models;

namespace BioMad_backend.Services
{
    public static class SocialAuthenticationServices
    {
        public abstract class SocialAuthenticationServiceWithIdentity : SocialAuthenticationService
        {
            public override SocialAccountProvider Provider { get; }

            protected readonly SocialAuthenticationCore.Services.SocialAuthenticationService _socialIdentityService;

            public SocialAuthenticationServiceWithIdentity(ApplicationContext db,
                SocialAuthenticationCore.Services.SocialAuthenticationService socialIdentityService) : base(db)
            {
                _socialIdentityService = socialIdentityService;
            }
        }

        #region [ Different providers implementation ]

        public class VkAuthenticationService : SocialAuthenticationServiceWithIdentity
        {
            public override SocialAccountProvider Provider => SocialAccountProvider.Vk;

            public VkAuthenticationService(ApplicationContext db,
                SocialAuthenticationCore.Services.SocialAuthenticationService socialIdentityService)
                : base(db, socialIdentityService)
            {
            }

            public override async Task<SocialAuthenticationIdentity> GetIdentity(string token) =>
                await _socialIdentityService.Vk.GetSocialAuthIdentity(token);
        }

        public class GoogleAuthenticationService : SocialAuthenticationServiceWithIdentity
        {
            public override SocialAccountProvider Provider => SocialAccountProvider.Google;

            public GoogleAuthenticationService(ApplicationContext db,
                SocialAuthenticationCore.Services.SocialAuthenticationService socialIdentityService)
                : base(db, socialIdentityService)
            {
            }

            public override async Task<SocialAuthenticationIdentity> GetIdentity(string token) =>
                await _socialIdentityService.Google.GetSocialAuthIdentity(token);
        }

        public class FacebookAuthenticationService : SocialAuthenticationServiceWithIdentity
        {
            public override SocialAccountProvider Provider => SocialAccountProvider.Vk;

            public FacebookAuthenticationService(ApplicationContext db,
                SocialAuthenticationCore.Services.SocialAuthenticationService socialIdentityService)
                : base(db, socialIdentityService)
            {
            }

            public override async Task<SocialAuthenticationIdentity> GetIdentity(string token) =>
                await _socialIdentityService.Facebook.GetSocialAuthIdentity(token);
        }

        #endregion
    }
}