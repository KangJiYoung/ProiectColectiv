using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.DomainModel.Enums;
using ProiectColectiv.Core.Interfaces;
using ProiectColectiv.Services.Data.Context;

namespace ProiectColectiv.Services
{
    public class DocumentTasksService : IDocumentTasksService
    {
        private readonly ApplicationDbContext dbContext;

        public DocumentTasksService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Add(string userId, int idDocumentTaskType, IEnumerable<int> idDocuments)
        {
            var now = DateTime.Now;
            var task = new DocumentTask
            {
                UserId = userId,
                DateAdded = now,
                LastModified = now,
                IdDocumentTaskType = idDocumentTaskType,
                Documents = await dbContext.Documents.Where(it => idDocuments.Contains(it.IdDocument)).ToListAsync()
            };

            var taskType = await dbContext.DocumentTaskTypes.Include(it => it.Paths).FirstAsync(it => it.IdDocumentTaskType == idDocumentTaskType);
            var state = new DocumentTaskState
            {
                StateDate = now,
                DocumentTaskStatus = DocumentTaskStatus.RequireAction,
                IdDocumentTaskTypePath = taskType.Paths.First(it => it.Index == 0).IdDocumentTaskTypePath
            };

            task.DocumentTaskStates.Add(state);
            dbContext.DocumentTasks.Add(task);
        }

        public async Task<List<DocumentTask>> GetByUserId(string userId)
        {
            var idUserGroup = await dbContext
                .Users
                .Where(it => it.Id == userId)
                .Select(it => it.IdUserGroup)
                .FirstAsync();

            return await dbContext
                .DocumentTasks
                .Include(it => it.User)
                .Include(it => it.DocumentTaskStates)
                .Include(it => it.DocumentTaskType).ThenInclude(it => it.DocumentTaskTemplate)
                .Where(it => it.UserId == userId ||
                             it.DocumentTaskStates.Last().IdDocumentTaskTypePath.HasValue &&
                             it.DocumentTaskStates.Last().DocumentTaskTypePath.IdUserGroup == idUserGroup)
                .ToListAsync();
        }
    }
}