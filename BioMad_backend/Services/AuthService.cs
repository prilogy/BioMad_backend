using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Models;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Infrastructure.Constants;
using BioMad_backend.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Services
{
    public class AuthService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly TokenService _tokenService;
        private readonly PasswordService _passwordService;
        private readonly HttpContext _httpContext;

        public AuthService(ApplicationContext applicationContext, TokenService tokenService,
            PasswordService passwordService, IHttpContextAccessor httpContextAccessor)
        {
            _applicationContext = applicationContext;
            _tokenService = tokenService;
            _passwordService = passwordService;
            _httpContext = httpContextAccessor.HttpContext;
        }

        #region [ Authentication flows implementation ]

        /// <summary>
        /// Authenticates user with given credentials
        /// </summary>
        public async Task<AuthenticationResult> Authenticate(LogInWithCredentialsModel model)
        {
            var user = await VerifyUser(model.Email, model.Password);
            if (user == null) return null;
            return await Authenticate(user);
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

            var member = user.Members.FirstOrDefault(x => x.Id == model.MemberId)
                         ?? user.Members.OrderBy(x => x.DateCreatedAt).FirstOrDefault();
            if (member == null || member.UserId != user.Id)
                return null;

            await _tokenService.RevokeRefreshToken(token);

            return await Authenticate(user, member);
        }

        /// <summary>
        /// Authenticates user with User data and no MemberId
        /// Takes earliest member of User
        /// </summary>
        public async Task<AuthenticationResult> Authenticate(User user)
        {
            var member = user.Members.OrderBy(x => x.DateCreatedAt).FirstOrDefault();
            return await Authenticate(user, member);
        }

        /// <summary>
        /// Authenticates user with given user and member
        /// </summary>
        public async Task<AuthenticationResult> Authenticate(User user, Member member)
        {
            var metaHeaders = GetMetaHeaders();

            try
            {
                if (user.CultureId != default)
                {
                    var culture = Culture.All.FirstOrDefault(x => x.Key == metaHeaders.Culture);
                    if (culture != null)
                        user.CultureId = culture.Id;
                    else metaHeaders.Culture = user.Culture.Key;
                }
                else
                {
                    var culture = Culture.All.FirstOrDefault(x => x.Key == metaHeaders.Culture);
                    if (culture != null)
                        user.CultureId = culture.Id;
                    else
                    {
                        culture = Culture.Fallback;
                        user.CultureId = culture.Id;
                        metaHeaders.Culture = culture.Key;
                    }
                }

                await _applicationContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                var culture = Culture.Fallback;
                user.CultureId = culture.Id;
                metaHeaders.Culture = culture.Key;
                await _applicationContext.SaveChangesAsync();
            }


            user.CurrentMemberId = member.Id;

            var result = new AuthenticationResult
            {
                User = user,
                AccessToken = _tokenService.GenerateAccessToken(user, member, metaHeaders),
                RefreshToken = await _tokenService.CreateRefreshToken(user)
            };

            return result;
        }

        #endregion

        #region [ Cookie authentication flow implementation]

        public async Task<User> AuthenticateCookies(string email, string password)
        {
            var user = await VerifyUser(email, password);
            if (user == null || user.Role.Id != Role.Admin.Id) return null;
            
            var claimsIdentity = _tokenService.GenerateClaimsIdentity(user);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.Now.AddDays(7),
            };

            await _httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return user;
        }

        public async Task LogOutCookies()
        =>  await _httpContext.SignOutAsync("Cookies");

        #endregion

        #region [ Heplper functionality]

        private MetaHeaders GetMetaHeaders() => new MetaHeaders
        {
            Culture = _httpContext.Request.Headers.ContainsKey(HeaderKeys.Culture)
                ? _httpContext.Request.Headers[HeaderKeys.Culture]
                : default
        };
        
        private async Task<User> VerifyUser(string email, string password)
        {
            var user = await _applicationContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null ||
                _passwordService.VerifyHashedPassword(user, user.Password, password) ==
                PasswordVerificationResult.Failed)
                return null;
            
            return user;
        }

        #endregion
    }
}