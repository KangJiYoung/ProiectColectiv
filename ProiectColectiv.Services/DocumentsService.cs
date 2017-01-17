using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProiectColectiv.Core.Constants;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.DomainModel.Enums;
using ProiectColectiv.Core.Interfaces;
using ProiectColectiv.Services.Data.Context;

namespace ProiectColectiv.Services
{
    public class DocumentsService : IDocumentsService
    {
        private readonly ApplicationDbContext dbContext;

        public DocumentsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddDocument(string userId, string name, string @abstract, byte[] data, IList<string> tags)
        {
            var now = DateTime.Now;
            var document = new Document
            {
                Name = name,
                DateAdded = now,
                Abstract = @abstract,
                UserId = userId
            };
            var state = new DocumentState
            {
                DocumentStatus = DocumentStatus.Draft,
                Version = DocumentVersions.FIRST_DRAFT,
                StatusDate = now,
                DocumentData = new DocumentDataUpload { Data = data }
            };

            document.DocumentStates.Add(state);

            foreach (var tag in tags)
            {
                var dbTag = await dbContext
                    .Tags
                    .FirstOrDefaultAsync(it => it.Name == tag);

                document.DocumentTags.Add(new DocumentTag { Tag = dbTag ?? new Tag { Name = tag } });
            }

            dbContext.Documents.Add(document);
        }

        public async Task AddDocumentFromTemplate(string userId, int idTemplate, string name, string @abstract, IList<string> tags, IDictionary<int, string> items)
        {
            var now = DateTime.Now;
            var document = new Document
            {
                Name = name,
                IdDocumentTemplate = idTemplate,
                Abstract = @abstract,
                DateAdded = now,
                UserId = userId,
            };
            var state = new DocumentState
            {
                DocumentStatus = DocumentStatus.Draft,
                Version = DocumentVersions.FIRST_DRAFT,
                StatusDate = now,
                DocumentData = new DocumentDataTemplate()
            };

            var documentData = (DocumentDataTemplate)state.DocumentData;
            foreach (var item in items)
                documentData.DocumentDataTemplateItems.Add(new DocumentDataTemplateItem { IdDocumentTemplateItem = item.Key, Value = item.Value, DocumentData = documentData });
            document.DocumentStates.Add(state);

            foreach (var tag in tags)
            {
                var dbTag = await dbContext
                    .Tags
                    .FirstOrDefaultAsync(it => it.Name == tag);

                document.DocumentTags.Add(new DocumentTag { Tag = dbTag ?? new Tag { Name = tag } });
            }

            dbContext.Documents.Add(document);
        }

        public async Task AddDocumentNewVersion(string userId, int idDocument, byte[] data)
        {
            var document = await GetDocumentById(idDocument);
            var lastState = document.DocumentStates.Last();
            var now = DateTime.Now;

            document.LastModified = now;
            document.DocumentStates.Add(new DocumentState
            {
                StatusDate = now,
                IdDocument = idDocument,
                DocumentStatus = lastState.DocumentStatus,
                Version = GetNextVersion(lastState.Version, lastState.DocumentStatus),
                DocumentData = new DocumentDataUpload { Data = data }
            });
        }
        
        public async Task ChangeStatus(int idDocument, DocumentStatus documentStatus)
        {
            var document = await dbContext
                .Documents
                .FirstAsync(it => it.IdDocument == idDocument);
            var lastState = await dbContext.DocumentStates.Where(it => it.IdDocument == idDocument).LastAsync();
            var now = DateTime.Now;

            dbContext.Entry(lastState).State = EntityState.Detached;
            lastState.IdDocumentState = 0;
            lastState.StatusDate = now;
            lastState.DocumentStatus = documentStatus;
            lastState.Version = GetNextVersion(lastState.Version, documentStatus);

            document.LastModified = now;
            document.DocumentStates.Add(lastState);
        }

        public async Task<List<Document>> GetDocumentsForTask(string userId, int? idDocumentTemplate)
        {
            var documents = await dbContext
                .Documents
                .Include(it => it.DocumentStates)
                .Where(it => 
                    it.UserId == userId && 
                    it.IdDocumentTemplate == idDocumentTemplate &&
                    !it.IdDocumentTask.HasValue)
                .ToListAsync();

            return documents
                .Where(it => it.DocumentStates.Last().DocumentStatus == DocumentStatus.Final)
                .ToList();
        }

        public Task<List<Document>> GetDocumentsByUserId(string userId)
        {
            return dbContext
                .Documents
                .Include(it => it.DocumentStates)
                .Include(it => it.DocumentTags).ThenInclude(it => it.Tag)
                .Where(it => it.UserId == userId)
                .ToListAsync();
        }

        public Task<Document> GetDocumentById(int idDocument)
        {
            return dbContext
                .Documents
                .Include(it => it.User)
                .Include(it => it.DocumentStates)
                .Include(it => it.DocumentTags).ThenInclude(it => it.Tag)
                .FirstOrDefaultAsync(it => it.IdDocument == idDocument);
        }

        public async Task DeleteDocumentById(int idDocument)
        {
            var document = await dbContext
                .Documents
                .FirstOrDefaultAsync(it => it.IdDocument == idDocument);

            dbContext.Remove(document);
            await dbContext.SaveChangesAsync();
        }

        public double GetNextVersion(double current, DocumentStatus status)
        {
            switch (status)
            {
                case DocumentStatus.Draft:
                    return current + DocumentVersions.DRAFT_VERSION_INCREMENT;
                case DocumentStatus.Final:
                    return Math.Round(current + DocumentVersions.FINAL_VERSION_INCREMENT, 0);
                case DocumentStatus.FinalRevizuit:
                    return current + DocumentVersions.FINAL_REVIZUIT_VERSION_INCREMENT;
                case DocumentStatus.Blocat:
                    return current;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        public async Task EditDocument(int idDocument, string userId, IDictionary<int, string> items)
        {
            var document = await dbContext
                .Documents
                .Include(it => it.DocumentTask)
                .FirstAsync(it => it.IdDocument == idDocument);
            var lastState = await dbContext.DocumentStates.Where(it => it.IdDocument == idDocument).LastAsync();
            var now = DateTime.Now;

            dbContext.Entry(lastState).State = EntityState.Detached;
            lastState.IdDocumentState = 0;
            lastState.StatusDate = now;
            if ((document.DocumentTask?.UserId ?? userId) != userId)
                lastState.DocumentStatus = DocumentStatus.FinalRevizuit;
            lastState.Version = GetNextVersion(lastState.Version, lastState.DocumentStatus);

            var documentData = new DocumentDataTemplate();
            foreach (var item in items)
                documentData.DocumentDataTemplateItems.Add(new DocumentDataTemplateItem { IdDocumentTemplateItem = item.Key, Value = item.Value, DocumentData = documentData });
            lastState.DocumentData = documentData;

            document.LastModified = now;
            document.DocumentStates.Add(lastState);
        }
    }
}