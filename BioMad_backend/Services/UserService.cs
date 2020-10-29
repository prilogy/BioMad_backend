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

        public UserService(ApplicationContext db, PasswordService passwordService,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _passwordService = passwordService;
            _httpContext = httpContextAccessor.HttpContext;
        }

        #region [ User and member context implementation ]

        private User _user;

        public User User => _user ??= _httpContext.User.Identity.IsAuthenticated
            ? _db.Users.FirstOrDefault(u => u.Id == int.Parse(_httpContext.User.Identity.Name))
            : null;

        public int UserId => _httpContext.User.Identity.Name != null
            ? int.Parse(_httpContext.User.Identity.Name)
            : default;

        public int CurrentMemberId => _httpContext.User.Claims.Any(x => x.Type == CustomClaimTypes.MemberId)
            ? int.Parse(_httpContext.User.Claims.First(x => x.Type == CustomClaimTypes.MemberId).Value)
            : default;

        #endregion

        #region [ User related implementation ]

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
                
                // TODO: #EMAIL send email confirmation
                
                return user;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw new Exception();
            }
        }

        public async Task<bool> Edit(UserEditModel model)
        {
            var user = User;

            try
            {
                if (model.Email != null)
                    user.Email = model.Email;
                if (model.Password != null)
                    user.Password = Hasher.Hash(model.Password);

                await _db.SaveChangesAsync();
                // TODO: #EMAIL send email confirmation
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region [ Member related implementation ]

        public async Task<bool> CreateMember(MemberModel model)
        {
            var member = new Member
            {
                Name = model.Name,
                DateBirthday = model.DateBirthday,
                GenderId = model.GenderId,
                UserId = UserId
            };

            try
            {
                await _db.Members.AddAsync(member);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveMember(int id)
        {
            var memberToDelete = User.Members.FirstOrDefault(x => x.Id == id);
            if (memberToDelete == null)
                return false;

            try
            {
                _db.Members.Remove(memberToDelete);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EditMember(MemberModel model, int id)
        {
            var member = User.Members.FirstOrDefault(x => x.Id == id);
            if (member == null)
                return false;

            try
            {
                if (model.GenderId != default)
                    member.GenderId = model.GenderId;
                if (model.DateBirthday != default)
                    member.DateBirthday = model.DateBirthday;
                if (model.Name != null)
                    member.Name = model.Name;

                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}