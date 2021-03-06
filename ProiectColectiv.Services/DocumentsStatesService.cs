﻿using System.Collections.Generic;
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
            var state = await dbContext
                .DocumentStates
                .Include(it => it.DocumentData)
                .Include(it => it.Document).ThenInclude(it => it.DocumentTemplate)
                .Where(it => it.IdDocumentState == idDocumentState)
                .FirstAsync();

            if (isFromTemplate)
            {
                ((DocumentDataTemplate) state.DocumentData).DocumentDataTemplateItems = await dbContext
                    .DocumentDataTemplateItems
                    .Include(it => it.DocumentTemplateItem)
                    .Where(it => it.IdDocumentData == state.IdDocumentData)
                    .ToListAsync();
            }

            return state;
        }

        public async Task<List<DocumentDataTemplateItem>> GetDocumentDataTemplateItems(int idDocument)
        {
            var idDocumentData = await dbContext
                .DocumentStates
                .Where(it => it.IdDocument == idDocument)
                .Select(it => it.IdDocumentData)
                .LastAsync();

            return await dbContext
                    .DocumentDataTemplateItems
                    .Include(it => it.DocumentTemplateItem).ThenInclude(it => it.DocumentTemplateItemValues)
                    .Where(it => it.IdDocumentData == idDocumentData)
                    .ToListAsync();
        }
    }
}