using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using ProiectColectiv.Core.Constants;
using ProiectColectiv.Core.DomainModel.Entities;

namespace ProiectColectiv.Services.Data.Context
{
    public static class ApplicationDbContextExtensions
    {
        public static void EnsureSeedData(this ApplicationDbContext context, UserManager<User> userManager)
        {
            var adminUser = context
                .Users
                .FirstOrDefault(it => it.UserName == Administrator.Username);

            if (adminUser == null)
            {
                var user = new User
                {
                    UserName = Administrator.Username,
                    Email = Administrator.Email,
                };
                var result = userManager.CreateAsync(user, Administrator.Password).Result;
                if (!result.Succeeded)
                    throw new Exception("Cannot seed Administrator");
            }
        }
    }
}
