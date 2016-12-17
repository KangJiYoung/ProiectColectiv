using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.DomainModel.Enums;
using ProiectColectiv.Services;
using ProiectColectiv.Services.Data.Context;
using Xunit;

namespace ProiectColectiv.Tests.Services
{
    public class DocumentsTemplateItemServiceTests
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
        public async Task Can_Get_Item_With_Values_From_Template()
        {
            var template = new DocumentTemplate
            {
                DocumentTemplateItems = new List<DocumentTemplateItem>
                {
                    new DocumentTemplateItem(),
                    new DocumentTemplateItem(),
                    new DocumentTemplateItem
                    {
                        DocumentTemplateItemValues = new List<DocumentTemplateItemValue> 
                        {
                            new DocumentTemplateItemValue(), new DocumentTemplateItemValue()
                        }
                    }
                }
            };

            var dbContextOptions = CreateNewContextOptions();
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.DocumentTemplates.Add(template);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentsTemplateItemService(context);
                var result = await service.GetItemsFromTemplate(template.IdDocumentTemplate);

                Assert.Equal(3, result.Count);
                Assert.Equal(2, result.Last().DocumentTemplateItemValues.Count);
            }
        }
    }
}