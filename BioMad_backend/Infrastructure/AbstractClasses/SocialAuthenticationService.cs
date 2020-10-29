using System.Threading.Tasks;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Infrastructure.AbstractClasses
{
    public abstract class SocialAuthenticationService
    {
        public abstract SocialAccountProvider Provider { get; }

        protected readonly ApplicationContext _db;
        
        protected SocialAuthenticationService(Data.ApplicationContext db)
        {
            _db = db;
        }

        public abstract Task<SocialAuthenticationCore.Models.SocialAuthenticationIdentity> GetIdentity(string token);

        public async Task<User> FindUser(string token)
        {
            var identity = await GetIdentity(token);

            if (identity == null)
                return null;

            var socialAccount = await _db.SocialAccounts.FirstOrDefaultAsync(x => x.Key == identity.Id && x.ProviderId == Provider.Id);

            return socialAccount?.User;
        }

        public async Task<SocialAccount> CreateAccount(string token)
        {
            var identity = await GetIdentity(token);
            return await CreateAccount(identity);
        }
    
        public async Task<SocialAccount> CreateAccount(SocialAuthenticationCore.Models.SocialAuthenticationIdentity identity)
        {
            if (identity == null || await _db.SocialAccounts.AnyAsync(x => x.Key == identity.Id))
                return null;

            return new SocialAccount
            {
                Key = identity.Id,
                ProviderId = Provider.Id
            };
        }
    }
}