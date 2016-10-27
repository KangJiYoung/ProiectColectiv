using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Services;
using ProiectColectiv.Services.Data.Context;
using ProiectColectiv.Services.Data.UnitOfWork;
using Xunit;

namespace ProiectColectiv.Tests.Services
{
    public class TagsServiceTests
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
        public async Task Can_Return_All_Document_Tags_By_User_Id()
        {
            var user = new User
            {
                Documents = new List<Document>
                {
                    new Document
                    {
                        DocumentTags = new List<DocumentTag>
                        {
                            new DocumentTag {Tag = new Tag {Name = "tag1"}},
                            new DocumentTag {Tag = new Tag {Name = "tag2"}}
                        }
                    },
                    new Document
                    {
                        DocumentTags = new List<DocumentTag>
                        {
                            new DocumentTag {Tag = new Tag {Name = "tag1"}}
                        }
                    }
                }
            };

            var dbContextOptions = CreateNewContextOptions();
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var result = await new UnitOfWork(context).TagsService.GetTagsByUserId(user.Id);

                Assert.Equal(2, result.Count);
            }
        }
    }
}
