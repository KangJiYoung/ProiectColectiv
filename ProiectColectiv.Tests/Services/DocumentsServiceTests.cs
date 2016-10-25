using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Services;
using ProiectColectiv.Services.Data.Context;
using Xunit;

namespace ProiectColectiv.Tests.Services
{
    public class DocumentsServiceTests
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
        public async Task Can_Add_Document()
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
                var documentsService = new DocumentsService(context);
                await documentsService.AddDocument(user.Id, "file", new List<string> { "tag1", "tag2" });
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var document = await context.Documents.Include(it => it.DocumentTags).FirstAsync();

                Assert.Equal("file", document.Name);
                Assert.Equal(user.Id, document.UserId);
                Assert.Equal(2, document.DocumentTags.Count);
            }
        }
    }
}
