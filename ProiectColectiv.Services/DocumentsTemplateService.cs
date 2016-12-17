using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces;
using ProiectColectiv.Services.Data.Context;

namespace ProiectColectiv.Services
{
    public class DocumentsTemplateService : IDocumentsTemplateService
    {
        private readonly ApplicationDbContext dbContext;

        public DocumentsTemplateService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<DocumentTemplate>> GetAllTemplates()
        {
            return dbContext
                .DocumentTemplates
                .ToListAsync();
        }

        public Task<DocumentTemplate> GetTemplateById(int idTemplate)
        {
            return dbContext
                .DocumentTemplates
                .FirstAsync(it => it.IdDocumentTemplate == idTemplate);
        }
    }
}
