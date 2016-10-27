using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Services.Data.Context;
using ProiectColectiv.Services.Data.UnitOfWork;
using Xunit;

namespace ProiectColectiv.Tests.Services
{
    public class UsersServiceTests
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
        public async Task Can_Return_User_By_Id()
        {
            var user = new User();

            var dbContextOptions = CreateNewContextOptions();
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var result = await new UnitOfWork(context).UsersService.GetUser(user.Id);

                Assert.NotNull(result);
            }
        }

        [Fact]
        public async Task Can_Return_All_Users()
        {
            var dbContextOptions = CreateNewContextOptions();
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.Users.AddRange(new User(), new User(), new User());
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var result = await new UnitOfWork(context).UsersService.GetUsers();

                Assert.Equal(3, result.Count);
            }
        }
    }
}
