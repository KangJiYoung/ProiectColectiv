using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProiectColectiv.Core.Interfaces;
using ProiectColectiv.Services.Data.Context;

namespace ProiectColectiv.Services
{
    public class TagsService : ITagsService
    {
        private readonly ApplicationDbContext dbContext;

        public TagsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<string>> GetTagsByUserId(string userId)
        {
            return dbContext
                .DocumentTags
                .Include(it => it.Document).Include(it => it.Tag)
                .Where(it => it.Document.UserId == userId)
                .Select(it => it.Tag.Name)
                .Distinct()
                .ToListAsync();
        }
    }
}
