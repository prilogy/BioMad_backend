using System.Linq;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Models;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Services
{
    public class AuthService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly TokenService _tokenService;
        private readonly PasswordService _passwordService;

        public AuthService(ApplicationContext applicationContext, TokenService tokenService,
            PasswordService passwordService)
        {
            _applicationContext = applicationContext;
            _tokenService = tokenService;
            _passwordService = passwordService;
        }

        /// <summary>
        /// Authenticates user with given credentials
        /// </summary>
        public async Task<AuthenticationResult> Authenticate(LogInWithCredentialsModel model)
        {
            var user = await _applicationContext.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
            if (user == null ||
                _passwordService.VerifyHashedPassword(user, user.Password, model.Password) ==
                PasswordVerificationResult.Failed)
                return null;

            var member = user.Members.OrderBy(x => x.DateCreatedAt).FirstOrDefault();
            return await Authenticate(user, member);
        }
        

        /// <summary>
        /// Authenticates user with given userId, memberId and refreshToken
        /// </summary>
        public async Task<AuthenticationResult> Authenticate(RefreshTokenAuthenticationModel model)
        {
            var user = await _applicationContext.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            if (user == null)
                return null;

            var token = _tokenService.FindRefreshToken(user, model.RefreshToken);
            if (token == null || !token.IsValid)
                return null;

            var member = await _applicationContext.Members.FirstOrDefaultAsync(x => x.Id == model.MemberId);
            if (member == null || member.UserId != user.Id)
                return null;

            await _tokenService.RevokeRefreshToken(token);

            return await Authenticate(user, member);
        }
        
        /// <summary>
        /// Authenticates user with given user and member
        /// </summary>
        public async Task<AuthenticationResult> Authenticate(User user, Member member)
        {
            user.CurrentMember = member;
            
            var result = new AuthenticationResult
            {
                User = user,
                AccessToken = _tokenService.GenerateAccessToken(user, member),
                RefreshToken = await _tokenService.CreateRefreshToken(user)
            };

            return result;
        }
    }
}