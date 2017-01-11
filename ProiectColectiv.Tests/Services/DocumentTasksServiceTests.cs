using System;
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
    public class DocumentTasksServiceTests
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

        [Theory]
        [InlineData(DocumentTaskStatus.RequireAction, DocumentTaskStatus.RequireAction, 2)]
        [InlineData(DocumentTaskStatus.RequireModifications, DocumentTaskStatus.RequireModifications, 2)]
        [InlineData(DocumentTaskStatus.Denied, DocumentTaskStatus.Denied, 2)]
        [InlineData(DocumentTaskStatus.Accepted, DocumentTaskStatus.RequireAction, 3)]
        public async Task Can_Change_Status(DocumentTaskStatus taskStatus, DocumentTaskStatus expectedStatus, int count)
        {
            var now = DateTime.Now;
            var dbContextOptions = CreateNewContextOptions();

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.DocumentTasks.Add(new DocumentTask
                {
                    LastModified = now,
                    DocumentTaskStates = new List<DocumentTaskState> { new DocumentTaskState
                    {
                        DocumentTaskStatus = DocumentTaskStatus.RequireAction,
                        DocumentTaskTypePath = new DocumentTaskTypePath { NextPath = new DocumentTaskTypePath() }
                    } }
                });
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentTasksService(context);
                await service.ChangeStatus(1, taskStatus);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var task = await context.DocumentTasks.Include(it => it.DocumentTaskStates).FirstAsync();
                var newState = task.DocumentTaskStates.Last();

                Assert.NotEqual(now, task.LastModified);
                Assert.Equal(count, task.DocumentTaskStates.Count);
                Assert.Equal(expectedStatus, newState.DocumentTaskStatus);
            }
        }

        [Fact]
        public async Task Can_Return_By_Id()
        {
            var dbContextOptions = CreateNewContextOptions();

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.DocumentTasks.Add(new DocumentTask());
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentTasksService(context);
                var task = service.GetById(1);

                Assert.NotNull(task);
            }
        }

        [Fact]
        public async Task Can_Return_Task_When_User_Is_Next_In_Group()
        {
            var user = new User { UserGroup = new UserGroup() };
            var dbContextOptions = CreateNewContextOptions();
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.Users.Add(user);
                context.DocumentTasks.Add(new DocumentTask
                {
                    DocumentTaskStates = new List<DocumentTaskState>
                    {
                        new DocumentTaskState
                        {
                            DocumentTaskTypePath = new DocumentTaskTypePath
                            {
                                UserGroup = user.UserGroup
                            }
                        }
                    }
                });

                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentTasksService(context);
                var tasks = await service.GetByUserId(user.Id);

                Assert.Equal(1, tasks.Count);
            }
        }

        [Fact]
        public async Task Can_Return_User_Created_Task()
        {
            var user = new User();
            var dbContextOptions = CreateNewContextOptions();
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.Users.Add(user);
                context.DocumentTasks.Add(new DocumentTask { User = user });

                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentTasksService(context);
                var tasks = await service.GetByUserId(user.Id);

                Assert.Equal(1, tasks.Count);
            }
        }

        [Fact]
        public async Task Can_Not_Return_Task_When_Status_Is_Require_Modifications()
        {
            var userGroup = new UserGroup();
            var user = new User { UserGroup = userGroup };
            var dbContextOptions = CreateNewContextOptions();
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.Users.Add(user);
                context.DocumentTasks.Add(new DocumentTask
                {
                    DocumentTaskStates = new List<DocumentTaskState> { new DocumentTaskState
                        {
                            DocumentTaskStatus = DocumentTaskStatus.RequireModifications,
                            DocumentTaskTypePath = new DocumentTaskTypePath {UserGroup = userGroup}
                        }
                    }
                });

                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentTasksService(context);
                var tasks = await service.GetByUserId(user.Id);

                Assert.Equal(0, tasks.Count);
            }
        }

        [Fact]
        public async Task Can_Add_Document_Task()
        {
            var dbContextOptions = CreateNewContextOptions();

            var user = new User();
            var taskType = new DocumentTaskType { Paths = new List<DocumentTaskTypePath> { new DocumentTaskTypePath(), new DocumentTaskTypePath() } };
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                context.Users.Add(user);
                context.DocumentTaskTypes.Add(taskType);
                context.Documents.AddRange(new Document(), new Document(), new Document());

                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var service = new DocumentTasksService(context);

                await service.Add("UserId", 1, new List<int> { 1, 2, 3 });
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                var task = await context.DocumentTasks.Include(it => it.Documents).Include(it => it.DocumentTaskStates).FirstAsync();

                Assert.NotEqual(DateTime.MinValue, task.DateAdded);
                Assert.NotEqual(DateTime.MinValue, task.LastModified);
                Assert.Equal("UserId", task.UserId);
                Assert.Equal(1, task.IdDocumentTaskType);
                Assert.Equal(3, task.Documents.Count);

                var state = task.DocumentTaskStates.Last();
                Assert.NotEqual(DateTime.MinValue, state.StateDate);
                Assert.Equal(DocumentTaskStatus.RequireAction, state.DocumentTaskStatus);
                Assert.Equal(1, state.IdDocumentTaskTypePath.Value);
            }
        }
    }
}