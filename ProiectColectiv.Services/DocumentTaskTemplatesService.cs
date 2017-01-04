using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces;
using ProiectColectiv.Services.Data.Context;

namespace ProiectColectiv.Services
{
    public class DocumentTaskTemplatesService : IDocumentTaskTemplatesService
    {
        private readonly ApplicationDbContext dbContext;

        public DocumentTaskTemplatesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<DocumentTaskTemplate>> GetAll()
        {
            return await dbContext
                .DocumentTaskTemplates
                .ToListAsync();
        }
    }
}