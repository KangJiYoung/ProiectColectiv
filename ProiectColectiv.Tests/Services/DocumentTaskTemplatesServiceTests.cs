using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Services;
using ProiectColectiv.Services.Data.Context;
using Xunit;

namespace ProiectColectiv.Tests.Services
{
    public class DocumentTaskTemplatesServiceTests
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
        public async Task Can_Return_All_Templates()
        {
            var dbContextOptions = CreateNewContextOptions();
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.DocumentTaskTemplates.AddRange(new DocumentTaskTemplate(), new DocumentTaskTemplate());
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentTaskTemplatesService(context);
                var result = await service.GetAll();

                Assert.Equal(2, result.Count);
            }
        }
    }
}