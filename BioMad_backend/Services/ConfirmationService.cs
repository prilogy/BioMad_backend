using System;
using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Services
{
    public class ConfirmationService
    {
        private ApplicationContext _applicationContext;

        public ConfirmationService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            Send = new ConfirmationSender(_applicationContext);
        }

        public ConfirmationSender Send;

        /// <summary>
        /// Returns ConfirmationCode that is valid and not confirmed yet
        /// </summary>
        public ConfirmationCode Find(User user, string code, ConfirmationCode.Types type)
            => user.ConfirmationCodes.FirstOrDefault(x => ConfirmationCode.CanBeUsed(x, code, type));

        public async Task<ConfirmationCode> Find(string code, ConfirmationCode.Types type)
            => await _applicationContext.ConfirmationCodes.FirstOrDefaultAsync(ConfirmationCode.CanBeUsed(code, type));

        public async Task<bool> Confirm(ConfirmationCode code)
        {
            if (code == null)
                return false;

            try
            {
                code.IsConfirmed = true;
                await _applicationContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        #region [ ConfirmationSender implementation ]

        public class ConfirmationSender
        {
            private readonly ApplicationContext _applicationContext;

            public ConfirmationSender(ApplicationContext applicationContext)
            {
                _applicationContext = applicationContext;
            }

            public async Task<bool> Email(User user)
            {
                if (user.EmailIsVerified)
                    return false;

                var code = new ConfirmationCode
                {
                    UserId = user.Id,
                    Type = ConfirmationCode.Types.EmailConfirmation,
                    HelperValue = user.Email
                };

                // TODO: email
                return await AddInternal(code);
            }

            public async Task<bool> PasswordReset(User user)
            {
                var code = new ConfirmationCode
                {
                    UserId = user.Id,
                    Type = ConfirmationCode.Types.PasswordReset
                };

                // TODO: email
                return await AddInternal(code);
            }

            private async Task<bool> AddInternal(ConfirmationCode code)
            {
                try
                {
                    while (await _applicationContext.ConfirmationCodes.AnyAsync(x =>
                        x.Code == code.Code && !x.IsConfirmed && DateTime.UtcNow < x.DateValidUntil))
                        code.Code = ConfirmationCode.GenerateCode();

                    await _applicationContext.ConfirmationCodes.AddAsync(code);
                    await _applicationContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        #endregion
    }
}