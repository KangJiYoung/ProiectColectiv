using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task Can_Return_By_Id()
        {
            var dbContextOptions = CreateNewContextOptions();
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.DocumentTaskTemplates.Add(new DocumentTaskTemplate());
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentTaskTemplatesService(context);

                var good = await service.GetById(1);
                Assert.NotNull(good);

                var bad = await service.GetById(2);
                Assert.Null(bad);
            }
        }

        [Fact]
        public async Task Can_Return_Task_Types()
        {
            var template = new DocumentTaskTemplate { DocumentTaskTypes = new List<DocumentTaskType> { new DocumentTaskType(), new DocumentTaskType() } };

            var dbContextOptions = CreateNewContextOptions();
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.DocumentTaskTemplates.Add(template);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentTaskTemplatesService(context);
                var result = await service.GetAllTaskTypes(1);

                Assert.Equal(2, result.Count);
            }
        }

        [Fact]
        public async Task Can_Add_New_Template()
        {
            var dbContextOptions = CreateNewContextOptions();
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.UserGroups.AddRange(new UserGroup(), new UserGroup(), new UserGroup());
                context.DocumentTemplates.Add(new DocumentTemplate());
                await context.SaveChangesAsync();
            }

            var paths = new Dictionary<Tuple<string, int>, IList<int>>
            {
                [new Tuple<string, int>("Type 1", 2)] = new List<int> { 1, 2, 3 },
                [new Tuple<string, int>("Type 4", 4)] = new List<int> { 3, 2 }
            };

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentTaskTemplatesService(context);
                service.Add("Template", 1, paths);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var templates = await context.DocumentTaskTemplates.Include(it => it.DocumentTaskTypes).ThenInclude(it => it.Paths).ToListAsync();

                Assert.Equal(1, templates.Count);

                var template = templates.First();
                Assert.Equal("Template", template.Name);
                Assert.Equal(1, template.IdDocumentTemplate);
                Assert.Equal(2, template.DocumentTaskTypes.Count);

                foreach (var taskType in template.DocumentTaskTypes)
                {
                    var key = new Tuple<string, int>(taskType.Name, taskType.DaysLimit);
                    Assert.True(paths.ContainsKey(key));
                    Assert.Equal(paths[key].Count, taskType.Paths.Count);

                    for (var i = 0; i < taskType.Paths.Count; i++)
                    {
                        var path = taskType.Paths[i];

                        Assert.Equal(paths[key][taskType.Paths.Count - i - 1], path.IdUserGroup);
                        Assert.Equal(taskType.Paths.Count - i - 1, path.Index);
                    }
                }
            }
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