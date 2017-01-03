using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.DomainModel.Enums;
using ProiectColectiv.Core.Interfaces;
using ProiectColectiv.Services.Data.Context;

namespace ProiectColectiv.Services
{
    public class DocumentsTemplateItemService : IDocumentsTemplateItemService
    {
        private readonly ApplicationDbContext dbContext;

        public DocumentsTemplateItemService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<DocumentTemplateItem>> GetItemsFromTemplate(int idTemplate)
        {
            return dbContext
                .DocumentTemplateItems
                .Include(it => it.DocumentTemplateItemValues)
                .Where(it => it.IdDocumentTemplate == idTemplate)
                .ToListAsync();
        }
    }
}
