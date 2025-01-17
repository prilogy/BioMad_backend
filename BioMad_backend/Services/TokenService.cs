﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Helpers;
using BioMad_backend.Models;
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

        #region [ Access token implementation ]

        public string GenerateAccessToken(User user, Member currentMember, MetaHeaders metaHeaders)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = GenerateTokenDescriptor(user, currentMember, metaHeaders);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        #endregion

        #region [ Refresh token implementation ]

        public async Task<RefreshToken> CreateRefreshToken(User user)
        {
            var nonHashedRefreshToken = GenerateRefreshToken(); 
            var refreshToken = new RefreshToken
            {
                Token = Hasher.Hash(nonHashedRefreshToken),
                UserId = user.Id
            };

            await _applicationContext.RefreshTokens.AddAsync(refreshToken);
            await _applicationContext.SaveChangesAsync();

            refreshToken.Token = nonHashedRefreshToken;
            
            return refreshToken;
        }

        public async Task<bool> RevokeRefreshToken(User user, string refreshToken)
        {
            var token = FindRefreshToken(user, refreshToken);
            return await RevokeRefreshToken(token);
        }

        public async Task<bool> RevokeRefreshToken(RefreshToken refreshToken)
        {
            if (refreshToken == null)
                return false;

            _applicationContext.Remove(refreshToken);
            await _applicationContext.SaveChangesAsync();
            return true;
        }

        public RefreshToken FindRefreshToken(User user, string refreshToken)
        {
            var refreshTokens = user.RefreshTokens;
            foreach (var token in refreshTokens)
                if (Hasher.Verify(token.Token, refreshToken))
                    return token;


            return null;
        }

        private static string GenerateRefreshToken()
            => Hasher.RandomToken();

        #endregion

        #region [ Token description implementation ]

        private SecurityTokenDescriptor GenerateTokenDescriptor(User user, Member currentMember, MetaHeaders metaHeaders)
        {
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaimsIdentity(user, currentMember, metaHeaders),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            return tokenDescriptor;
        }

        public ClaimsIdentity GenerateClaimsIdentity(User user, Member currentMember = null, MetaHeaders metaHeaders = null)
        {
            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Key),
            }, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            if(metaHeaders?.Culture != null)
                claims.AddClaim(new Claim(ClaimTypes.Locality, metaHeaders.Culture));
            
            if (currentMember != null)
                claims.AddClaim(new Claim(CustomClaimTypes.MemberId, currentMember.Id.ToString()));

            return claims;
        }

        #endregion
    }
}