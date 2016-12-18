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
    public class DocumentsStatesServiceTests
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
        public async Task Can_Return_Document_State_By_Id()
        {
            var dbContextOptions = CreateNewContextOptions();
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.DocumentStates.AddRange(
                    new DocumentUploadState { Data = new byte[] { 1, 2, 3 } },
                    new DocumentTemplateState { DocumentTemplateStateItems = new List<DocumentTemplateStateItem> { new DocumentTemplateStateItem(), new DocumentTemplateStateItem() } });

                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentsStatesService(context);

                var documentUploadState = await service.GetDocumentStateById(1, false);
                Assert.NotNull(documentUploadState); Assert.Equal(new byte[] { 1, 2, 3 }, ((DocumentUploadState)documentUploadState).Data);

                var documentTemplateState = await service.GetDocumentStateById(2, true); Assert.NotNull(documentTemplateState);
                Assert.Equal(2, ((DocumentTemplateState)documentTemplateState).DocumentTemplateStateItems.Count);
            }
        }
    }
}