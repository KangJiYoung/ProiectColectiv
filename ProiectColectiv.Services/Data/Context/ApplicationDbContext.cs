using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProiectColectiv.Core.DomainModel.Entities;

namespace ProiectColectiv.Services.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Document> Documents { get; set; }

        public DbSet<DocumentData> DocumentDatas { get; set; }

        public DbSet<DocumentDataTemplate> DocumentDataTemplates { get; set; }

        public DbSet<DocumentDataUpload> DocumentDataUploads { get; set; }

        public DbSet<DocumentDataTemplateItem> DocumentDataTemplateItems { get; set; }

        public DbSet<DocumentState> DocumentStates { get; set; }

        public DbSet<DocumentTag> DocumentTags { get; set; }

        public DbSet<DocumentTaskTemplate> DocumentTaskTemplates { get; set; }

        public DbSet<DocumentTaskType> DocumentTaskTypes { get; set; }

        public DbSet<DocumentTaskTypePath> DocumentTaskTypePaths { get; set; }

        public DbSet<DocumentTemplate> DocumentTemplates { get; set; }

        public DbSet<DocumentTemplateItem> DocumentTemplateItems { get; set; }

        public DbSet<DocumentTemplateItemValue> DocumentTemplateItemValues { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
