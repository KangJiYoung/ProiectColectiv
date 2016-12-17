using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Services;
using ProiectColectiv.Services.Data.Context;
using Xunit;

namespace ProiectColectiv.Tests.Services
{
    public class DocumentsTemplateServiceTests
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
        public async Task Can_Get_All_Templates()
        {
            var dbContextOptions = CreateNewContextOptions();

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.DocumentTemplates.AddRange(new DocumentTemplate(), new DocumentTemplate());
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentsTemplateService(context);
                var result = await service.GetAllTemplates();

                Assert.Equal(2, result.Count);
            }
        }

        [Fact]
        public async Task Can_Get_Template_By_Id()
        {
            var template = new DocumentTemplate();
            var dbContextOptions = CreateNewContextOptions();

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.DocumentTemplates.Add(template);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentsTemplateService(context);
                var result = await service.GetTemplateById(template.IdDocumentTemplate);

                Assert.NotNull(result);
            }
        }
    }
}
