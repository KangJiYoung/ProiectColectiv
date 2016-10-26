using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task AddDocument(string userId, string fileName, IList<string> tags)
        {
            var now = DateTime.Now;
            var document = new Document
            {
                UserId = userId,
                Name = fileName,
                DateAdded = now
            };
            document.DocumentStates.Add(new DocumentState { DocumentStatus = DocumentStatus.Draft, Version = 0.01, StatusDate = now });

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
    }
}