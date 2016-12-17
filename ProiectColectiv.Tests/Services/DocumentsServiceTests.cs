﻿using System.Collections.Generic;
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
        public async Task Can_Add_Document_From_Template()
        {
            var template = new DocumentTemplate
            {
                DocumentTemplateItems = new List<DocumentTemplateItem>
                    {
                        new DocumentTemplateItem(), new DocumentTemplateItem()
                    }
            };

            var dbContextOptions = CreateNewContextOptions();
            var user = await CreateUser(dbContextOptions);

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.DocumentTemplates.Add(template);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentsService(context);

                var items = new Dictionary<int, string> { [0] = "Item 1", [1] = "Item 2" };
                await service.AddDocumentFromTemplate(user.Id, template.IdDocumentTemplate, "Document Name", "Abstract", new List<string> { "tag1", "tag2" }, items);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var document = await context.Documents.Include(it => it.DocumentTags).FirstAsync();

                Assert.Equal(template.IdDocumentTemplate, document.IdDocumentTemplate);
                Assert.Equal(user.Id, document.UserId);
                Assert.Equal(2, document.DocumentTags.Count);
                Assert.Equal("Abstract", document.Abstract);
                Assert.Equal("Document Name", document.Name);
                Assert.Equal(2, await context.DocumentTemplateStateItems.CountAsync());
            }
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
                Assert.Equal(new byte[] { 1, 2, 3 }, ((DocumentUploadState)document.DocumentStates.First()).Data);
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

        private static async Task<User> CreateUserWithDocument(DbContextOptions<ApplicationDbContext> dbContextOptions)
        {
            var user = await CreateUser(dbContextOptions);

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var unitOfWork = new UnitOfWork(context);

                await unitOfWork.DocumentsService.AddDocument(user.Id, "File.doc", new byte[] { 1, 2, 3 }, new List<string> { "tag1", "tag2" });
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
    }
}
