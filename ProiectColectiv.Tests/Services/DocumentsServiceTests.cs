using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.DomainModel.Enums;
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
        public async Task Can_Return_Documents_By_User_Who_Are_Not_From_Template()
        {
            var dbContextOptions = CreateNewContextOptions();
            var user = await CreateUserWithDocument(dbContextOptions, 1);

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.Documents.Add(new Document {UserId =  user.Id});
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentsService(context);

                var good = await service.GetDocumentsByUserAndTemplate(user.Id, null);
                Assert.Equal(1, good.Count);
            }
        }

        [Fact]
        public async Task Can_Return_Documents_By_User_And_Template()
        {
            var dbContextOptions = CreateNewContextOptions();
            var user1 = await CreateUserWithDocument(dbContextOptions, 1);
            var user2 = await CreateUserWithDocument(dbContextOptions, 2);

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentsService(context);

                var good = await service.GetDocumentsByUserAndTemplate(user1.Id, 1);
                Assert.Equal(1, good.Count);

                var good2 = await service.GetDocumentsByUserAndTemplate(user2.Id, 2);
                Assert.Equal(1, good2.Count);

                var bad = await service.GetDocumentsByUserAndTemplate(user1.Id, 2);
                Assert.Equal(0, bad.Count);

                var bad2 = await service.GetDocumentsByUserAndTemplate(user2.Id, 1);
                Assert.Equal(0, bad2.Count);
            }
        }

        [Fact]
        public async Task Can_Edit_Document_From_Template_MetaData()
        {
            var dbContextOptions = CreateNewContextOptions();
            var user = await CreateUser(dbContextOptions);

            await CreateTemplate(dbContextOptions, user.Id);

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentsService(context);
                var items = new Dictionary<int, string> { [0] = "Item 3", [1] = "Item 4" };

                await service.EditDocument(1, items);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var states = await context.DocumentStates.ToListAsync();

                Assert.Equal(2, states.Count);

                var oldState = states.First();
                var newState = states.Last();

                Assert.NotEqual(oldState.IdDocumentData, newState.IdDocumentData);
            }
        }

        [Theory]
        [InlineData(0.01, DocumentStatus.Draft, 0.02)]
        [InlineData(0.02, DocumentStatus.Draft, 0.03)]
        [InlineData(0.01, DocumentStatus.Final, 1.00)]
        [InlineData(1.00, DocumentStatus.Draft, 1.01)]
        [InlineData(1.01, DocumentStatus.Draft, 1.02)]
        [InlineData(1.02, DocumentStatus.Final, 2.00)]
        public void Can_Get_New_Version(double initial, DocumentStatus status, double expected)
        {
            var dbContextOptions = CreateNewContextOptions();
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentsService(context);

                var result = service.GetNextVersion(initial, status);
                Assert.Equal(expected, result);
            }
        }

        [Fact]
        public async Task Can_Change_Document_Status_From_Template()
        {
            var dbContextOptions = CreateNewContextOptions();
            var user = await CreateUser(dbContextOptions);

            await CreateTemplate(dbContextOptions, user.Id);

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentsService(context);

                await service.ChangeStatus(1, DocumentStatus.Final);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var document = await context.Documents.Include(it => it.DocumentStates).FirstAsync();

                Assert.Equal(2, document.DocumentStates.Count);

                var states = await context.DocumentStates.ToListAsync();
                var oldState = states.First();
                var newState = states.Last();

                Assert.Equal(1, newState.Version);
                Assert.NotEqual(oldState.StatusDate, newState.StatusDate);
                Assert.Equal(DocumentStatus.Final, newState.DocumentStatus);
                Assert.Equal(oldState.IdDocumentData, newState.IdDocumentData);
            }
        }

        [Fact]
        public async Task Can_Change_Document_Status()
        {
            var dbContextOptions = CreateNewContextOptions();
            await CreateUserWithDocument(dbContextOptions);

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentsService(context);

                await service.ChangeStatus(1, DocumentStatus.Final);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var document = await context.Documents.Include(it => it.DocumentStates).FirstAsync();

                Assert.Equal(2, document.DocumentStates.Count);

                var states = await context.DocumentStates.ToListAsync();
                var oldState = states.First();
                var newState = states.Last();

                Assert.Equal(1, newState.Version);
                Assert.NotEqual(oldState.StatusDate, newState.StatusDate);
                Assert.Equal(DocumentStatus.Final, newState.DocumentStatus);
                Assert.Equal(oldState.IdDocumentData, newState.IdDocumentData);
            }
        }

        [Fact]
        public async Task Can_Add_Document_New_Version()
        {
            var dbContextOptions = CreateNewContextOptions();
            var user = await CreateUserWithDocument(dbContextOptions);

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentsService(context);

                await service.AddDocumentNewVersion(user.Id, 1, new byte[] { 2, 3, 4 });
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var states = await context.DocumentStates.ToListAsync();

                Assert.Equal(2, states.Count);

                var oldState = states.First();
                var newState = states.Last();

                Assert.Equal(0.02, newState.Version);
                Assert.NotEqual(oldState.IdDocumentData, newState.IdDocumentData);
            }
        }

        [Fact]
        public async Task Can_Add_Document_From_Template()
        {
            var dbContextOptions = CreateNewContextOptions();
            var user = await CreateUser(dbContextOptions);
            var template = await CreateTemplate(dbContextOptions, user.Id);

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var document = await context.Documents.Include(it => it.DocumentTags).FirstAsync();

                Assert.Equal(template.IdDocumentTemplate, document.IdDocumentTemplate);
                Assert.Equal(user.Id, document.UserId);
                Assert.Equal(2, document.DocumentTags.Count);
                Assert.Equal("Abstract", document.Abstract);
                Assert.Equal("Document Name", document.Name);
                Assert.Equal(2, await context.DocumentDataTemplateItems.CountAsync());
            }
        }

        [Fact]
        public async Task Can_Add_Document()
        {
            var dbContextOptions = CreateNewContextOptions();
            var user = await CreateUserWithDocument(dbContextOptions);

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var document = await context.Documents.Include(it => it.DocumentTags).Include(it => it.DocumentStates).ThenInclude(it => it.DocumentData).FirstAsync();

                Assert.Equal("Abstract", document.Abstract);
                Assert.Equal("File.doc", document.Name);
                Assert.Equal(user.Id, document.UserId);
                Assert.Equal(2, document.DocumentTags.Count);
                Assert.Equal(1, document.DocumentStates.Count);
                Assert.Equal(new byte[] { 1, 2, 3 }, ((DocumentDataUpload)document.DocumentStates.First().DocumentData).Data);
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

        [Fact]
        public async Task Can_Return_Document_By_Id()
        {
            var dbContextOptions = CreateNewContextOptions();

            var document = new Document();
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.Documents.Add(document);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var result = await new UnitOfWork(context).DocumentsService.GetDocumentById(document.IdDocument);

                Assert.NotNull(result);
            }
        }

        [Fact]
        public async Task Can_Delete_Document_By_Id()
        {
            var dbContextOptions = CreateNewContextOptions();

            var document = new Document();
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.Documents.Add(document);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                await new UnitOfWork(context).DocumentsService.DeleteDocumentById(document.IdDocument);
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var isAnyDocument = await context.Documents.AnyAsync();

                Assert.False(isAnyDocument);
            }
        }

        #region Private

        private async Task<DocumentTemplate> CreateTemplate(DbContextOptions<ApplicationDbContext> dbContextOptions, string userId)
        {
            var template = new DocumentTemplate
            {
                DocumentTemplateItems = new List<DocumentTemplateItem>
                {
                    new DocumentTemplateItem(), new DocumentTemplateItem()
                }
            };

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.DocumentTemplates.Add(template);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentsService(context);

                var items = new Dictionary<int, string> { [0] = "Item 1", [1] = "Item 2" };
                await service.AddDocumentFromTemplate(userId, template.IdDocumentTemplate, "Document Name", "Abstract", new List<string> { "tag1", "tag2" }, items);
                await context.SaveChangesAsync();
            }

            return template;
        }

        private static async Task<User> CreateUserWithDocument(DbContextOptions<ApplicationDbContext> dbContextOptions, int? idDocumentTemplate = null)
        {
            var user = await CreateUser(dbContextOptions);

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var unitOfWork = new UnitOfWork(context);

                if (idDocumentTemplate.HasValue)
                    await unitOfWork.DocumentsService.AddDocumentFromTemplate(user.Id, idDocumentTemplate.Value, "File.doc", "Abstract", new List<string> {"tag1", "tag2"}, new Dictionary<int, string>());
                else
                    await unitOfWork.DocumentsService.AddDocument(user.Id, "File.doc", "Abstract", new byte[] { 1, 2, 3 }, new List<string> { "tag1", "tag2" });

                await unitOfWork.Commit();
            }

            return user;
        }

        private static async Task<User> CreateUser(DbContextOptions<ApplicationDbContext> dbContextOptions)
        {
            var user = new User();

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }

            return user;
        }

        #endregion
    }
}
