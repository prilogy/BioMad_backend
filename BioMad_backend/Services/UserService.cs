using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BioMad_backend.Areas.Api.V1.Models;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace BioMad_backend.Services
{
    public class UserService
    {
        private readonly ApplicationContext _applicationContext;
        private readonly PasswordService _passwordService;

        public UserService(ApplicationContext applicationContext, PasswordService passwordService)
        {
            _applicationContext = applicationContext;
            _passwordService = passwordService;
        }


        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>User if everything went good, null if user with given email already exists</returns>
        /// <exception cref="Exception">If some unexpected thing happened while processing user creation</exception>
        public async Task<User> Create(SignUpModel model)
        {
            if (await _applicationContext.Users.AnyAsync(x => x.Email == model.Email))
                return null;

            await using var transaction = await _applicationContext.Database.BeginTransactionAsync();
            try
            {
                var user = new User { Email = model.Email };
                user.Password = _passwordService.HashPassword(user, model.Password);

                await _applicationContext.Users.AddAsync(user);
                await _applicationContext.SaveChangesAsync();
                
                Console.WriteLine($"User id: {user.Id}");
                
                var member = new Member
                {
                    GenderId = model.GenderId,
                    DateBirthday = model.DateBirthday,
                    Name = model.Name,
                    UserId = user.Id
                };

                await _applicationContext.Members.AddAsync(member);
                await _applicationContext.SaveChangesAsync();
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