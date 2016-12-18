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

        public async Task<List<DocumentTemplateStateItem>> GetDocumentTemplateStateitems(int idDocument)
        {
            var items = await dbContext
                .DocumentStates
                .Where(it => it.IdDocument == idDocument)
                .OfType<DocumentTemplateState>()
                .Include(it => it.DocumentTemplateStateItems)
                .Select(it => it.DocumentTemplateStateItems)
                .LastAsync();

            foreach (var item in items)
            {
                item.DocumentTemplateItem = await dbContext
                    .DocumentTemplateItems
                    .FirstAsync(it => it.IdDocumentTemplateItem == item.IdDocumentTemplateItem);
            }

            return items.ToList();
        }
    }
}