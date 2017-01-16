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

        public async Task<List<DocumentTask>> GetByUserId(string userId, bool final = false)
        {
            var idUserGroup = await dbContext
                .Users
                .Where(it => it.Id == userId)
                .Select(it => it.IdUserGroup)
                .FirstAsync();

            return await dbContext
                .DocumentTasks
                .Include(it => it.User)
                .Include(it => it.DocumentTaskStates).ThenInclude(it => it.DocumentTaskTypePath)
                .Include(it => it.DocumentTaskType).ThenInclude(it => it.DocumentTaskTemplate)
                .Where(it => it.UserId == userId || (final ? IsFinalized(it) : IsInitiated(it, idUserGroup)))
                .ToListAsync();
        }

        private static bool IsFinalized(DocumentTask task)
        {
            return task.DocumentTaskStates.Last().DocumentTaskStatus == DocumentTaskStatus.Accepted ||
                   task.DocumentTaskStates.Last().DocumentTaskStatus == DocumentTaskStatus.Denied;
        }

        private static bool IsInitiated(DocumentTask task, int? idUserGroup)
        {
            return task.DocumentTaskStates.Last().DocumentTaskStatus != DocumentTaskStatus.RequireModifications &&
                   task.DocumentTaskStates.Last().IdDocumentTaskTypePath.HasValue &&
                   task.DocumentTaskStates.Last().DocumentTaskTypePath.IdUserGroup == idUserGroup;
        }

        public Task<DocumentTask> GetById(int id)
        {
            return dbContext
                .DocumentTasks
                .Include(it => it.User)
                .Include(it => it.Documents).ThenInclude(it => it.DocumentStates)
                .Include(it => it.DocumentTaskStates).ThenInclude(it => it.DocumentTaskTypePath)
                .Include(it => it.DocumentTaskType).ThenInclude(it => it.DocumentTaskTemplate)
                .FirstOrDefaultAsync(it => it.IdDocumentTask == id);
        }

        public async Task ChangeStatus(int idDocumentTask, DocumentTaskStatus documentStatus)
        {
            var now = DateTime.Now;
            var task = await dbContext
                .DocumentTasks
                .Include(it => it.DocumentTaskStates).ThenInclude(it => it.DocumentTaskTypePath)
                .FirstAsync(it => it.IdDocumentTask == idDocumentTask);
            task.LastModified = now;

            var lastState = task.DocumentTaskStates.Last();
            var idNextPath = lastState.DocumentTaskTypePath?.IdNextPath;

            var state = new DocumentTaskState
            {
                IdDocumentTask = idDocumentTask,
                StateDate = now,
                DocumentTaskStatus = documentStatus,
                IdDocumentTaskTypePath = idNextPath == null ? null : lastState.IdDocumentTaskTypePath
            };

            dbContext.DocumentTaskStates.Add(state);

            if (documentStatus == DocumentTaskStatus.Accepted && idNextPath.HasValue)
            {
                dbContext.DocumentTaskStates.Add(new DocumentTaskState
                {
                    IdDocumentTask = idDocumentTask,
                    StateDate = now,
                    DocumentTaskStatus = DocumentTaskStatus.RequireAction,
                    IdDocumentTaskTypePath = idNextPath
                });
            }

            if (!idNextPath.HasValue && documentStatus == DocumentTaskStatus.Accepted || documentStatus == DocumentTaskStatus.Denied)
            {
                var documents = await dbContext
                    .Documents
                    .Where(it => it.IdDocumentTask == idDocumentTask)
                    .Select(it => it.IdDocument)
                    .ToListAsync();

                var service = new DocumentsService(dbContext);
                foreach (var idDocument in documents)
                    await service.ChangeStatus(idDocument, DocumentStatus.Blocat);
            }
        }
    }
}