using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Models;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using BioMad_backend.Helpers;
using BioMad_backend.Infrastructure.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BioMad_backend.Services
{
    public class UserService
    {
        private readonly ApplicationContext _db;
        private readonly PasswordService _passwordService;
        private readonly HttpContext _httpContext;
        private readonly ConfirmationService _confirmationService;

        public UserService(ApplicationContext db, PasswordService passwordService,
            IHttpContextAccessor httpContextAccessor, ConfirmationService confirmationService)
        {
            _db = db;
            _passwordService = passwordService;
            _httpContext = httpContextAccessor.HttpContext;
            _confirmationService = confirmationService;
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

        private Member _currentMember;
        
        public Member CurrentMember => _currentMember ??=  _httpContext.User.Identity.IsAuthenticated
            ? _db.Members.FirstOrDefault(u => u.Id == CurrentMemberId)
            : null;

        public Culture Culture => Culture.All
                                      .FirstOrDefault(x => x.Key == _httpContext.User.Claims
                                          .FirstOrDefault(y => y.Type == ClaimTypes.Locality)?.Value)
                                  ?? Culture.Fallback;

        #endregion

        #region [ User related implementation ]

        public async Task<User> Create(SignUpModel model)
        {
            if (await _db.Users.AnyAsync(x => x.Email == model.Email))
                return null;

            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var user = new User { Email = model.Email, RoleId = Role.User.Id };
                user.Password = _passwordService.HashPassword(user, model.Password);

                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();

                var member = new Member
                {
                    GenderId = model.GenderId,
                    DateBirthday = model.DateBirthday,
                    Name = model.Name,
                    UserId = user.Id,
                    Color = model.Color
                };

                await _db.Members.AddAsync(member);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                await _confirmationService.Send.EmailConfirmation(user);

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
            model.Email = model.Email.ToLower();

            var userAlreadyHas = await _db.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (userAlreadyHas != null && userAlreadyHas.Id != user.Id)
                return false;
            
            try
            {
                if (model.Email != null && model.Email != user.Email)
                {
                    user.Email = model.Email;
                    await _confirmationService.Send.EmailConfirmation(user);
                }

                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region [ Admin related implementation ]

        public async Task<User> CreateAdmin(string email, string password)
        {
            if (await _db.Users.AnyAsync(x => x.Email == email))
                return null;
            
            try
            {
                var user = new User
                {
                    Email = email, RoleId = Role.Admin.Id
                };
                user.Password = _passwordService.HashPassword(user, password);

                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();

                return user;
            }
            catch (Exception)
            {
                return null;
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
                UserId = UserId,
                Color = model.Color
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
                if (model.Color != null)
                    member.Color = model.Color;

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