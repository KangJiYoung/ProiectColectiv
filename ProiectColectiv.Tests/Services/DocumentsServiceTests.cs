using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Services;
using ProiectColectiv.Services.Data.Context;
using ProiectColectiv.Services.Data.UnitOfWork;
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
            var dbContextOptions = CreateNewContextOptions();
            var user = await CreateUserWithDocument(dbContextOptions);

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var document = await context.Documents.Include(it => it.DocumentTags).Include(it => it.DocumentStates).FirstAsync();

                Assert.Equal("File.doc", document.Name);
                Assert.Equal(user.Id, document.UserId);
                Assert.Equal(2, document.DocumentTags.Count);
                Assert.Equal(1, document.DocumentStates.Count);
                Assert.Equal(new byte[] { 1, 2, 3 }, document.DocumentStates.First().Data);
            }
        }

        [Fact]
        public async Task Can_Return_Documents_By_User_Id()
        {
            var dbContextOptions = CreateNewContextOptions();
            var user = await CreateUserWithDocument(dbContextOptions);

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var result = await new UnitOfWork(context).DocumentsService.GetDocumentsByUserId(user.Id);

                Assert.Equal(1, result.Count);
            }
        }

        private static async Task<User> CreateUserWithDocument(DbContextOptions<ApplicationDbContext> dbContextOptions)
        {
            var user = new User();

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var unitOfWork = new UnitOfWork(context);

                await unitOfWork.DocumentsService.AddDocument(user.Id, "File.doc", new byte[] { 1, 2, 3 }, new List<string> { "tag1", "tag2" });
                await unitOfWork.Commit();
            }
            return user;
        }
    }
}
