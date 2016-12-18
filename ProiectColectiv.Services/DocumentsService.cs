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

        public async Task AddDocument(string userId, string name, byte[] data, IList<string> tags)
        {
            var now = DateTime.Now;
            var document = new Document
            {
                Name = name,
                DateAdded = now,
                UserId = userId
            };
            document.DocumentStates.Add(new DocumentUploadState { DocumentStatus = DocumentStatus.Draft, Version = DocumentVersions.FIRST_DRAFT, StatusDate = now, Data = data });

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
                UserId = userId
            };

            var state = new DocumentTemplateState { DocumentStatus = DocumentStatus.Draft, Version = DocumentVersions.FIRST_DRAFT, StatusDate = now };
            foreach (var item in items)
                state.DocumentTemplateStateItems.Add(new DocumentTemplateStateItem { IdDocumentTemplateItem = item.Key, Value = item.Value });
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
            document.DocumentStates.Add(new DocumentUploadState
            {
                Data = data,
                StatusDate = now,
                IdDocument = idDocument,
                DocumentStatus = lastState.DocumentStatus,
                Version = lastState.Version + DocumentVersions.DRAFT_VERSION_INCREMENT
            });
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
    }
}