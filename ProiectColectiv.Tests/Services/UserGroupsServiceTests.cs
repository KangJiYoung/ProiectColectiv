using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProiectColectiv.Core.Constants;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Services;
using ProiectColectiv.Services.Data.Context;
using Xunit;

namespace ProiectColectiv.Tests.Services
{
    public class UserGroupsServiceTests
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
        public async Task Can_Return_All_User_Groups()
        {
            var dbContextOptions = CreateNewContextOptions();

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.UserGroups.AddRange(new UserGroup(), new UserGroup());
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new UserGroupsService(context);
                var result = await service.GetAll();

                Assert.Equal(2, result.Count);
            }
        }

        [Fact]
        public async Task Can_Add_User_Group()
        {
            var dbContextOptions = CreateNewContextOptions();

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new UserGroupsService(context);
                service.Add(UserGroups.STUDENT);

                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var userGroups = await context.UserGroups.ToListAsync();

                Assert.Equal(1, userGroups.Count);
                Assert.Equal(UserGroups.STUDENT, userGroups.First().Name);
            }
        }
    }
}