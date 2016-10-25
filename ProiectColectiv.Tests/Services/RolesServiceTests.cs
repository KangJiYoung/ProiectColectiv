﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProiectColectiv.Services;
using ProiectColectiv.Services.Data.Context;
using Xunit;

namespace ProiectColectiv.Tests.Services
{
    public class RolesServiceTests
    {
        private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase()
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        [Fact]
        public async Task Can_Return_All_Roles()
        {
            var dbContextOptions = CreateNewContextOptions();
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.Roles.AddRange(new IdentityRole(), new IdentityRole(), new IdentityRole());
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var rolesService = new RolesService(context);
                var result = await rolesService.GetRoles();

                Assert.Equal(3, result.Count);
            }
        }
    }
}
