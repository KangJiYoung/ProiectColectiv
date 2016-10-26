using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public async Task AddDocument(string userId, IFormFile file, IList<string> tags)
        {
            var now = DateTime.Now;
            var document = new Document
            {
                UserId = userId,
                Name = file.FileName,
                DateAdded = now
            };
            document.DocumentStates.Add(new DocumentState { DocumentStatus = DocumentStatus.Draft, Version = 0.01, StatusDate = now });

            var dataStream = new MemoryStream();
            await file.CopyToAsync(dataStream);
            document.Data = dataStream.ToArray();

            foreach (var tag in tags)
            {
                var dbTag = await dbContext
                    .Tags
                    .FirstOrDefaultAsync(it => it.Name == tag);

                document.DocumentTags.Add(new DocumentTag { Tag = dbTag ?? new Tag { Name = tag } });
            }

            dbContext.Documents.Add(document);
            await dbContext.SaveChangesAsync();
        }

        public Task<List<Document>> GetDocumentsByUserId(string userId)
        {
            return dbContext
                .Documents
                .Where(it => it.UserId == userId)
                .ToListAsync();
        }
    }
}