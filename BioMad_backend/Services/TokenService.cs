using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BioMad_backend.Services
{
    public class TokenService
    {
        private readonly AppSettings _appSettings;
        private readonly ApplicationContext _applicationContext;

        public TokenService(IOptions<AppSettings> appSettings, ApplicationContext applicationContext)
        {
            _appSettings = appSettings.Value;
            _applicationContext = applicationContext;
        }

        public async Task<RefreshToken> CreateRefreshToken(User user)
        {
            var refreshToken = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                UserId = user.Id
            };
            
            await _applicationContext.RefreshTokens.AddAsync(refreshToken);
            return refreshToken;
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        
        public string GenerateAccessToken(User user, Member currentMember = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = GenerateTokenDescriptor(user, currentMember);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private SecurityTokenDescriptor GenerateTokenDescriptor(User user, Member currentMember = null)
        {
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaimsIdentity(user, currentMember),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenDescriptor;
        }

        private ClaimsIdentity GenerateClaimsIdentity(User user, Member currentMember = null)
        {
            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Key),
                // TODO: add culture claim
            }, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            if(currentMember != null)
                claims.AddClaim(new Claim(CustomClaimTypes.MemberId, currentMember.Id.ToString()));
            
            return claims;
        }
    }
}