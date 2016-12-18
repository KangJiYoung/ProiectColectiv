using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces;
using ProiectColectiv.Services.Data.Context;

namespace ProiectColectiv.Services
{
    public class DocumentsStatesService : IDocumentsStatesService
    {
        private readonly ApplicationDbContext dbContext;

        public DocumentsStatesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<DocumentState> GetDocumentStateById(int idDocumentState, bool isFromTemplate)
        {
            var query = dbContext
                .DocumentStates
                .Where(it => it.IdDocumentState == idDocumentState);

            DocumentState documentState;
            if (isFromTemplate)
                documentState = await query
                    .OfType<DocumentTemplateState>()
                    .Include(it => it.Document).ThenInclude(it => it.DocumentTemplate)
                    .Include(it => it.DocumentTemplateStateItems)
                    .ThenInclude(it => it.DocumentTemplateItem)
                    .FirstAsync();
            else
                documentState = await query
                    .OfType<DocumentUploadState>()
                    .Include(it => it.Document)
                    .FirstAsync();

            return documentState;
        }
    }
}