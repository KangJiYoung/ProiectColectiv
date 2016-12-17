﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProiectColectiv.Core.DomainModel.Entities;

namespace ProiectColectiv.Services.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Document> Documents { get; set; }

        public DbSet<DocumentState> DocumentStates { get; set; }

        public DbSet<DocumentUploadState> DocumentUploadStates { get; set; }

        public DbSet<DocumentTemplateState> DocumentTemplateStates { get; set; }

        public DbSet<DocumentTemplateStateItem> DocumentTemplateStateItems { get; set; }

        public DbSet<DocumentTag> DocumentTags { get; set; }

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
