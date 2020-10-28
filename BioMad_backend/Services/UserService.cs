using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Models;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Services
{
    public class UserService
    {
        private readonly ApplicationContext _db;
        private readonly PasswordService _passwordService;
        private readonly HttpContext _httpContext;

        public UserService(ApplicationContext db, PasswordService passwordService, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _passwordService = passwordService;
            _httpContext = httpContextAccessor.HttpContext;
        }

        #region [ User and member context implementation ]

        private User _user;
        
        public User User => _user ??= _httpContext.User.Identity.IsAuthenticated ? 
            _db.Users.FirstOrDefault(u => u.Id == int.Parse(_httpContext.User.Identity.Name)) : null;

        public int? UserId => _httpContext.User.Identity.Name != null ? int.Parse(_httpContext.User.Identity.Name) : default;

        public int? CurrentMemberId => _httpContext.User.Claims.Any(x => x.Type == CustomClaimTypes.MemberId) ?
            int.Parse(_httpContext.User.Claims.First(x => x.Type == CustomClaimTypes.MemberId).Value) : default;

        #endregion

        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>User if everything went good, null if user with given email already exists</returns>
        /// <exception cref="Exception">If some unexpected thing happened while processing user creation</exception>
        public async Task<User> Create(SignUpModel model)
        {
            if (await _db.Users.AnyAsync(x => x.Email == model.Email))
                return null;

            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var user = new User { Email = model.Email };
                user.Password = _passwordService.HashPassword(user, model.Password);

                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
                
                Console.WriteLine($"User id: {user.Id}");
                
                var member = new Member
                {
                    GenderId = model.GenderId,
                    DateBirthday = model.DateBirthday,
                    Name = model.Name,
                    UserId = user.Id
                };

                await _db.Members.AddAsync(member);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return user;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw new Exception();
            }
        }
    }
}