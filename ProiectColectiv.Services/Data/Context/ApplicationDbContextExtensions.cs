using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ProiectColectiv.Core.Constants;
using ProiectColectiv.Core.DomainModel.Entities;

namespace ProiectColectiv.Services.Data.Context
{
    public static class ApplicationDbContextExtensions
    {
        public static void EnsureSeedData(this ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminUser = SeedAdminUser(context, userManager);
            SeedRoles(context, roleManager);
            SeedAdministratorRoleToAdminUser(context, userManager, adminUser);
        }

        private static void SeedRoles(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            var roles = context
                .Roles
                .Select(it => it.Name)
                .ToList();

            foreach (var role in Roles.AllRoles)
            {
                if (roles.Contains(role))
                    continue;

                var result = roleManager.CreateAsync(new IdentityRole {Name = role}).Result;
                if (!result.Succeeded)
                    throw new Exception($"Cannot create role: {role}");
            }
        }

        private static void SeedAdministratorRoleToAdminUser(ApplicationDbContext context, UserManager<User> userManager, User adminUser)
        {
            var adminUserRole = userManager.GetRolesAsync(adminUser).Result;
            if (adminUserRole.Count == 0)
            {
                var adminRole = context
                    .Roles
                    .First(it => it.Name == Roles.ADMINISTRATOR);

                context.UserRoles.Add(new IdentityUserRole<string> {RoleId = adminRole.Id, UserId = adminUser.Id});
                context.SaveChanges();
            }
        }

        private static User SeedAdminUser(ApplicationDbContext context, UserManager<User> userManager)
        {
            var adminUser = context
                .Users
                .FirstOrDefault(it => it.UserName == Administrator.USERNAME);

            if (adminUser == null)
            {
                var user = new User
                {
                    UserName = Administrator.USERNAME,
                    Email = Administrator.EMAIL,
                };
                var result = userManager.CreateAsync(user, Administrator.PASSWORD).Result;
                if (!result.Succeeded)
                    throw new Exception("Cannot seed Administrator");
            }
            return context
                .Users
                .FirstOrDefault(it => it.UserName == Administrator.USERNAME);
        }
    }
}
