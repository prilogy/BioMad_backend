using System;
using System.Linq;
using System.Security.Cryptography;
using BioMad_backend.Entities;
using BioMad_backend.Helpers;
using Microsoft.AspNetCore.Identity;

namespace BioMad_backend.Services
{
    public sealed class PasswordHasherService : IPasswordHasher<User>
    {
        public string HashPassword(User user, string password)
        {
            return Hasher.Hash(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword,
            string providedPassword)
        {
            return Hasher.Verify(hashedPassword, providedPassword)
                ? PasswordVerificationResult.Success
                : PasswordVerificationResult.Failed;
        }
    }
}