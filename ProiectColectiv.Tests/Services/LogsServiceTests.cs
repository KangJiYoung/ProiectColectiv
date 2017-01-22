using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Services;
using ProiectColectiv.Services.Data.Context;
using Xunit;

namespace ProiectColectiv.Tests.Services
{
    public class LogsServiceTests
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
        public async Task Can_Return_Filtered()
        {
            var dbContextOptions = CreateNewContextOptions();

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.Logs.AddRange(
                    new Logs { Date = DateTime.Today },
                    new Logs { UserId = "UserId" },
                    new Logs());

                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new LogsService(context);

                var result1 = await service.GetFiltered(null, null);
                Assert.Equal(3, result1.Count);

                var result2 = await service.GetFiltered(null, DateTime.Today);
                Assert.Equal(1, result2.Count);

                var result3 = await service.GetFiltered("UserId", null);
                Assert.Equal(1, result3.Count);

                var result4 = await service.GetFiltered("UserId", DateTime.Today);
                Assert.Equal(0, result4.Count);
            }
        }

        [Fact]
        public async Task Can_Return_Logs()
        {
            var dbContextOptions = CreateNewContextOptions();

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.Logs.AddRange(new Logs(), new Logs());

                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new LogsService(context);
                var result = await service.GetAll();

                Assert.Equal(2, result.Count);
            }
        }

        [Fact]
        public async Task Can_Add_Log()
        {
            var now = DateTime.Now;
            var dbContextOptions = CreateNewContextOptions();

            var user = new User();
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.Users.Add(user);

                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new LogsService(context);
                service.Add(user.Id, "Message");

                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var log = await context.Logs.FirstAsync();

                Assert.True(now < log.Date);
                Assert.Equal(log.UserId, user.Id);
                Assert.Equal(log.Message, "Message");
            }
        }
    }
}